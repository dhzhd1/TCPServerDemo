using System;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Remoting.Lifetime;
using System.Threading;
using System.Security.AccessControl;

namespace TCPServer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			try {
				TcpListener tcpServer = null;
				Int32 port = 14928;
				//IPAddress listenIP = IPAddress.Parse ("0.0.0.0");
				tcpServer = new TcpListener (IPAddress.Any, port);
				tcpServer.Start ();
				Console.WriteLine (String.Format ("Service Lisensed on 0.0.0.0:{0}", port.ToString ()));
				
				//Start Licensing Loop
				while (true) {
					Byte[] receive_buffer = new byte[10];
					String receivedStr = String.Empty;
				
					Console.WriteLine ("Waiting for connection...");
					TcpClient client = tcpServer.AcceptTcpClient ();
					Console.WriteLine ("Connecting with " + client.Client.RemoteEndPoint.ToString ());
				
					NetworkStream ns = client.GetStream ();
					int counter;
					counter = ns.Read (receive_buffer, 0, receive_buffer.Length);
					do {
						receivedStr += System.Text.Encoding.ASCII.GetString (receive_buffer, 0, counter);
						counter = ns.Read (receive_buffer, 0, receive_buffer.Length);
					} while (ns.DataAvailable);
					if (counter != 0) {
						receivedStr += System.Text.Encoding.ASCII.GetString (receive_buffer, 0, counter);
					}
					Console.WriteLine ("Received:" + receivedStr); 
				
					String responseStr = receivedStr.ToUpper ();
					Byte[] response_buffer = System.Text.Encoding.ASCII.GetBytes (responseStr);
					ns.Write (response_buffer, 0, response_buffer.Length);
					Console.WriteLine ("Send:" + responseStr);
					//ns.Close();
					client.Close ();
				}
			} catch (SocketException ex) {
				Console.WriteLine (ex.SocketErrorCode.ToString ());
			}
		}
	}
}
