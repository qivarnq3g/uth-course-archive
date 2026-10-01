using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _066206005044_BT21_SIMPLECHAT;

public partial class Form1 : Form
{
    bool isServerMode = true;
    bool isServerRunning = false;
    bool isServerClientConnected = false;
    bool isClientConnected = false;

    TcpListener? serverListener;
    TcpClient? serverClient;
    TcpClient? clientSocket;
    StreamReader? networkReader;
    StreamWriter? networkWriter;
    CancellationTokenSource? receiveCancelSource;
    Task? acceptTask;
    Task? receiveTask;

    public Form1()
    {
        InitializeComponent();
    }

    void formMain_Load(object? sender, EventArgs e)
    {
        initializeDefaultState();
        FormClosing += formMain_FormClosing;
    }

    void initializeDefaultState()
    {
        tabMode.SelectedIndex = 0;
        isServerMode = true;
        isServerRunning = false;
        isServerClientConnected = false;
        isClientConnected = false;
        textSend.Text = string.Empty;
        statusLabel.Text = "Status: Disconnected";
        updateControlState();
    }

    void updateControlState()
    {
        buttonStartService.Enabled = isServerMode && !isServerRunning;
        buttonStopService.Enabled = isServerMode && isServerRunning;

        buttonConnect.Enabled = !isServerMode && !isClientConnected;
        buttonDisconnect.Enabled = !isServerMode && isClientConnected;

        buttonSend.Enabled = isServerMode ? isServerClientConnected : isClientConnected;
        textSend.Enabled = buttonSend.Enabled;
    }

    string getCurrentModeTag()
    {
        if (isServerMode)
        {
            return "Server";
        }

        return "Client";
    }

    void appendLog(string modeTag, string message)
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

    void appendLogSafe(string modeTag, string message)
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

    void setStatusSafe(string statusText)
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

