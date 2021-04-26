using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Driver_Client
{
    class TcpServer
    {
        TcpListener listener;
        ShouldBeService form;
        public bool isRunning = true;

        public TcpServer(string ipAddress,int port,ShouldBeService form)
        {
            listener = new TcpListener(IPAddress.Any, port);
            this.form = form;
        }

        public void StartListening()
        {
            listener.Start();
            while (isRunning)
            {
                var client = listener.AcceptTcpClient();
                var sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
                var sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
                try
                {
                    var income = sReader.ReadLine();
                    if (income == "Start")
                    {
                        form.Start();

                        sWriter.WriteLine("VMS " + ShouldBeService.VmNum + " Started Successfully");
                        sWriter.Flush();
                    }

                    else if (income == "Stop")
                    {   
                        form.Stop();
                        sWriter.WriteLine("VMS " + ShouldBeService.VmNum + " Stopped Successfully");
                        sWriter.Flush();
                    }
                }
                catch (Exception ex){
                    Console.WriteLine("something went wrong :"+ex);
                    Reporter.Report(ex.Message);
                }
                
                    
                
            }
            
        }
    }
}
