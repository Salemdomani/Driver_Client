using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driver_Client
{
    public partial class ShouldBeService : Form
    {

        public static int VmNum;
        bool isBusy = false;
        string host = SQLserver.getHost();

        List<TODO> todos = new List<TODO>();
  
        Chrome chrome;
        Client client;
        TcpServer server;

        public ShouldBeService()
        {
            InitializeComponent();
            VmNum = int.Parse(new string(Environment.UserName.Where(char.IsDigit).ToArray()));
            client = new Client(host, 12000);
            server = new TcpServer(host, 11000, this);
            chrome = new Chrome();
        }

        #region Workers
        private void SQLworker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                todos = SQLserver.FindJobs();
                if (todos.Count > 0 && SQLserver.GetProfiles() != null)
                    isBusy = true;
                Console.WriteLine("SQL Connected");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error");
                Reporter.Report(ex.Message);
            }

        }

        private void SQLworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                CheckInternetConnection();
                Console.WriteLine("Internet Connected");
                if (!DRworker.IsBusy && todos.Count > 0)
                    DRworker.RunWorkerAsync();
            }
            catch (NoInternetException ex)
            {
                Console.WriteLine("Internet Error");
                Reporter.Report(ex.Message);
            }

        }

        private void DRworker_DoWork(object sender, DoWorkEventArgs e)
        {
            var count = 0;
            JOB job = todos.First().JOB;
            try
            {
                while (todos.Count > 0)
                {
                    foreach (var p in chrome.profiles)
                    {
                        if (DRworker.CancellationPending)
                            throw new ForceClosedException("Force closed");
                        chrome.StartProfile(p);
                        if (DoJob())
                            count++;
                        Thread.Sleep(new Random().Next(5000, 9000));
                    }
                    JobIsDone();
                }
            }
            catch (ForceClosedException)
            {
                JobIsDone();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Reporter.Report(ex.Message);
                if (chrome.driverIsActive())
                    chrome.Close();
            }
            finally
            {
                client.Send("VMS " + VmNum + " done " + count + " " + job.action.TrimEnd() + "s out of " + chrome.profiles.Count);
            }

        }

        private void DRworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isBusy = false;
        }

        private void ListenerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            server.StartListening();
        }
        #endregion

        #region Form Events
        private void Form1_Load(object sender, EventArgs e)
        {
            SQLserver.ReportVmIs("READY");
            ListenerWorker.RunWorkerAsync();
            HelloTimer.Start();
        }

        private void ShouldBeService_FormClosing(object sender, FormClosingEventArgs e)
        {
            SQLserver.ReportVmIs("DOWN");
            server.isRunning = false;
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            Repeater.Enabled = StartBtn.Text == "Start" ? true : false;
            if (StartBtn.Text == "Stop")
                DRworker.CancelAsync();
            StartBtn.Text = StartBtn.Text == "Start" ? "Stop" : "Start";
        }

        private void Repeater_Tick(object sender, EventArgs e)
        {
            if (DRworker.IsBusy) return;

            if (!SQLworker.IsBusy && todos.Count == 0)
                SQLworker.RunWorkerAsync();
            else if (todos.Count > 0)
                DRworker.RunWorkerAsync();


        }

        private void HelloTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Task.Factory.StartNew(() => client.Send("Hi from " + VmNum + " and I'm " + isBusy));
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
        #endregion

        #region Job

        private bool DoJob()
        {
            var success = false;
            var job = todos.First().JOB;
            var id = job.post_id;
            var action = job.action;
            var text = job.text;
            try
            {
                switch (action.TrimEnd())
                {
                    case "like":
                        chrome.Like(id);
                        break;
                    case "unlike":
                        chrome.Unlike(id);
                        break;
                    case "comment":
                        chrome.Comment(id, text);
                        break;
                    case "share":
                        chrome.Share(id, text);
                        break;
                    case "ShrToGroub":
                        chrome.ShareToGroub(id, text);
                        break;
                }
                success = true;
            }
            catch (AlreadyLikedException ex)
            {
                Console.WriteLine(ex.Message);
                Reporter.Report(ex.Message);
            }
            catch (BlockedException ex)
            {
                SQLserver.ReportBlocked(ex.profile);
                Reporter.Report(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Reporter.Report(ex.Message);
            }
            finally
            {
                chrome.Close();
            }
            return success;

        }

        void JobIsDone()
        {
            SQLserver.ThisJobIsDone(todos.First());
            Reporter.Report("done a job with id :"+todos.First().JOB.Id);
            todos.RemoveAt(0);
        }
        #endregion

        #region SQL Internt Connection
        public void CheckInternetConnection()
        {
            try { using (var webClient = new WebClient()) webClient.OpenRead("http://google.com/generate_204"); }
            catch { throw new NoInternetException("no internet"); }
        }

        public void CheckSQlConnection()
        {

        }
        #endregion

        #region Start Stop
        public void Start()
        {
            this.Invoke((MethodInvoker)delegate
            {
                Repeater.Start();
                StartBtn.Text = "Stop";
            });
        }
        public void Stop()
        {
            this.Invoke((MethodInvoker)delegate
            {
                Repeater.Stop();
                StartBtn.Text = "Start";
                DRworker.CancelAsync();
            });
        }

        #endregion


    }

}
