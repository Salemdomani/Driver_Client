namespace Driver_Client
{
    partial class ShouldBeService
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShouldBeService));
            this.StartBtn = new System.Windows.Forms.Button();
            this.SQLworker = new System.ComponentModel.BackgroundWorker();
            this.DRworker = new System.ComponentModel.BackgroundWorker();
            this.Repeater = new System.Windows.Forms.Timer(this.components);
            this.JobLabel = new System.Windows.Forms.Label();
            this.ListenerWorker = new System.ComponentModel.BackgroundWorker();
            this.HelloTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(12, 76);
            this.StartBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(205, 65);
            this.StartBtn.TabIndex = 2;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // SQLworker
            // 
            this.SQLworker.WorkerSupportsCancellation = true;
            this.SQLworker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SQLworker_DoWork);
            this.SQLworker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.SQLworker_RunWorkerCompleted);
            // 
            // DRworker
            // 
            this.DRworker.WorkerSupportsCancellation = true;
            this.DRworker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DRworker_DoWork);
            this.DRworker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DRworker_RunWorkerCompleted);
            // 
            // Repeater
            // 
            this.Repeater.Interval = 30000;
            this.Repeater.Tick += new System.EventHandler(this.Repeater_Tick);
            // 
            // JobLabel
            // 
            this.JobLabel.AutoSize = true;
            this.JobLabel.Location = new System.Drawing.Point(16, 32);
            this.JobLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.JobLabel.Name = "JobLabel";
            this.JobLabel.Size = new System.Drawing.Size(38, 17);
            this.JobLabel.TabIndex = 12;
            this.JobLabel.Text = "Jobs";
            // 
            // ListenerWorker
            // 
            this.ListenerWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ListenerWorker_DoWork);
            // 
            // HelloTimer
            // 
            this.HelloTimer.Interval = 30000;
            this.HelloTimer.Tick += new System.EventHandler(this.HelloTimer_Tick);
            // 
            // ShouldBeService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 152);
            this.Controls.Add(this.JobLabel);
            this.Controls.Add(this.StartBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ShouldBeService";
            this.Text = "ShouldBeService";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShouldBeService_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button StartBtn;
        private System.ComponentModel.BackgroundWorker SQLworker;
        private System.ComponentModel.BackgroundWorker DRworker;
        private System.Windows.Forms.Timer Repeater;
        private System.Windows.Forms.Label JobLabel;
        private System.ComponentModel.BackgroundWorker ListenerWorker;
        private System.Windows.Forms.Timer HelloTimer;
    }
}