    string getLogFilePath()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
    }

    void tabMode_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (isServerRunning || isClientConnected)
        {
            tabMode.SelectedIndex = isServerMode ? 0 : 1;
            return;
        }

        isServerMode = tabMode.SelectedIndex == 0;

        if (isServerMode)
        {
            statusLabel.Text = "Status: Server Mode";
        }
        else
        {
            statusLabel.Text = "Status: Client Mode";
        }

        updateControlState();
    }

    async void buttonStartService_Click(object? sender, EventArgs e)
    {
        try
        {
            int port = (int)numericServerPort.Value;
            serverListener = new TcpListener(IPAddress.Any, port);
            serverListener.Start();

            isServerRunning = true;
            isServerClientConnected = false;
            statusLabel.Text = "Status: Connected/Waiting for Connections";
            appendLog("Server", "Service started at port " + port);
            updateControlState();

            receiveCancelSource = new CancellationTokenSource();
            acceptTask = waitForClientAsync(receiveCancelSource.Token);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Cannot start service: " + ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            stopServerSide();
            updateControlState();
        }
    }

    void buttonStopService_Click(object? sender, EventArgs e)
    {
        stopServerSide();
        statusLabel.Text = "Status: Server stopped";
        appendLog("Server", "Service stopped");
        updateControlState();
    }

    async void buttonConnect_Click(object? sender, EventArgs e)
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

            IPAddress[] addresses = Dns.GetHostAddresses(serverHost);
            if (addresses.Length == 0)
            {
                MessageBox.Show("Cannot resolve server address.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clientSocket = new TcpClient();
            await clientSocket.ConnectAsync(addresses[0], serverPort);

            setupNetworkStreams(clientSocket);

            isClientConnected = true;
            statusLabel.Text = "Status: Connected/Waiting for Connections";
            appendLog("Client", "Connected to " + serverHost + ":" + serverPort);
            appendLog("Client", "LocalEndPoint: " + clientSocket.Client.LocalEndPoint);
            appendLog("Client", "RemoteEndPoint: " + clientSocket.Client.RemoteEndPoint);

            receiveCancelSource = new CancellationTokenSource();
            receiveTask = receiveLoopAsync("Server", receiveCancelSource.Token);

            updateControlState();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Cannot connect: " + ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            disconnectClientSide();
            updateControlState();
        }
    }

    void buttonDisconnect_Click(object? sender, EventArgs e)
    {
        disconnectClientSide();
        statusLabel.Text = "Status: Client disconnected";
        appendLog("Client", "Disconnected");
        updateControlState();
    }

    async void buttonSend_Click(object? sender, EventArgs e)
    {
        string message = textSend.Text.Trim();

        if (message.Length == 0)
        {
            MessageBox.Show("Please enter message.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textSend.Focus();
            return;
        }

        if (networkWriter == null)
        {
            MessageBox.Show("Network is not connected.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            await networkWriter.WriteLineAsync(message);
            appendLog(getCurrentModeTag(), message);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Cannot send message: " + ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        textSend.Clear();
        textSend.Focus();
    }

    void buttonLoad_Click(object? sender, EventArgs e)
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

    void buttonSave_Click(object? sender, EventArgs e)
    {
        string logPath = getLogFilePath();
        File.WriteAllText(logPath, richLog.Text);
        statusLabel.Text = "Status: Log saved to file";
        MessageBox.Show("Saved to log.txt", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    void buttonClear_Click(object? sender, EventArgs e)
    {
        DialogResult result = MessageBox.Show("Do you want to clear log?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            richLog.Clear();
            statusLabel.Text = "Status: Log cleared";
        }
    }

    void buttonExit_Click(object? sender, EventArgs e)
    {
        DialogResult result = MessageBox.Show("Do you want to exit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            Close();
        }
    }

    async Task waitForClientAsync(CancellationToken token)
    {
        if (serverListener == null)
        {
            return;
        }

        try
        {
            TcpClient acceptedClient = await serverListener.AcceptTcpClientAsync(token);
            serverClient = acceptedClient;
            setupNetworkStreams(serverClient);

            setStatusSafe("Status: Connected/Waiting for Connections");
            appendLogSafe("Server", "Client connected");
            appendLogSafe("Server", "LocalEndPoint: " + serverClient.Client.LocalEndPoint);
            appendLogSafe("Server", "RemoteEndPoint: " + serverClient.Client.RemoteEndPoint);
            isServerClientConnected = true;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(updateControlState));
            }
            else
            {
                updateControlState();
            }

            receiveTask = receiveLoopAsync("Client", token);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            appendLogSafe("Server", "Accept error: " + ex.Message);
        }
    }

    void setupNetworkStreams(TcpClient socket)
    {
        NetworkStream stream = socket.GetStream();
        networkReader = new StreamReader(stream, Encoding.UTF8);
        networkWriter = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
    }

    async Task receiveLoopAsync(string fromTag, CancellationToken token)
    {
        try
        {
            if (networkReader == null)
            {
                return;
            }

            while (!token.IsCancellationRequested)
            {
                string? line = await networkReader.ReadLineAsync(token);
                if (line == null)
                {
                    break;
                }

                appendLogSafe(fromTag, line);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            appendLogSafe("System", "Receive error: " + ex.Message);
        }
        finally
        {
            if (isServerMode)
            {
                disconnectPeerOnlyOnServer();

                if (serverListener != null && receiveCancelSource != null)
                {
                    setStatusSafe("Status: Connected/Waiting for Connections");
                    acceptTask = waitForClientAsync(receiveCancelSource.Token);
                }
            }
            else
            {
                disconnectClientSide();
            }

            if (!isServerMode)
            {
                setStatusSafe("Status: Disconnected");
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action(updateControlState));
            }
            else
            {
                updateControlState();
            }
        }
    }

    void disconnectPeerOnlyOnServer()
    {
        closeNetworkStreams();

        if (serverClient != null)
        {
            try
            {
                serverClient.Close();
            }
            catch
            {
            }

            serverClient = null;
        }

        isServerClientConnected = false;
    }

    void disconnectClientSide()
    {
        cancelReceiveLoop();
        closeNetworkStreams();

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

        isClientConnected = false;
    }

    void stopServerSide()
    {
        cancelReceiveLoop();
        disconnectPeerOnlyOnServer();

        if (serverListener != null)
        {
            try
            {
                serverListener.Stop();
            }
            catch
            {
            }

            serverListener = null;
        }

        isServerRunning = false;
        isServerClientConnected = false;
    }

    void cancelReceiveLoop()
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
    }

    void closeNetworkStreams()
    {
        if (networkReader != null)
        {
            try
            {
                networkReader.Dispose();
            }
            catch
            {
            }

            networkReader = null;
        }

        if (networkWriter != null)
        {
            try
            {
                networkWriter.Dispose();
            }
            catch
            {
            }

            networkWriter = null;
        }
    }

    void formMain_FormClosing(object? sender, FormClosingEventArgs e)
    {
        stopServerSide();
        disconnectClientSide();
    }
}