using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        public static int port = 0;
        static void Main(string[] args)
        {
            Console.Write("~");
            string input = Convert.ToString(Console.ReadLine());
            
            if (input == "start")
            {
                Console.Write("what port would you like the server to be hosted on?: ");                 
                
                try
                {                    
                    port = Convert.ToInt32(Console.ReadLine());

                }
                catch (OverflowException)
                {
                    Console.WriteLine("What in tarnation! that port is not valid! Try another port.");

                }
                catch (FormatException)
                {
                    Console.WriteLine("That isnt a number, mate. Try again");
                }


                Console.Write("Would you like a password?");
                string passStr = Convert.ToString(Console.ReadLine());
                passStr = passStr.ToLower();
                if (passStr == "yes" || passStr == "y")
                {
                    Console.Write("Please enter the password you would like for your chatroom: ");
                    string password = (Console.ReadLine());
                    Console.Write("Confirm? [y/n]");
                    string dec = Convert.ToString(Console.ReadLine());
                    dec = dec.ToLower();
                    if (dec == "y")
                    {
                        Console.Write("Confirmed. Now listening on " + port + " with password");
                    }
                    else
                    {
                       //not confirmed message
                    }
                } 
            }
        }
    }
}
