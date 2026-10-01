using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _066206005044_BT42_TimeClientGUIUDP;

public partial class Form1 : Form
{
    private UdpClient? clientSocket;

    public Form1()
    {
        InitializeComponent();
    }

    private void formMain_Load(object? sender, EventArgs e)
    {
        comboRequest.SelectedIndex = 1;
    }

    private async void buttonSendRequest_Click(object? sender, EventArgs e)
    {
        try
        {
            string ipText = textIp.Text.Trim();
            if (!IPAddress.TryParse(ipText, out IPAddress? ipAddress))
            {
                MessageBox.Show("IP khong hop le.", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textIp.Focus();
                return;
            }

            int port = (int)numericPort.Value;
            string selectedRequest = comboRequest.SelectedItem?.ToString() ?? string.Empty;
            if (selectedRequest.Length == 0)
            {
                MessageBox.Show("Hay chon yeu cau.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string requestCode = selectedRequest[0].ToString();

            if (clientSocket == null)
            {
                clientSocket = new UdpClient(0);
            }

            IPEndPoint serverEndPoint = new IPEndPoint(ipAddress, port);
            byte[] sendBuffer = Encoding.UTF8.GetBytes(requestCode);
            await clientSocket.SendAsync(sendBuffer, serverEndPoint);

            using CancellationTokenSource timeoutSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            UdpReceiveResult response = await clientSocket.ReceiveAsync(timeoutSource.Token);
            string resultText = Encoding.UTF8.GetString(response.Buffer).Trim();

            MessageBox.Show(resultText, "Ket qua", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (OperationCanceledException)
        {
            MessageBox.Show("Khong nhan duoc phan hoi tu server (timeout).", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Gui yeu cau that bai: " + ex.Message, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void buttonExit_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void formMain_FormClosing(object? sender, FormClosingEventArgs e)
    {
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
    }
}
