using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPChatClientServerGUI
{
    public partial class FrmClient : Form
    {
        public FrmClient()
        {
            InitializeComponent();
            FormClosing += FrmClient_FormClosing;
        }

        UdpClient clientSocket;
        IPEndPoint configuredServerEndPoint;
        CancellationTokenSource receiveCancelSource;
        Task receiveTask;
        bool isClientConnected;

        private void butKetnoi_Click(object sender, EventArgs e)
        {
            if (isClientConnected)
            {
                disconnectClientSide();
                return;
            }

            string ipText = txtServerIP.Text.Trim();
            IPAddress serverIp;
            if (!IPAddress.TryParse(ipText, out serverIp))
            {
                MessageBox.Show("IP server khong hop le.");
                txtServerIP.Focus();
                return;
            }

            int serverPort = (int)numServerPort.Value;

            try
            {
                disconnectClientSide();
                configuredServerEndPoint = new IPEndPoint(serverIp, serverPort);
                clientSocket = new UdpClient(0);
                isClientConnected = true;
                butKetnoi.Text = "Ngat ket noi";
                CapNhatTrangThai("Da ket noi den " + configuredServerEndPoint);
                receiveCancelSource = new CancellationTokenSource();
                receiveTask = receiveClientLoopAsync(receiveCancelSource.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khong the ket noi: " + ex.Message);
                disconnectClientSide();
            }
        }

        async Task receiveClientLoopAsync(CancellationToken token)
        {
            UdpClient socket = clientSocket;
            if (socket == null)
            {
                return;
            }

            try
            {
                while (!token.IsCancellationRequested)
                {
                    UdpReceiveResult result = await socket.ReceiveAsync();
                    string thongdiep = Encoding.UTF8.GetString(result.Buffer).Trim();
                    appendChatSafe("Server: " + thongdiep);

                    if (thongdiep.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                    {
                        beginDisconnectFromWorker("Da ngat ket noi.");
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
                appendChatSafe("He thong: " + ex.Message);
                updateStatusSafe("Loi nhan du lieu.");
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
            if (!isClientConnected || clientSocket == null || configuredServerEndPoint == null)
            {
                MessageBox.Show("Client chua ket noi.");
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
                await clientSocket.SendAsync(data, data.Length, configuredServerEndPoint);
                CapNhatNoiDungChat("Client: " + thongdiep);
                txtThongdiep.Clear();

                if (thongdiep.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    disconnectClientSide();
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

        void disconnectClientSide()
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

            if (clientSocket != null)
            {
                try
                {
                    clientSocket.Close();
                }
                catch
                {
                }

                clientSocket = null;
            }

            receiveTask = null;
            configuredServerEndPoint = null;
            isClientConnected = false;
            butKetnoi.Text = "Ket noi";
            CapNhatTrangThai("Chua ket noi");
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

        void beginDisconnectFromWorker(string status)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(beginDisconnectFromWorker), status);
                return;
            }

            disconnectClientSide();
            CapNhatTrangThai(status);
        }

        private void FrmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            disconnectClientSide();
        }

    }
}
