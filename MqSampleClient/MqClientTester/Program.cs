using Mq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqClientTester
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter message text and press return to send.");
                Console.WriteLine("Press return with a blank line to exit");

                string messageToSend = Console.ReadLine();
                using (var client = new MqClient("localhost", "TEST.CHANNEL", "TEST.QUEUE"))
                {
                    while (!String.IsNullOrEmpty(messageToSend))
                    {
                        client.Put(messageToSend);
                        Console.WriteLine($"Message Sent: {messageToSend}");
                        messageToSend = Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
