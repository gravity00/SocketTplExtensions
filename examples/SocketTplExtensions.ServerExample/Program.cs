using System;
using System.Threading.Tasks;

namespace SocketTplExtensions.ServerExample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Application started...");

            try
            {

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
            }

            Console.WriteLine("Press <enter> to exit...");
            Console.ReadLine();
        }
    }
}
