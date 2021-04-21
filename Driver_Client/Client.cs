using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Driver_Client
{
    class Client
    {
        TcpClient _client;
        string ipAddress;
        int port;
        //StreamReader _sReader;
        StreamWriter _sWriter;
        public Client(string ipAddress,int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void Send(string data)
        {
            
            try
            {
                _client = new TcpClient();
                _client.Connect(ipAddress, port);
                _sWriter = new StreamWriter(_client.GetStream(), Encoding.ASCII);
                //_sReader = new StreamReader(_client.GetStream(), Encoding.ASCII);
                _sWriter.WriteLine(data);
                _sWriter.Flush();
                //return _sReader.ReadLine();
            }
            catch (Exception)
            {

                Console.WriteLine("cannot send data");
            }
            

        }

       
    } 
}
