using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("MSSV: 066206005044 - BT4.2 - UDPChat42 Server");
Console.Write("Nhap port (mac dinh 6000): ");
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

using UdpClient server = new UdpClient(port);

Dictionary<string, IPEndPoint> nicknameToEndPoint = new Dictionary<string, IPEndPoint>(StringComparer.OrdinalIgnoreCase);
Dictionary<string, string> endPointToNickname = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

Console.WriteLine("Server UDPChat42 dang lang nghe cong " + port);
Console.WriteLine("Lenh ho tro: REGISTER:<nick>, SEND:<toNick>:<message>, EXIT");

while (true)
{
	UdpReceiveResult received = await server.ReceiveAsync();
	IPEndPoint senderEndPoint = received.RemoteEndPoint;
	string endPointKey = senderEndPoint.ToString();
	string text = Encoding.UTF8.GetString(received.Buffer).Trim();

	if (text.StartsWith("REGISTER:", StringComparison.OrdinalIgnoreCase))
	{
		string nickname = text.Substring("REGISTER:".Length).Trim();

		if (nickname.Length == 0)
		{
			await SendTextAsync(server, senderEndPoint, "ERROR: Nickname khong duoc rong.");
			continue;
		}

		if (endPointToNickname.TryGetValue(endPointKey, out string? oldNickname))
		{
			if (!oldNickname.Equals(nickname, StringComparison.OrdinalIgnoreCase) &&
				nicknameToEndPoint.TryGetValue(nickname, out IPEndPoint? existingEndPoint) &&
				!existingEndPoint.Equals(senderEndPoint))
			{
				await SendTextAsync(server, senderEndPoint, "ERROR: Nickname da ton tai.");
				continue;
			}

			nicknameToEndPoint.Remove(oldNickname);
			nicknameToEndPoint[nickname] = senderEndPoint;
			endPointToNickname[endPointKey] = nickname;
			await SendTextAsync(server, senderEndPoint, "OK: Da cap nhat nickname " + nickname);
			Console.WriteLine("[REGISTER] " + senderEndPoint + " -> " + nickname);
			continue;
		}

		if (nicknameToEndPoint.ContainsKey(nickname))
		{
			await SendTextAsync(server, senderEndPoint, "ERROR: Nickname da ton tai.");
			continue;
		}

		nicknameToEndPoint[nickname] = senderEndPoint;
		endPointToNickname[endPointKey] = nickname;

		await SendTextAsync(server, senderEndPoint, "OK: Dang ky nickname thanh cong " + nickname);
		Console.WriteLine("[REGISTER] " + senderEndPoint + " -> " + nickname);
	}
	else if (text.StartsWith("SEND:", StringComparison.OrdinalIgnoreCase))
	{
		if (!endPointToNickname.TryGetValue(endPointKey, out string? senderNickname))
		{
			await SendTextAsync(server, senderEndPoint, "ERROR: Ban chua dang ky nickname.");
			continue;
		}

		string payload = text.Substring("SEND:".Length);
		int splitIndex = payload.IndexOf(':');
		if (splitIndex <= 0 || splitIndex == payload.Length - 1)
		{
			await SendTextAsync(server, senderEndPoint, "ERROR: Sai dinh dang. Dung SEND:<toNick>:<message>");
			continue;
		}

		string toNickname = payload.Substring(0, splitIndex).Trim();
		string message = payload.Substring(splitIndex + 1).Trim();

		if (toNickname.Length == 0 || message.Length == 0)
		{
			await SendTextAsync(server, senderEndPoint, "ERROR: Nickname nhan va noi dung khong duoc rong.");
			continue;
		}

		if (!nicknameToEndPoint.TryGetValue(toNickname, out IPEndPoint? targetEndPoint))
		{
			await SendTextAsync(server, senderEndPoint, "ERROR: Nickname nguoi nhan khong ton tai.");
			continue;
		}

		string forward = "MSG:" + senderNickname + ":" + message;
		await SendTextAsync(server, targetEndPoint, forward);
		await SendTextAsync(server, senderEndPoint, "OK: Da gui den " + toNickname);

		Console.WriteLine("[SEND] " + senderNickname + " -> " + toNickname + ": " + message);
	}
	else if (text.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
	{
		if (endPointToNickname.TryGetValue(endPointKey, out string? nickname))
		{
			endPointToNickname.Remove(endPointKey);
			nicknameToEndPoint.Remove(nickname);
			Console.WriteLine("[EXIT] " + nickname + " roi phong chat.");
		}

		await SendTextAsync(server, senderEndPoint, "OK: Da thoat.");
	}
	else
	{
		await SendTextAsync(server, senderEndPoint, "ERROR: Lenh khong hop le.");
	}
}

static async Task SendTextAsync(UdpClient udp, IPEndPoint endPoint, string message)
{
	byte[] buffer = Encoding.UTF8.GetBytes(message);
	await udp.SendAsync(buffer, endPoint);
}
