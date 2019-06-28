using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketTplExtensions.ClientExample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Application started...");

            try
            {
                await StartClient();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fatal exception has been thrown");
                Console.WriteLine(e);
                Console.ResetColor();
            }

            Console.WriteLine("Press <enter> to exit...");
            Console.ReadLine();
        }

        private static async Task StartClient()
        {
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, 11000);

            while (true)
            {
                using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    await socket.ConnectAsync(localEndPoint);

                    Console.WriteLine("Sending request");
                    
                    await socket.SendMessageAsync(
                        $"ClientTime: {DateTimeOffset.Now:O}<EOF>", Encoding.ASCII, SocketFlags.None);

                    var buffer = new byte[1024];
                    var sb = new StringBuilder();

                    while (true)
                    {
                        var receivedBytes = await socket.ReceiveAsync(buffer, 0, buffer.Length, SocketFlags.None);
                        if (receivedBytes == 0)
                            continue;

                        var message = Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                        sb.Append(message);
                        if (message.IndexOf("<EOF>", StringComparison.OrdinalIgnoreCase) > -1)
                            break;
                    }

                    Console.WriteLine(sb.ToString());

                    socket.Shutdown(SocketShutdown.Both);
                }
            }
        }
    }
}
