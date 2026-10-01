Trong bài kiểm tra thực hành này, sinh viên được yêu cầu viết 1 TCP Client để kết nối đến server. Để hoàn thành bài kiểm tra này, sinh viên cần gửi nhận dữ liệu theo đúng yêu cầu nhận được từ server. Yêu cầu này được gửi ở message đầu tiên sau khi kết nối thành công. Nếu hoàn thành đầy đủ các bước, server sẽ ghi nhận và điểm sẽ được cập nhật trên website.

Lưu ý:

(1) server giới hạn thời gian là 8s để hoàn thành sau mỗi lần kết nối, do đó các thông tin client gửi cho server cần được xử lý trực tiếp trong code và không nhập từ bàn phím.

(2) sinh viên được chạy nhiều lần để thử nghiệm chương trình, nhưng muốn hoàn thành thì tất cả các yêu cầu cần được thực thi trong một lần chạy.

- Thông tin của TCP Server: 

   IP: 127.0.0.1

   Port: 9999

Requested files
Client.cs
//------------------------------------------------------------------------
// LUU Y: MOI SINH VIEN SE DUOC GIAO DE KHAC NHAU THEO THONG TIN TREN LOP
//
using System.Net.Sockets;
using System.Net;
using System.Text;

int ServerPort = 9999;
Server server = new Server(ServerPort);
server.Start();
//------------------------------------------------------------------------
//A. Dien MSSV cua ban o chuoi ben duoi
string MSSV = "000000002";
//------------------------------------------------------------------------
//B. Lay thong tin cua server tu giang vien
// (cho tren lop)
//-----------------------------------------------------------------------
//C. Hoan thien doan chuong trinh sau de nhan de bai tu server
//-------------------------------------
//C1. Tao socket TCP phia Client, ket noi den mot sever co IP theo thong tin GV cung cap
Socket sckClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//sckClient.Connect(...);
//C2. Nhan loi chao tu server dang chuoi 
byte[] buffer = new byte[1024];
int n = sckClient.Receive(buffer);
string s = Encoding.ASCII.GetString(buffer, 0, n);
System.Console.WriteLine(s);
//C3. Gui chu so cuoi cua MSSV
sckClient.Send(Encoding.ASCII.GetBytes(MSSV[MSSV.Length-1].ToString()));
//C4. Nhan de bai dang chuoi
n = sckClient.Receive(buffer);
//C5. Hien thi de bai
System.Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, n));
//---------------------------------------------------------------------------
//D. Lam theo yeu cau de bai
//---------------------------------------------------------------------------
