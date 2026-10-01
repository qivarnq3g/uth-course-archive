using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _066206005044_BT43_FileTransferServerGUITCP;

public partial class Form1 : Form
{
    private TcpListener? listener;
    private CancellationTokenSource? acceptCancelSource;
    private Task? acceptTask;
    private bool isRunning;
    private string receiveFilePath = string.Empty;

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
    }

    private void buttonBrowseReceive_Click(object? sender, EventArgs e)
    {
        using SaveFileDialog dialog = new SaveFileDialog();
        dialog.Title = "Chon file nhan";
        dialog.Filter = "All files (*.*)|*.*";
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            receiveFilePath = dialog.FileName;
            textReceivePath.Text = receiveFilePath;
        }
    }

    private void buttonStart_Click(object? sender, EventArgs e)
    {
        try
        {
            int port = (int)numericPort.Value;
            stopService();

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            acceptCancelSource = new CancellationTokenSource();
            acceptTask = acceptLoopAsync(acceptCancelSource.Token);

            isRunning = true;
            labelStatus.Text = "Status: Listening on port " + port;
            updateControlState();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Khong the khoi dong server: " + ex.Message, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            stopService();
            updateControlState();
        }
    }

    private void buttonStop_Click(object? sender, EventArgs e)
    {
        stopService();
        labelStatus.Text = "Status: Service stopped";
        updateControlState();
    }

    private async Task acceptLoopAsync(CancellationToken token)
    {
        if (listener == null)
        {
            return;
        }

        try
        {
            while (!token.IsCancellationRequested)
            {
                TcpClient client = await listener.AcceptTcpClientAsync(token);
                _ = Task.Run(() => handleClientAsync(client, token), token);
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
                BeginInvoke(new Action(() => labelStatus.Text = "Status: Accept error - " + ex.Message));
            }
        }
    }

    private async Task handleClientAsync(TcpClient client, CancellationToken token)
    {
        try
        {
            using (client)
            {
                using NetworkStream stream = client.GetStream();
                using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true);
                using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, true);

                string fileName = reader.ReadString();
                long fileLength = reader.ReadInt64();

                string outputPath = receiveFilePath;
                if (outputPath.Length == 0)
                {
                    outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "received_" + fileName);
                }

                string? outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrWhiteSpace(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                BeginInvoke(new Action(() =>
                {
                    progressReceive.Value = 0;
                    labelStatus.Text = "Status: Receiving " + fileName;
                }));

                byte[] buffer = new byte[81920];
                long totalRead = 0;

                using FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, buffer.Length, true);

                while (totalRead < fileLength && !token.IsCancellationRequested)
                {
                    int maxRead = (int)Math.Min(buffer.Length, fileLength - totalRead);
                    int read = await stream.ReadAsync(buffer.AsMemory(0, maxRead), token);
                    if (read <= 0)
                    {
                        break;
                    }

                    await fileStream.WriteAsync(buffer.AsMemory(0, read), token);
                    totalRead += read;

                    int percent = fileLength == 0 ? 100 : (int)(totalRead * 100 / fileLength);
                    BeginInvoke(new Action(() => progressReceive.Value = Math.Max(0, Math.Min(100, percent))));
                }

                writer.Write(totalRead == fileLength ? "OK" : "INCOMPLETE");
                writer.Flush();

                BeginInvoke(new Action(() =>
                {
                    labelStatus.Text = totalRead == fileLength
                        ? "Status: File received -> " + Path.GetFileName(outputPath)
                        : "Status: Receive incomplete";
                }));
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            BeginInvoke(new Action(() => labelStatus.Text = "Status: Receive error - " + ex.Message));
        }
    }

    private void stopService()
    {
        if (acceptCancelSource != null)
        {
            try
            {
                acceptCancelSource.Cancel();
            }
            catch
            {
            }

            acceptCancelSource.Dispose();
            acceptCancelSource = null;
        }

        if (listener != null)
        {
            try
            {
                listener.Stop();
            }
            catch
            {
            }

            listener = null;
        }

        acceptTask = null;
        isRunning = false;
    }

    private void formMain_FormClosing(object? sender, FormClosingEventArgs e)
    {
        stopService();
    }
}
