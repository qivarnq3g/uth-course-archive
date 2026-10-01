using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("MSSV: 066206005044 - BT4.1 - UDPChat41 Client");
Console.Write("Nhap IP server (mac dinh 127.0.0.1): ");
string host = (Console.ReadLine() ?? string.Empty).Trim();
if (host.Length == 0)
{
	host = "127.0.0.1";
}

Console.Write("Nhap port server (mac dinh 5000): ");
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

IPEndPoint serverEndPoint;
try
{
	IPAddress ip = IPAddress.Parse(host);
	serverEndPoint = new IPEndPoint(ip, port);
}
catch
{
	Console.WriteLine("IP server khong hop le.");
	return;
}

using UdpClient client = new UdpClient();
Console.WriteLine("Da san sang chat UDP. Nhap EXIT de ket thuc.");

while (true)
{
	string message;
	do
	{
		Console.Write("[Client] Nhap tin nhan: ");
		message = (Console.ReadLine() ?? string.Empty).Trim();
	}
	while (message.Length == 0);

	byte[] sendBuffer = Encoding.UTF8.GetBytes(message);
	await client.SendAsync(sendBuffer, serverEndPoint);

	UdpReceiveResult response = await client.ReceiveAsync();
	string serverMessage = Encoding.UTF8.GetString(response.Buffer).Trim();
	Console.WriteLine("[Server " + response.RemoteEndPoint + "] " + serverMessage);

	if (message.Equals("EXIT", StringComparison.OrdinalIgnoreCase) ||
		serverMessage.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
	{
		Console.WriteLine("Phien chat da ket thuc.");
		break;
	}
}
