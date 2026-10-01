using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _066206005044_BT42_TimeServerGUIUDP;

public partial class Form1 : Form
{
    private UdpClient? serverSocket;
    private CancellationTokenSource? receiveCancelSource;
    private Task? receiveTask;
    private bool isRunning;

    public Form1()
    {
        InitializeComponent();
    }

    private void formMain_Load(object? sender, EventArgs e)
    {
        isRunning = false;
        updateControlState();
    }

    private void updateControlState()
    {
        buttonStart.Enabled = !isRunning;
        buttonStop.Enabled = isRunning;
        numericPort.Enabled = !isRunning;
        Text = isRunning
            ? "UDP Time Server is running on port " + numericPort.Value
            : "Time UDP Server";
    }

    private void buttonStart_Click(object? sender, EventArgs e)
    {
        try
        {
            int port = (int)numericPort.Value;
            stopServer();

            serverSocket = new UdpClient(port);
            receiveCancelSource = new CancellationTokenSource();
            receiveTask = receiveLoopAsync(receiveCancelSource.Token);

            isRunning = true;
            updateControlState();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Khong the khoi dong server: " + ex.Message, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            stopServer();
            updateControlState();
        }
    }

    private void buttonStop_Click(object? sender, EventArgs e)
    {
        stopServer();
        updateControlState();
    }

    private void buttonExit_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private async Task receiveLoopAsync(CancellationToken token)
    {
        if (serverSocket == null)
        {
            return;
        }

        try
        {
            while (!token.IsCancellationRequested)
            {
                UdpReceiveResult result = await serverSocket.ReceiveAsync(token);
                string request = Encoding.UTF8.GetString(result.Buffer).Trim();
                string responseText = buildTimeResponse(request);
                byte[] responseBuffer = Encoding.UTF8.GetBytes(responseText);
                await serverSocket.SendAsync(responseBuffer, result.RemoteEndPoint);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (ObjectDisposedException)
        {
        }
        catch (Exception ex)
        {
            if (!token.IsCancellationRequested)
            {
                BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Server receive error: " + ex.Message, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    stopServer();
                    updateControlState();
                }));
            }
        }
    }

    private static string buildTimeResponse(string request)
    {
        string normalized = request.Trim();
        if (normalized.StartsWith("1", StringComparison.Ordinal))
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        if (normalized.StartsWith("2", StringComparison.Ordinal))
        {
            return DateTime.Now.ToString("MM/dd/yyyy");
        }

        if (normalized.StartsWith("3", StringComparison.Ordinal))
        {
            return DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        }

        return "Yeu cau khong hop le";
    }

    private void stopServer()
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
        isRunning = false;
    }

    private void formMain_FormClosing(object? sender, FormClosingEventArgs e)
    {
        stopServer();
    }
}
