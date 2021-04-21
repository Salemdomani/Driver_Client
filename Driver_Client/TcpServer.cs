using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Driver_Client
{
    class TcpServer
    {
        private TcpListener _server;
        ShouldBeService form;
        public TcpServer(string ipAddress,int port,ShouldBeService form)
        {
            _server = new TcpListener(IPAddress.Any, port);
            this.form = form;
        }

        public void StartListening()
        {
            _server.Start();
            while (true)
            {
                var client = _server.AcceptTcpClient();
                var sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
                var sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
                try
                {
                    var incom = sReader.ReadLine();
                    if (incom == "Start")
                    {
                        sWriter.WriteLine("VMS " + ShouldBeService.VmNum + " Started Successfully");
                        form.Start(); 
                        sWriter.Flush();
                    }

                    else if (incom == "Stop")
                    {
                        sWriter.WriteLine("VMS " + ShouldBeService.VmNum + " Stopped Successfully");
                        form.Stop();
                        sWriter.Flush();
                    }
                }
                catch (Exception)
                {

                    Console.WriteLine("Stopped suddenly");
                }
                
                    
                
            }
            
        }
    }
}
