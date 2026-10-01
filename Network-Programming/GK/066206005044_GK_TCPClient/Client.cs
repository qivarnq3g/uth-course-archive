using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

internal static class Program
{
    private static void Main()
    {
        string MSSV = "066206005044";

        string ServerIp = "127.0.0.1";
        int ServerPort = 9999;

        string StudentFullName = "Hoang Viet Quang";

        Socket sckClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        byte[] buffer = new byte[4096];

        try
        {
            sckClient.Connect(new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort));
            Console.WriteLine("Connected to server {0}:{1}", ServerIp, ServerPort);

            int n = sckClient.Receive(buffer);
            string greeting = Encoding.ASCII.GetString(buffer, 0, n);
            Console.WriteLine("[Server greeting] {0}", greeting);

            string lastDigit = MSSV[MSSV.Length - 1].ToString();
            sckClient.Send(Encoding.ASCII.GetBytes(lastDigit));
            Console.WriteLine("[Client] MSSV last digit sent: {0}", lastDigit);

            n = sckClient.Receive(buffer);
            string taskMessage = Encoding.ASCII.GetString(buffer, 0, n);
            Console.WriteLine("[Task] {0}", taskMessage);

            sckClient.Send(Encoding.ASCII.GetBytes(MSSV));
            Console.WriteLine("[Step 1] Sent MSSV: {0}", MSSV);
            n = sckClient.Receive(buffer);
            Console.WriteLine("[Step 1] {0}", Encoding.ASCII.GetString(buffer, 0, n));

            sckClient.Send(Encoding.ASCII.GetBytes(StudentFullName));
            Console.WriteLine("[Step 2] Sent full name: {0}", StudentFullName);
            n = sckClient.Receive(buffer);
            Console.WriteLine("[Step 2] {0}", Encoding.ASCII.GetString(buffer, 0, n));

            sckClient.Send(Encoding.ASCII.GetBytes("CHAO"));
            Console.WriteLine("[Step 3] Sent CHAO");
            n = sckClient.Receive(buffer);
            string textFromServer = Encoding.ASCII.GetString(buffer, 0, n);
            Console.WriteLine("[Step 3] Data from server: {0}", textFromServer);

            string lowercaseText = textFromServer.ToLowerInvariant();
            int wordCount = CountWords(lowercaseText);
            string payload = lowercaseText + ";" + wordCount;
            sckClient.Send(Encoding.ASCII.GetBytes(payload));
            Console.WriteLine("[Step 4] Sent transformed payload: {0}", payload);
            n = sckClient.Receive(buffer);
            Console.WriteLine("[Step 4] {0}", Encoding.ASCII.GetString(buffer, 0, n));

            sckClient.Send(Encoding.ASCII.GetBytes("TSCHUSS"));
            Console.WriteLine("[Step 5] Sent TSCHUSS");
            n = sckClient.Receive(buffer);
            Console.WriteLine("[Final] {0}", Encoding.ASCII.GetString(buffer, 0, n));
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Socket error: {0}", ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: {0}", ex.Message);
        }
        finally
        {
            if (sckClient.Connected)
            {
                sckClient.Shutdown(SocketShutdown.Both);
            }

            sckClient.Close();
        }
    }

    private static int CountWords(string input)
    {
        return input.Split(' ').Length;
    }
}
