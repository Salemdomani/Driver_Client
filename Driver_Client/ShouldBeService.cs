using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Driver_Client
{
    public partial class ShouldBeService : Form
    {

        public static int VmNum;
        bool isBusy = false;

        List<TODO> todos = new List<TODO>();

        Chrome chrome;
        Client client;
        TcpServer server;
        public ShouldBeService()
        {
            InitializeComponent();
            VmNum = int.Parse(File.ReadAllText("vms.txt"));
            client = new Client("172.16.1.36", 12000);
            server = new TcpServer("172.16.1.36", 11000, this);
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
                ReporterWorker.RunWorkerAsync(ex.Message);
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
                ReporterWorker.RunWorkerAsync(ex.Message);
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
                ReporterWorker.RunWorkerAsync(ex.Message);
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

        private void ReporterWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Reporter.Report(e.Argument as string);
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
                client.Send("Hi from " + VmNum + " and I'm " + isBusy);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ReporterWorker.RunWorkerAsync(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chrome.JoinedGroups();
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
                ReporterWorker.RunWorkerAsync(ex.Message);
            }
            catch (BlockedException ex)
            {
                SQLserver.ReportBlocked(ex.profile);
                ReporterWorker.RunWorkerAsync(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ReporterWorker.RunWorkerAsync(ex.Message);
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
