using System;
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

        }
    }
}
