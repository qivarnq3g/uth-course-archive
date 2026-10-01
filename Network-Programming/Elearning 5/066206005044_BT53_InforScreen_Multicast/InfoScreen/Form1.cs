using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfoScreen
{
    public partial class Form1 : Form
    {
        private const string MULTICAST_ADDRESS = "239.0.0.1";
        private const int MULTICAST_PORT = 9099;
        private ArrayList groupList = new ArrayList();
        private Dictionary<string, ArrayList> groupMembers = new Dictionary<string, ArrayList>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmdStart.Enabled = true;
            cmdStop.Enabled = false;
            notifyIcon1.Icon = this.Icon;
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(1000);
            
            groupList.Add("admin");
            groupList.Add("user");
            comboGroups.Items.Add("admin");
            comboGroups.Items.Add("user");
            comboGroups.SelectedIndex = 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!existing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private bool existing = false;
        private void cmdExit_Click(object sender, EventArgs e)
        {
            existing = true;
            this.Close();
        }

        Socket sock = null;
        private void cmdStart_Click(object sender, EventArgs e)
        {
            sock = new Socket(SocketType.Dgram, ProtocolType.Udp);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, (int)numericUpDown1.Value);
            try
            {
                sock.Bind(ep);
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERROR: Cannot bind to this port!" + ex.Message);
                return;
            }
            
            MulticastOption mreq = new MulticastOption(IPAddress.Parse(MULTICAST_ADDRESS), IPAddress.Any);
            sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mreq);
            
            groupMembers["admin"] = new ArrayList();
            groupMembers["user"] = new ArrayList();
            
            cmdStart.Enabled = false;
            cmdStop.Enabled = true;
            sock.BeginReceiveFrom(buffer, 0, 1024, SocketFlags.None, ref fromEP,new AsyncCallback(handleMessage),sock);
        }
        EndPoint fromEP = new IPEndPoint(IPAddress.Any, 0);
        byte[] buffer = new byte[1024];
        ArrayList userList = new ArrayList();
        private void handleMessage(IAsyncResult result)
        {
            try
            {
                int dataSize = sock.EndReceiveFrom(result, ref fromEP);
                string s = Encoding.ASCII.GetString(buffer, 0, dataSize);
                string[] p = s.Split(new char[] { '#' });
                if (p[0]=="STATUS")
                {
                    if (!userList.Contains(p[1]))
                        userList.Add(p[1]);
                    this.Invoke(new UpDateUI(UpdateList));
                }
                else if (p[0] == "SENDALL")
                {
                    notifyIcon1.BalloonTipText = "[" + DateTime.Now + "] " + p[1];
                    notifyIcon1.ShowBalloonTip(1000);
                }
                else if (p[0] == "SENDUSER")
                {
                    if (p[1]==txtUsername.Text)
                    {
                        notifyIcon1.BalloonTipText = "[" + DateTime.Now + "] " + p[2] + ": "+p[3];
                        notifyIcon1.ShowBalloonTip(1000);
                    }
                }
                else if (p[0] == "JOINGROUP")
                {
                    JoinGroup(p[1], p[2]);
                }
                else if (p[0] == "SENDGROUP")
                {
                    if (groupMembers.ContainsKey(p[1]) && groupMembers[p[1]].Contains(txtUsername.Text))
                    {
                        notifyIcon1.BalloonTipText = "[" + DateTime.Now + "] GROUP:" + p[1] + " - " + p[2];
                        notifyIcon1.ShowBalloonTip(1000);
                    }
                }
                sock.BeginReceiveFrom(buffer, 0, 1024, SocketFlags.None, ref fromEP, new AsyncCallback(handleMessage), sock);
            }
            catch(Exception)
            {

            }
        }
        delegate void UpDateUI();
        void UpdateList()
        {
            lstUser.Items.Clear();
            foreach (string t in userList)
                lstUser.Items.Add(t);
        }
        
        private void cmdStop_Click(object sender, EventArgs e)
        {
            if (sock != null)
            {
                MulticastOption mreq = new MulticastOption(IPAddress.Parse(MULTICAST_ADDRESS), IPAddress.Any);
                sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, mreq);
                sock.Close();
                sock = null;
            }
            cmdStart.Enabled = true;
            cmdStop.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage("SENDALL#"+txtMessage.Text);
        }

        void SendMessage(string s)
        {
            Socket sendSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(MULTICAST_ADDRESS), MULTICAST_PORT);
            sendSocket.SendTo(Encoding.ASCII.GetBytes(s), ep);
        }
        
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            cmdExit_Click(null, null);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string s = "STATUS#"+txtUsername.Text;
            SendMessage(s);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            userList.Clear();
        }

        private void lstUser_Click(object sender, EventArgs e)
        {
            if (lstUser.SelectedItem != null)
                txtToUser.Text = lstUser.SelectedItem.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s = "SENDUSER#" + txtToUser.Text + "#" +txtUsername.Text+"#" + txtMessage.Text;
            SendMessage(s);
        }

        private void JoinGroup(string groupName, string userName)
        {
            if (groupMembers.ContainsKey(groupName))
            {
                if (!groupMembers[groupName].Contains(userName))
                    groupMembers[groupName].Add(userName);
            }
        }

        private void buttonJoinGroup_Click(object sender, EventArgs e)
        {
            string group = comboGroups.SelectedItem.ToString();
            JoinGroup(group, txtUsername.Text);
            SendMessage("JOINGROUP#" + group + "#" + txtUsername.Text);
            MessageBox.Show("Joined group: " + group);
        }

        private void buttonSendGroup_Click(object sender, EventArgs e)
        {
            string group = comboGroups.SelectedItem.ToString();
            SendMessage("SENDGROUP#" + group + "#" + txtMessage.Text);
        }
    }
}
