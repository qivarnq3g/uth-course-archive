using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _066206005044_BT41_SimpleChatGUIUDP;

public partial class Form1 : Form
{
    private bool isServerMode = true;
    private bool isServerRunning;
    private bool isClientConnected;

    private UdpClient? serverSocket;
    private UdpClient? clientSocket;
    private IPEndPoint? activeClientEndPoint;
    private IPEndPoint? configuredServerEndPoint;
    private CancellationTokenSource? receiveCancelSource;
    private Task? receiveTask;

    public Form1()
    {
        InitializeComponent();
    }

    private void formMain_Load(object? sender, EventArgs e)
    {
        initializeDefaultState();
        FormClosing += formMain_FormClosing;
    }

    private void initializeDefaultState()
    {
        tabMode.SelectedIndex = 0;
        isServerMode = true;
        isServerRunning = false;
        isClientConnected = false;
        textSend.Text = string.Empty;
        comboServerIp.Text = "127.0.0.1";
        numericServerPort.Value = 5000;
        numericClientPort.Value = 5000;
        statusLabel.Text = "Status: Disconnected";
        updateControlState();
    }

    private void updateControlState()
    {
        buttonStartService.Enabled = isServerMode && !isServerRunning;
        buttonStopService.Enabled = isServerMode && isServerRunning;

        buttonConnect.Enabled = !isServerMode && !isClientConnected;
        buttonDisconnect.Enabled = !isServerMode && isClientConnected;

        buttonSend.Enabled = isServerMode ? isServerRunning : isClientConnected;
        textSend.Enabled = buttonSend.Enabled;
    }

    private string getCurrentModeTag()
    {
        return isServerMode ? "Server" : "Client";
    }

    private void appendLog(string modeTag, string message)
    {
        string line = DateTime.Now.ToString("hh:mm tt") + ": [" + modeTag + "] " + message;

        if (richLog.TextLength == 0)
        {
            richLog.Text = line;
        }
        else
        {
            richLog.AppendText(Environment.NewLine + line);
        }
    }

