using System;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Threading;
using System.Net;

namespace Server
{
    class Program
    {
        public static Hashtable CL = new Hashtable();
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
                else
                {
                    Console.Write("What is the IP of this machine? ");
                    string ipIn = Convert.ToString(Console.ReadLine());
                    IPAddress ipA = Dns.Resolve(ipIn).AddressList[0];
                    Console.WriteLine("Now listening on port " + port);
                    TcpListener servSock = new TcpListener(ipA, port);
                    TcpClient clientsock = default(TcpClient);
                    int count = 0;
                    servSock.Start();
                    while ((true))
                    {
                        count++;
                        clientsock = servSock.AcceptTcpClient();

                        byte[] bytesFrom = new byte[10025];
                        string clientData = null;

                        NetworkStream netstr = clientsock.GetStream();
                        netstr.Read(bytesFrom, 0, bytesFrom.Length);
                        clientData = Encoding.ASCII.GetString(bytesFrom);
                        clientData = clientData.Substring(0, clientData.IndexOf("$"));
                        
                        CL.Add(clientData, clientsock);

                        transmit(clientData + " Has Arrived \n", clientData, false);
                        ClientHandler client0 = new ClientHandler();
                        Console.WriteLine(clientData + " Joined chat room ");
                        client0.ClientStart(clientsock, clientData, CL);
                    }

                }
            }
        }
       public static void transmit(string msg, string handle, bool flg)
        {
            foreach (DictionaryEntry Item in CL)
            {
                TcpClient bcsock;
                bcsock = (TcpClient)Item.Value;
                NetworkStream transstream = null;
                try
                {
                    transstream = bcsock.GetStream();
                }
                catch
                {
                    bcsock.Close();
                    
                }
                Byte[] transbytes = null;

                if (flg == true)
                {
                    transbytes = Encoding.ASCII.GetBytes(handle + ": " + msg);
                }
                else
                {
                    transbytes = Encoding.ASCII.GetBytes(msg);
                }

                transstream.Write(transbytes, 0, transbytes.Length);
                transstream.Flush();
            }
        }  
    }
    public class ClientHandler
    {
        TcpClient clsock;
        string clientN;
        Hashtable CL;

        public void ClientStart(TcpClient clsockin, string clientn0, Hashtable List0)
        {
            clsock = clsockin;
            clientN = clientn0;
            CL = List0;
            Thread clientThread = new Thread(chatthings);
            clientThread.Start();

        }

        private void chatthings()
        {
            int reqs = 0;
            byte[] bytes = new byte[10025];
            string ClientData = null;
            string response = null;
            string Count1 = null;
            NetworkStream netstream = null;
            string netDC = "";
            
            while ((true))
            {
                try
                {
                    reqs++;
                    try
                    {
                        netstream = clsock.GetStream();
                    }
                    catch
                    {
                        netstream.Close();
                        netstream.Dispose();
                    }
                    try
                    {
                       netstream.Read(bytes, 0, bytes.Length);
                    }
                    catch
                    {
                        netstream.Close();
                        netstream.Dispose();
                    }
                    ClientData = Encoding.ASCII.GetString(bytes);
                    try
                    {
                        ClientData = ClientData.Substring(0, ClientData.IndexOf("$"));
                        if (ClientData == "3DF38FC9")
                        {
                            CL.Remove(1);
                            Program.transmit(ClientData, "has disconnected", true);
                            netstream.Close();
                            netstream.Dispose();
                            
                        }
                       
                    }
                    catch
                    {
                        netstream.Close();
                    }
                    //ClientData = ClientData.Substring(0, ClientData.IndexOf("$"));
                    Count1 = Convert.ToString(reqs);

                    Program.transmit(ClientData, clientN, true);

                }
                catch (Exception FF)
                {
                    Console.WriteLine(FF.ToString());

                }
            }
        }
    }
    
}
