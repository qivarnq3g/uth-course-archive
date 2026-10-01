using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("MSSV: 066206005044 - BT2.3 - TCP Chat 2 Client (DoubleChatTCP)");
Console.WriteLine("Chon che do: S = Server, C = Client");
Console.Write("Nhap che do: ");

string mode = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

if (mode == "S")
{
	await RunServerAsync();
}
else if (mode == "C")
{
	await RunClientAsync();
}
else
{
	Console.WriteLine("Che do khong hop le.");
}

static async Task RunServerAsync()
{
	const int port = 9999;
	TcpListener listener = new TcpListener(IPAddress.Any, port);
	listener.Start();

	Console.WriteLine("[Server] Dang lang nghe cong 9999");
	Console.WriteLine("[Server] LocalEndPoint: " + listener.LocalEndpoint);
	Console.WriteLine("[Server] Dang doi Client 1...");
	TcpClient client1 = await listener.AcceptTcpClientAsync();
	Console.WriteLine("[Server] Client 1 da ket noi");
	Console.WriteLine("[Server] Client 1 LocalEndPoint: " + client1.Client.LocalEndPoint);
	Console.WriteLine("[Server] Client 1 RemoteEndPoint: " + client1.Client.RemoteEndPoint);

	Console.WriteLine("[Server] Dang doi Client 2...");
	TcpClient client2 = await listener.AcceptTcpClientAsync();
	Console.WriteLine("[Server] Client 2 da ket noi");
	Console.WriteLine("[Server] Client 2 LocalEndPoint: " + client2.Client.LocalEndPoint);
	Console.WriteLine("[Server] Client 2 RemoteEndPoint: " + client2.Client.RemoteEndPoint);

	listener.Stop();

	NetworkStream stream1 = client1.GetStream();
	NetworkStream stream2 = client2.GetStream();
	StreamReader reader1 = new StreamReader(stream1, Encoding.UTF8);
	StreamWriter writer1 = new StreamWriter(stream1, Encoding.UTF8) { AutoFlush = true };
	StreamReader reader2 = new StreamReader(stream2, Encoding.UTF8);
	StreamWriter writer2 = new StreamWriter(stream2, Encoding.UTF8) { AutoFlush = true };

	await writer1.WriteLineAsync("[Server] Ban la Client 1. Da san sang chat voi Client 2.");
	await writer2.WriteLineAsync("[Server] Ban la Client 2. Da san sang chat voi Client 1.");

	CancellationTokenSource cts = new CancellationTokenSource();

	Task relay1To2 = RelayAsync(reader1, writer2, "Client 1", cts.Token);
	Task relay2To1 = RelayAsync(reader2, writer1, "Client 2", cts.Token);

	Task firstEnd = await Task.WhenAny(relay1To2, relay2To1);
	cts.Cancel();

	try
	{
		await writer1.WriteLineAsync("[Server] Phien chat ket thuc.");
	}
	catch
	{
	}

	try
	{
		await writer2.WriteLineAsync("[Server] Phien chat ket thuc.");
	}
	catch
	{
	}

	reader1.Dispose();
	writer1.Dispose();
	reader2.Dispose();
	writer2.Dispose();
	client1.Close();
	client2.Close();

	if (firstEnd.Exception != null)
	{
		Console.WriteLine("[Server] Loi relay: " + firstEnd.Exception.GetBaseException().Message);
	}

	Console.WriteLine("[Server] Da dong.");
}

static async Task RelayAsync(StreamReader reader, StreamWriter targetWriter, string fromName, CancellationToken token)
{
	while (!token.IsCancellationRequested)
	{
		string? message = await reader.ReadLineAsync(token);
		if (message == null)
		{
			break;
		}

		string line = "[" + fromName + "] " + message;
		await targetWriter.WriteLineAsync(line);
		Console.WriteLine("[Server] " + line);
	}
}

static async Task RunClientAsync()
{
	Console.Write("Nhap IP server (mac dinh 127.0.0.1): ");
	string host = (Console.ReadLine() ?? string.Empty).Trim();
	if (host.Length == 0)
	{
		host = "127.0.0.1";
	}

	const int port = 9999;

	TcpClient client = new TcpClient();

	try
	{
		await client.ConnectAsync(host, port);
		Console.WriteLine("[Client] Da ket noi den server " + host + ":" + port);
		Console.WriteLine("[Client] LocalEndPoint: " + client.Client.LocalEndPoint);
		Console.WriteLine("[Client] RemoteEndPoint: " + client.Client.RemoteEndPoint);

		NetworkStream stream = client.GetStream();
		StreamReader reader = new StreamReader(stream, Encoding.UTF8);
		StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

		CancellationTokenSource cts = new CancellationTokenSource();

		Task receiveTask = Task.Run(async () =>
		{
			while (!cts.Token.IsCancellationRequested)
			{
				string? incoming = await reader.ReadLineAsync(cts.Token);
				if (incoming == null)
				{
					Console.WriteLine("[Client] Mat ket noi toi server.");
					break;
				}

				Console.WriteLine(incoming);
			}
		}, cts.Token);

		Console.WriteLine("[Client] Nhap tin nhan va nhan Enter de gui. Nhap /exit de thoat.");

		while (true)
		{
			string? text = Console.ReadLine();
			if (text == null)
			{
				continue;
			}

			if (string.Equals(text.Trim(), "/exit", StringComparison.OrdinalIgnoreCase))
			{
				break;
			}

			await writer.WriteLineAsync(text);
		}

		cts.Cancel();
		try
		{
			await receiveTask;
		}
		catch
		{
		}

		reader.Dispose();
		writer.Dispose();
	}
	catch (Exception ex)
	{
		Console.WriteLine("[Client] Khong ket noi duoc: " + ex.Message);
	}
	finally
	{
		client.Close();
	}
}