    private void appendLogSafe(string modeTag, string message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => appendLog(modeTag, message)));
        }
        else
        {
            appendLog(modeTag, message);
        }
    }

    private void setStatusSafe(string statusText)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => statusLabel.Text = statusText));
        }
        else
        {
            statusLabel.Text = statusText;
        }
    }

    private string getLogFilePath()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
    }

    private void tabMode_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (isServerRunning || isClientConnected)
        {
            tabMode.SelectedIndex = isServerMode ? 0 : 1;
            return;
        }

        isServerMode = tabMode.SelectedIndex == 0;
        statusLabel.Text = isServerMode ? "Status: Server Mode" : "Status: Client Mode";
        updateControlState();
    }

    private void buttonStartService_Click(object? sender, EventArgs e)
    {
        try
        {
            int port = (int)numericServerPort.Value;

            stopServerSide();
            serverSocket = new UdpClient(port);
            activeClientEndPoint = null;

            isServerRunning = true;
            statusLabel.Text = "Status: Connected/Waiting for Connections";
            appendLog("Server", "Service started at port " + port);

            receiveCancelSource = new CancellationTokenSource();
            receiveTask = receiveServerLoopAsync(receiveCancelSource.Token);

            updateControlState();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Cannot start service: " + ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            stopServerSide();
            updateControlState();
        }
    }

    private void buttonStopService_Click(object? sender, EventArgs e)
    {
        stopServerSide();
        statusLabel.Text = "Status: Server stopped";
        appendLog("Server", "Service stopped");
        updateControlState();
    }

    private async void buttonConnect_Click(object? sender, EventArgs e)
    {
        try
        {
            string serverHost = comboServerIp.Text.Trim();
            int serverPort = (int)numericClientPort.Value;

            if (serverHost.Length == 0)
            {
                MessageBox.Show("Please enter server address.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboServerIp.Focus();
                return;
            }

            IPAddress[] addresses = await Dns.GetHostAddressesAsync(serverHost);
            IPAddress? ipv4Address = addresses.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            if (ipv4Address == null)
            {
                MessageBox.Show("Cannot resolve server address.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            disconnectClientSide();

            configuredServerEndPoint = new IPEndPoint(ipv4Address, serverPort);
            clientSocket = new UdpClient(0);

            isClientConnected = true;
            statusLabel.Text = "Status: Connected/Waiting for Connections";
            appendLog("Client", "Connected to " + configuredServerEndPoint);

            receiveCancelSource = new CancellationTokenSource();
            receiveTask = receiveClientLoopAsync(receiveCancelSource.Token);

            updateControlState();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Cannot connect: " + ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            disconnectClientSide();
            updateControlState();
        }
    }

    private void buttonDisconnect_Click(object? sender, EventArgs e)
    {
        disconnectClientSide();
        statusLabel.Text = "Status: Client disconnected";
        appendLog("Client", "Disconnected");
        updateControlState();
    }

    private async void buttonSend_Click(object? sender, EventArgs e)
    {
        string message = textSend.Text.Trim();
        if (message.Length == 0)
        {
            MessageBox.Show("Please enter message.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textSend.Focus();
            return;
        }

        byte[] sendBuffer = Encoding.UTF8.GetBytes(message);

        try
        {
            if (isServerMode)
            {
                if (!isServerRunning || serverSocket == null)
                {
                    MessageBox.Show("Server is not running.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (activeClientEndPoint == null)
                {
                    MessageBox.Show("No client endpoint available yet.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await serverSocket.SendAsync(sendBuffer, activeClientEndPoint);
                appendLog("Server", message);

                if (message.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    stopServerSide();
                    statusLabel.Text = "Status: Server stopped";
                }
            }
            else
            {
                if (!isClientConnected || clientSocket == null || configuredServerEndPoint == null)
                {
                    MessageBox.Show("Client is not connected.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await clientSocket.SendAsync(sendBuffer, configuredServerEndPoint);
                appendLog("Client", message);

                if (message.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    disconnectClientSide();
                    statusLabel.Text = "Status: Client disconnected";
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Cannot send message: " + ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        textSend.Clear();
        textSend.Focus();
        updateControlState();
    }

    private void buttonLoad_Click(object? sender, EventArgs e)
    {
        string logPath = getLogFilePath();
        if (!File.Exists(logPath))
        {
            MessageBox.Show("log.txt not found.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        richLog.Text = File.ReadAllText(logPath);
        statusLabel.Text = "Status: Log loaded from file";
    }

    private void buttonSave_Click(object? sender, EventArgs e)
    {
        string logPath = getLogFilePath();
        File.WriteAllText(logPath, richLog.Text);
        statusLabel.Text = "Status: Log saved to file";
        MessageBox.Show("Saved to log.txt", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void buttonClear_Click(object? sender, EventArgs e)
    {
        DialogResult result = MessageBox.Show("Do you want to clear log?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            richLog.Clear();
            statusLabel.Text = "Status: Log cleared";
        }
    }

    private void buttonExit_Click(object? sender, EventArgs e)
    {
        DialogResult result = MessageBox.Show("Do you want to exit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            Close();
        }
    }

    private async Task receiveServerLoopAsync(CancellationToken token)
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
                activeClientEndPoint = result.RemoteEndPoint;

                string message = Encoding.UTF8.GetString(result.Buffer).Trim();
                appendLogSafe("Client", message);

                if (message.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    byte[] exitBuffer = Encoding.UTF8.GetBytes("EXIT");
                    await serverSocket.SendAsync(exitBuffer, activeClientEndPoint);

                    if (InvokeRequired)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            stopServerSide();
                            statusLabel.Text = "Status: Server stopped";
                            updateControlState();
                        }));
                    }
                    else
                    {
                        stopServerSide();
                        statusLabel.Text = "Status: Server stopped";
                        updateControlState();
                    }

                    break;
                }
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
            appendLogSafe("System", "Receive error: " + ex.Message);
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    disconnectClientSide();
                    statusLabel.Text = "Status: Client disconnected";
                    updateControlState();
                }));
            }
            else
            {
                disconnectClientSide();
                statusLabel.Text = "Status: Client disconnected";
                updateControlState();
            }
        }
    }

    private async Task receiveClientLoopAsync(CancellationToken token)
    {
        if (clientSocket == null)
        {
            return;
        }

        try
        {
            while (!token.IsCancellationRequested)
            {
                UdpReceiveResult result = await clientSocket.ReceiveAsync(token);
                string message = Encoding.UTF8.GetString(result.Buffer).Trim();
                appendLogSafe("Server", message);

                if (message.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    if (InvokeRequired)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            disconnectClientSide();
                            statusLabel.Text = "Status: Client disconnected";
                            updateControlState();
                        }));
                    }
                    else
                    {
                        disconnectClientSide();
                        statusLabel.Text = "Status: Client disconnected";
                        updateControlState();
                    }

                    break;
                }
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
            appendLogSafe("System", "Receive error: " + ex.Message);
            setStatusSafe("Status: Disconnected");
        }
    }

    private void stopServerSide()
    {
        cancelReceiveLoop();

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

        activeClientEndPoint = null;
        isServerRunning = false;
    }

    private void disconnectClientSide()
    {
        cancelReceiveLoop();

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

        configuredServerEndPoint = null;
        isClientConnected = false;
    }

    private void cancelReceiveLoop()
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

        receiveTask = null;
    }

    private void formMain_FormClosing(object? sender, FormClosingEventArgs e)
    {
        stopServerSide();
        disconnectClientSide();
    }
}
