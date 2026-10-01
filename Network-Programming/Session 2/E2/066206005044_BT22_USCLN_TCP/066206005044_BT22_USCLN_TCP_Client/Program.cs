using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("MSSV: 066206005044 - BT2.2 - TCP Client tim USCLN");

Console.Write("Nhap IP Server (mac dinh 127.0.0.1): ");
string? host = Console.ReadLine();
if (string.IsNullOrWhiteSpace(host))
{
	host = "127.0.0.1";
}

Console.Write("Nhap port Server (mac dinh 5000): ");
string? portText = Console.ReadLine();
int port = 5000;
if (!string.IsNullOrWhiteSpace(portText) && (!int.TryParse(portText, out port) || port <= 0 || port > 65535))
{
	Console.WriteLine("Port khong hop le.");
	return;
}

int a = ReadPositiveInt("a");
int b = ReadPositiveInt("b");

try
{
	using TcpClient client = new TcpClient();
	client.Connect(host, port);

	Console.WriteLine("Da ket noi server: " + host + ":" + port);
	Console.WriteLine("LocalEndPoint: " + client.Client.LocalEndPoint);
	Console.WriteLine("RemoteEndPoint: " + client.Client.RemoteEndPoint);

	using NetworkStream stream = client.GetStream();
	using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
	using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

	writer.WriteLine(a + ";" + b);

	string? response = reader.ReadLine();
	if (response == null)
	{
		Console.WriteLine("Khong nhan duoc phan hoi tu server.");
	}
	else if (response.StartsWith("ERROR:"))
	{
		Console.WriteLine("Server bao loi: " + response);
	}
	else
	{
		Console.WriteLine("USCLN(" + a + ", " + b + ") = " + response);
	}
}
catch (Exception ex)
{
	Console.WriteLine("Khong ket noi hoac gui nhan duoc: " + ex.Message);
}

static int ReadPositiveInt(string variableName)
{
	int value;

	do
	{
		Console.Write("Nhap so nguyen duong " + variableName + ": ");
	}
	while (!int.TryParse(Console.ReadLine(), out value) || value <= 0);

	return value;
}
