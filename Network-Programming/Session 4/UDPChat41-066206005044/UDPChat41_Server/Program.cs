using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("MSSV: 066206005044 - BT4.1 - UDPChat41 Server");
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

using UdpClient server = new UdpClient(port);
Console.WriteLine("Server UDP dang lang nghe cong " + port);
Console.WriteLine("Nhap EXIT de ket thuc.");

while (true)
{
	UdpReceiveResult received = await server.ReceiveAsync();
	string clientMessage = Encoding.UTF8.GetString(received.Buffer).Trim();
	IPEndPoint clientEndPoint = received.RemoteEndPoint;

	Console.WriteLine("\n[Client " + clientEndPoint + "] " + clientMessage);

	if (clientMessage.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
	{
		byte[] exitBuffer = Encoding.UTF8.GetBytes("EXIT");
		await server.SendAsync(exitBuffer, clientEndPoint);
		Console.WriteLine("Nhan EXIT tu client. Server ket thuc.");
		break;
	}

	string serverMessage;
	do
	{
		Console.Write("[Server] Nhap tin nhan: ");
		serverMessage = (Console.ReadLine() ?? string.Empty).Trim();
	}
	while (serverMessage.Length == 0);

	byte[] sendBuffer = Encoding.UTF8.GetBytes(serverMessage);
	await server.SendAsync(sendBuffer, clientEndPoint);

	if (serverMessage.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
	{
		Console.WriteLine("Server da gui EXIT. Ket thuc chuong trinh.");
		break;
	}
}
