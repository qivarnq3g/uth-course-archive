using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("MSSV: 066206005044 - BT2.2 - TCP Server tim USCLN");
Console.Write("Nhap port lang nghe (mac dinh 5000): ");
string? portText = Console.ReadLine();

int port = 5000;
if (!string.IsNullOrWhiteSpace(portText) && (!int.TryParse(portText, out port) || port <= 0 || port > 65535))
{
	Console.WriteLine("Port khong hop le.");
	return;
}

TcpListener listener = new TcpListener(IPAddress.Any, port);
listener.Start();

Console.WriteLine("Server dang chay tai port " + port);
Console.WriteLine("Nhan Ctrl+C de dung.");

while (true)
{
	TcpClient client = listener.AcceptTcpClient();

	try
	{
		IPEndPoint? local = client.Client.LocalEndPoint as IPEndPoint;
		IPEndPoint? remote = client.Client.RemoteEndPoint as IPEndPoint;
		Console.WriteLine("Client ket noi: " + remote + " -> " + local);

		using NetworkStream stream = client.GetStream();
		using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
		using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

		string? request = reader.ReadLine();
		if (request == null)
		{
			writer.WriteLine("ERROR: Khong nhan duoc du lieu.");
			continue;
		}

		string[] parts = request.Split(';');
		if (parts.Length != 2)
		{
			writer.WriteLine("ERROR: Dinh dang du lieu phai la a;b");
			continue;
		}

		if (!int.TryParse(parts[0], out int a) || !int.TryParse(parts[1], out int b) || a <= 0 || b <= 0)
		{
			writer.WriteLine("ERROR: a va b phai la so nguyen duong");
			continue;
		}

		int gcd = FindGcdBySubtraction(a, b);
		writer.WriteLine(gcd);
		Console.WriteLine("Da xu ly: a=" + a + ", b=" + b + ", uscln=" + gcd);
	}
	catch (Exception ex)
	{
		Console.WriteLine("Loi xu ly client: " + ex.Message);
	}
	finally
	{
		client.Close();
	}
}

static int FindGcdBySubtraction(int a, int b)
{
	while (a != b)
	{
		if (a > b)
		{
			a = a - b;
		}
		else
		{
			b = b - a;
		}
	}

	return a;
}
