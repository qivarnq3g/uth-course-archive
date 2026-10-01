using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("MSSV: 066206005044 - BT3.1 - TimeTCPServer");
Console.Write("Nhap port (mac dinh 5000): ");
string? portText = Console.ReadLine();

int port = 5000;
if (!string.IsNullOrWhiteSpace(portText))
{
	if (!int.TryParse(portText, out port) || port <= 0 || port > 65535)
	{
		Console.WriteLine("Port khong hop le.");
		return;
	}
}

TcpListener listener = new TcpListener(IPAddress.Any, port);
listener.Start();

Console.WriteLine("Server dang lang nghe tai cong " + port);
Console.WriteLine("LocalEndPoint: " + listener.LocalEndpoint);

while (true)
{
	TcpClient client = await listener.AcceptTcpClientAsync();
	_ = HandleClientAsync(client);
}

static async Task HandleClientAsync(TcpClient client)
{
	string remoteEndPoint = client.Client.RemoteEndPoint?.ToString() ?? "Unknown";
	string localEndPoint = client.Client.LocalEndPoint?.ToString() ?? "Unknown";

	try
	{
		Console.WriteLine("Client ket noi: " + remoteEndPoint + " -> " + localEndPoint);

		using NetworkStream stream = client.GetStream();
		using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
		using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

		while (true)
		{
			string? request = await reader.ReadLineAsync();
			if (request == null)
			{
				break;
			}

			string command = request.Trim();
			string response;

			if (command == "1")
			{
				response = DateTime.Now.ToString("HH:mm:ss");
			}
			else if (command == "2")
			{
				response = DateTime.Now.ToString("dd/MM/yyyy");
			}
			else if (command == "3")
			{
				response = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
			}
			else if (command.Equals("BYE", StringComparison.OrdinalIgnoreCase))
			{
				response = "Da ngat ket noi.";
				await writer.WriteLineAsync(response);
				break;
			}
			else
			{
				response = "Yeu cau khong hop le. Dung 1, 2 hoac 3.";
			}

			await writer.WriteLineAsync(response);
			Console.WriteLine("Request=" + command + " | Response=" + response);
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine("Loi xu ly client: " + ex.Message);
	}
	finally
	{
		client.Close();
		Console.WriteLine("Client da ngat: " + remoteEndPoint);
	}
}
