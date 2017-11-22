using System;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Threading;
using System.Net;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Server
{
    class Program
    {
        public static Hashtable CL = new Hashtable();
        public static int port = 0;
        public string clientData;
        Random rand0;
        
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
                        Random rand0 = new Random();
                        NetworkStream netstr = clientsock.GetStream();
                        netstr.Read(bytesFrom, 0, bytesFrom.Length);
                        clientData = Encoding.ASCII.GetString(bytesFrom);
                        clientData = clientData.Substring(0, clientData.IndexOf("$"));
                        CL.Clear();
                        try
                        {
                            CL.Add(clientData, clientsock);
                        }
                        catch
                        {
                            int temp0 = rand0.Next(0, 1111);
                            string temp1 = Convert.ToString(temp0);
                            clientData = clientData + temp1;
                            CL.Add(clientData, clientsock);
                        }
                       

                        transmit(clientData + " Has Arrived \n", clientData, false);
                        ClientHandler client0 = new ClientHandler();
                        Console.WriteLine(clientData + " Has Arrived ");
                        client0.ClientStart(clientsock, clientData, CL);
                    }

                }
            }
        }
        public string temp1 = "";
        public string temp2 = "";
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
                try
                {
                    transstream.Write(transbytes, 0, transbytes.Length);
                    
                    transstream.Flush();
                    Console.WriteLine(msg);
                }
                catch
                {

                    //CL.Clear();


                }
                
            }
        }
        
    }
    public class ClientHandler
    {
        TcpClient clsock;
        string clientN;
        Hashtable CL;
        Thread clientThread;


        public void ClientStart(TcpClient clsockin, string clientn0, Hashtable List0)
        {
            clsock = clsockin;
            clientN = clientn0;
            CL = List0;
            clientThread = new Thread(chatthings);
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

                            try
                            {
                                Program.transmit(ClientData ,"a client has disconnected", true);
                            }
                            catch
                            {
                                
                            }

                            clientThread.Abort();
                        }
                       
                    }
                    catch
                    {
                        netstream.Close();
                    }
                    //ClientData = ClientData.Substring(0, ClientData.IndexOf("$"));
                    Count1 = Convert.ToString(reqs);
                    try
                    {
                        Program.transmit(ClientData, clientN, true);
                    }
                    catch
                    {
                        break;
                    }

                }
                catch (Exception FF)
                {
                    Console.WriteLine(FF.ToString());

                }
            }
        }
      

    }
    
    
}
