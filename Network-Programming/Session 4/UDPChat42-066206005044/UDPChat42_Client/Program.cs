using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("MSSV: 066206005044 - BT4.2 - UDPChat42 Client");
Console.Write("Nhap IP server (mac dinh 127.0.0.1): ");
string host = (Console.ReadLine() ?? string.Empty).Trim();
if (host.Length == 0)
{
	host = "127.0.0.1";
}

Console.Write("Nhap port server (mac dinh 6000): ");
string? portText = Console.ReadLine();

int port = 6000;
if (!string.IsNullOrWhiteSpace(portText))
{
	if (!int.TryParse(portText, out port) || port <= 0 || port > 65535)
	{
		Console.WriteLine("Port khong hop le.");
		return;
	}
}

Console.Write("Nhap nickname cua ban: ");
string nickname = (Console.ReadLine() ?? string.Empty).Trim();
if (nickname.Length == 0)
{
	Console.WriteLine("Nickname khong duoc rong.");
	return;
}

IPEndPoint serverEndPoint;
try
{
	serverEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
}
catch
{
	Console.WriteLine("IP server khong hop le.");
	return;
}

using UdpClient client = new UdpClient();

await SendTextAsync(client, serverEndPoint, "REGISTER:" + nickname);
UdpReceiveResult registerResult = await client.ReceiveAsync();
string registerResponse = Encoding.UTF8.GetString(registerResult.Buffer).Trim();
Console.WriteLine("[Server] " + registerResponse);

if (!registerResponse.StartsWith("OK:", StringComparison.OrdinalIgnoreCase))
{
	return;
}

CancellationTokenSource cts = new CancellationTokenSource();

Task receiveTask = Task.Run(async () =>
{
	while (!cts.Token.IsCancellationRequested)
	{
		try
		{
			UdpReceiveResult response = await client.ReceiveAsync(cts.Token);
			string text = Encoding.UTF8.GetString(response.Buffer).Trim();

			if (text.StartsWith("MSG:", StringComparison.OrdinalIgnoreCase))
			{
				string payload = text.Substring("MSG:".Length);
				int index = payload.IndexOf(':');
				if (index > 0 && index < payload.Length - 1)
				{
					string fromNickname = payload.Substring(0, index);
					string content = payload.Substring(index + 1);
					Console.WriteLine("\n[" + fromNickname + "] " + content);
				}
				else
				{
					Console.WriteLine("\n[Server] " + text);
				}
			}
			else
			{
				Console.WriteLine("\n[Server] " + text);
			}
		}
		catch
		{
			break;
		}
	}
});

Console.WriteLine("Dang chat voi nickname: " + nickname);
Console.WriteLine("Nhap ten nguoi nhan va noi dung. Nhap EXIT de thoat.");

while (true)
{
	Console.Write("Nguoi nhan: ");
	string toNickname = (Console.ReadLine() ?? string.Empty).Trim();

	if (toNickname.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
	{
		await SendTextAsync(client, serverEndPoint, "EXIT");
		break;
	}

	if (toNickname.Length == 0)
	{
		Console.WriteLine("Ten nguoi nhan khong duoc rong.");
		continue;
	}

	Console.Write("Noi dung: ");
	string content = (Console.ReadLine() ?? string.Empty).Trim();
	if (content.Length == 0)
	{
		Console.WriteLine("Noi dung khong duoc rong.");
		continue;
	}

	string sendCommand = "SEND:" + toNickname + ":" + content;
	await SendTextAsync(client, serverEndPoint, sendCommand);
}

cts.Cancel();
client.Close();
try
{
	await receiveTask;
}
catch
{
}

static async Task SendTextAsync(UdpClient udp, IPEndPoint endPoint, string message)
{
	byte[] buffer = Encoding.UTF8.GetBytes(message);
	await udp.SendAsync(buffer, endPoint);
}
