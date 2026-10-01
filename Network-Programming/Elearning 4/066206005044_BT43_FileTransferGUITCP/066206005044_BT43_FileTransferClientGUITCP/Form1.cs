using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _066206005044_BT43_FileTransferClientGUITCP;

public partial class Form1 : Form
{
    private string sendFilePath = string.Empty;

    public Form1()
    {
        InitializeComponent();
    }

    private void buttonBrowseSend_Click(object? sender, EventArgs e)
    {
        using OpenFileDialog dialog = new OpenFileDialog();
        dialog.Title = "Chon file gui";
        dialog.Filter = "All files (*.*)|*.*";
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            sendFilePath = dialog.FileName;
            textSendFile.Text = sendFilePath;
        }
    }

    private async void buttonSend_Click(object? sender, EventArgs e)
    {
        if (sendFilePath.Length == 0 || !File.Exists(sendFilePath))
        {
            MessageBox.Show("Hay chon file gui hop le.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!IPAddress.TryParse(textServerIp.Text.Trim(), out IPAddress? serverIp))
        {
            MessageBox.Show("Server IP khong hop le.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textServerIp.Focus();
            return;
        }

        buttonSend.Enabled = false;
        progressSend.Value = 0;

        try
        {
            int serverPort = (int)numericPort.Value;
            using TcpClient client = new TcpClient();
            await client.ConnectAsync(serverIp, serverPort);

            using NetworkStream stream = client.GetStream();
            using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, true);
            using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true);

            FileInfo fileInfo = new FileInfo(sendFilePath);
            writer.Write(fileInfo.Name);
            writer.Write(fileInfo.Length);
            writer.Flush();

            byte[] buffer = new byte[81920];
            long sent = 0;

            using FileStream fileStream = new FileStream(sendFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, buffer.Length, true);
            int read;
            while ((read = await fileStream.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
            {
                await stream.WriteAsync(buffer.AsMemory(0, read));
                sent += read;

                int percent = fileInfo.Length == 0 ? 100 : (int)(sent * 100 / fileInfo.Length);
                progressSend.Value = Math.Max(0, Math.Min(100, percent));
            }

            await stream.FlushAsync();

            string ack = reader.ReadString();
            if (ack == "OK")
            {
                labelStatus.Text = "Status: Gui file thanh cong";
                MessageBox.Show("Gui file thanh cong.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                labelStatus.Text = "Status: Gui file chua hoan tat";
                MessageBox.Show("Server bao gui file chua hoan tat.", "Canh bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            labelStatus.Text = "Status: Send error";
            MessageBox.Show("Gui file that bai: " + ex.Message, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            buttonSend.Enabled = true;
        }
    }
}
