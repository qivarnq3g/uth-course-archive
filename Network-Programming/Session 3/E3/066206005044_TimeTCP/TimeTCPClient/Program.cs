using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("MSSV: 066206005044 - BT3.1 - TimeTCPClient");

TcpClient? client = null;
StreamReader? reader = null;
StreamWriter? writer = null;

while (true)
{
	Console.WriteLine();
	Console.WriteLine("===== MENU CLIENT =====");
	Console.WriteLine("1 - Ket noi den server");
	Console.WriteLine("2 - Gui yeu cau dich vu");
	Console.WriteLine("3 - Ngat ket noi");
	Console.WriteLine("0 - Thoat chuong trinh");
	Console.Write("Chon: ");
	string choice = (Console.ReadLine() ?? string.Empty).Trim();

	if (choice == "1")
	{
		if (client != null && client.Connected)
		{
			Console.WriteLine("Dang ket noi server roi.");
			continue;
		}

		Console.Write("Nhap dia chi server: ");
		string host = (Console.ReadLine() ?? string.Empty).Trim();
		if (host.Length == 0)
		{
			Console.WriteLine("Dia chi server khong duoc rong.");
			continue;
		}

		Console.Write("Nhap port server: ");
		string portText = (Console.ReadLine() ?? string.Empty).Trim();
		if (!int.TryParse(portText, out int port) || port <= 0 || port > 65535)
		{
			Console.WriteLine("Port khong hop le.");
			continue;
		}

		try
		{
			client = new TcpClient();
			await client.ConnectAsync(host, port);

			NetworkStream stream = client.GetStream();
			reader = new StreamReader(stream, Encoding.UTF8);
			writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

			Console.WriteLine("Ket noi thanh cong toi " + host + ":" + port);
			Console.WriteLine("LocalEndPoint: " + client.Client.LocalEndPoint);
			Console.WriteLine("RemoteEndPoint: " + client.Client.RemoteEndPoint);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Khong ket noi duoc: " + ex.Message);
			reader?.Dispose();
			writer?.Dispose();
			client?.Close();
			reader = null;
			writer = null;
			client = null;
		}
	}
	else if (choice == "2")
	{
		if (client == null || !client.Connected || reader == null || writer == null)
		{
			Console.WriteLine("Ban chua ket noi server.");
			continue;
		}

		Console.WriteLine("Chon dich vu:");
		Console.WriteLine("1 - Time");
		Console.WriteLine("2 - Date");
		Console.WriteLine("3 - Date & Time");
		Console.Write("Nhap lua chon: ");
		string request = (Console.ReadLine() ?? string.Empty).Trim();

		if (request != "1" && request != "2" && request != "3")
		{
			Console.WriteLine("Lua chon khong hop le.");
			continue;
		}

		try
		{
			await writer.WriteLineAsync(request);
			string? response = await reader.ReadLineAsync();

			if (response == null)
			{
				Console.WriteLine("Mat ket noi server.");
				reader.Dispose();
				writer.Dispose();
				client.Close();
				reader = null;
				writer = null;
				client = null;
			}
			else
			{
				Console.WriteLine("Ket qua tu server: " + response);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Loi gui/nhan: " + ex.Message);
			reader?.Dispose();
			writer?.Dispose();
			client?.Close();
			reader = null;
			writer = null;
			client = null;
		}
	}
	else if (choice == "3")
	{
		if (client == null || !client.Connected || reader == null || writer == null)
		{
			Console.WriteLine("Khong co ket noi de ngat.");
			continue;
		}

		try
		{
			await writer.WriteLineAsync("BYE");
			string? response = await reader.ReadLineAsync();
			if (response != null)
			{
				Console.WriteLine("Server: " + response);
			}
		}
		catch
		{
		}
		finally
		{
			reader.Dispose();
			writer.Dispose();
			client.Close();
			reader = null;
			writer = null;
			client = null;
			Console.WriteLine("Da ngat ket noi.");
		}
	}
	else if (choice == "0")
	{
		reader?.Dispose();
		writer?.Dispose();
		client?.Close();
		break;
	}
	else
	{
		Console.WriteLine("Lua chon khong hop le.");
	}
}
