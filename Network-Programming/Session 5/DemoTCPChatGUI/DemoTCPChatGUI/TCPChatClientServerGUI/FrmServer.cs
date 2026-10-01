using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TCPChatClientServerGUI
{
    public partial class FrmServer : Form
    {
        public FrmServer()
        {
            InitializeComponent();
            FormClosing += FrmServer_FormClosing;
        }

        UdpClient serverSocket;
        IPEndPoint activeClientEndPoint;
        CancellationTokenSource receiveCancelSource;
        Task receiveTask;
        bool isServerRunning;

        private void butKhoitao_Click(object sender, EventArgs e)
        {
            if (isServerRunning)
            {
                stopServerSide();
                return;
            }

            int port = (int)numServerPort.Value;
            try
            {
                stopServerSide();
                serverSocket = new UdpClient(port);
                activeClientEndPoint = null;
                isServerRunning = true;
                lbTrangThai.Text = "Dang cho client gui tin ...";
                butKhoitao.Text = "Dung server";
                receiveCancelSource = new CancellationTokenSource();
                receiveTask = receiveServerLoopAsync(receiveCancelSource.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khong the khoi tao server: " + ex.Message);
                stopServerSide();
            }
        }

        async Task receiveServerLoopAsync(CancellationToken token)
        {
            UdpClient socket = serverSocket;
            if (socket == null)
            {
                return;
            }

            try
            {
                while (!token.IsCancellationRequested)
                {
                    UdpReceiveResult result = await socket.ReceiveAsync();
                    activeClientEndPoint = result.RemoteEndPoint;
                    string thongdiep = Encoding.UTF8.GetString(result.Buffer).Trim();
                    appendChatSafe("Client: " + thongdiep);

                    if (thongdiep.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                    {
                        byte[] exitBuffer = Encoding.UTF8.GetBytes("EXIT");
                        await socket.SendAsync(exitBuffer, exitBuffer.Length, activeClientEndPoint);
                        beginStopServerFromWorker("Da dung server.");
                        break;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (SocketException)
            {
            }
            catch (Exception ex)
            {
                updateStatusSafe("Loi nhan du lieu.");
                appendChatSafe("He thong: " + ex.Message);
            }
        }

        delegate void CapNhatGiaoDien(string s);
        void CapNhatTrangThai(string s)
        {
            lbTrangThai.Text = s;
        }
        void CapNhatNoiDungChat(string s)
        {
            txtNoidungChat.Text += s + "\r\n";
        }

        async private void butGui_Click(object sender, EventArgs e)
        {
            if (!isServerRunning || serverSocket == null)
            {
                MessageBox.Show("Server chua khoi tao.");
                return;
            }

            if (activeClientEndPoint == null)
            {
                MessageBox.Show("Chua co client gui den.");
                return;
            }

            string thongdiep = txtThongdiep.Text.Trim();
            if (thongdiep.Length == 0)
            {
                return;
            }

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(thongdiep);
                await serverSocket.SendAsync(data, data.Length, activeClientEndPoint);
                CapNhatNoiDungChat("Server: " + thongdiep);
                txtThongdiep.Clear();

                if (thongdiep.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    stopServerSide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khong gui duoc: " + ex.Message);
            }
        }

        private void txtThongdiep_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
            {
                e.Handled = true;
                butGui.PerformClick();
            }
        }

        void stopServerSide()
        {
            if (receiveCancelSource != null)
            {
                try
                {
                    receiveCancelSource.Cancel();
                }
                catch
                {
                }

                receiveCancelSource.Dispose();
                receiveCancelSource = null;
            }

            if (serverSocket != null)
            {
                try
                {
                    serverSocket.Close();
                }
                catch
                {
                }

                serverSocket = null;
            }

            receiveTask = null;
            activeClientEndPoint = null;
            isServerRunning = false;
            butKhoitao.Text = "Khoi tao server";
            lbTrangThai.Text = "Da dung server.";
        }

        void updateStatusSafe(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(updateStatusSafe), text);
                return;
            }

            CapNhatTrangThai(text);
        }

        void appendChatSafe(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(appendChatSafe), text);
                return;
            }

            CapNhatNoiDungChat(text);
        }

        void beginStopServerFromWorker(string status)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(beginStopServerFromWorker), status);
                return;
            }

            stopServerSide();
            CapNhatTrangThai(status);
        }

        private void FrmServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopServerSide();
        }
    }
}
