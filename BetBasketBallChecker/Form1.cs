using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace BetBasketBallChecker
{
    public partial class Form1 : Form
    {
        private Button okButton;
        private readonly SynchronizationContext synchronizationContext;
        CompareClass checker;
        CompareRuelala checker2;
        CompareAccess checker3;
        CompareDonatetwr checker4;

        public int m_checked, m_live, m_total;
        public Form1()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
            //checker= new CompareClass(this);
            //checker2 = new CompareRuelala(this);
            //checker3 = new CompareAccess(this);
            checker4 = new CompareDonatetwr(this);
            //this.Text = "BetBasketBallChecker "+ checker3.getVersion();
            this.FormClosing += Form1_Closing;
            //startChecker();
        }


        private void startChecker()
        {
            //checker.setUserName(m_userId.Text);
            //checker.setPassword(m_password.Text);
            //checker.Start(m_cartUrl.Text);
            //checker2.Start(m_cartUrl.Text);
            //checker3.Start(m_cartUrl.Text);
            checker4.Start(m_cartUrl.Text,m_inputData.Text);
            m_startBtn.Enabled = false;
            m_totalCount.Text = m_inputData.Text.Split('\n').Length.ToString();

            new Thread(() =>
            {
                updateThread();
            }).Start();
        }

        protected void updateThread()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(5000);
                    if (checker.m_interface.checkUpdate())
                    {
                        //checker.preExit();
                        //checker2.preExit();
                        //checker3.preExit();
                        checker4.preExit();
                        System.Environment.Exit(1);
                    }
                }
                catch (Exception ex)
                {
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            startChecker();
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            checker.Destroy();
            System.Environment.Exit(1);
        }


        public async void writeData(String logText)
        {
            try
            {
                await Task.Run(() =>
                {
                    UpdateData(logText);
                });
            }
            catch (Exception e)
            {

            }
        }

        public void UpdateData(String logText)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                if (m_inputData.Text.Length > 1024 * 10)
                    m_inputData.Text = "";
                m_inputData.Text += o + System.Environment.NewLine;
                m_inputData.SelectionStart = m_inputData.Text.Length;
                m_inputData.ScrollToCaret();
                m_inputData.Refresh();
                m_total += logText.Split('\n').Length;
                m_totalCount.Text = m_total.ToString();
            }), logText);

        }


        public async void writeLog(String logText)
        {
            try
            {
                await Task.Run(() =>
                {
                    UpdateLog(logText);
                });
            }
            catch (Exception e)
            {

            }
        }

        public void UpdateLog(String logText)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                if (m_logBox.Text.Length > 1024 * 10)
                    m_logBox.Text = "";
                m_logBox.Text += o + System.Environment.NewLine;
                m_logBox.SelectionStart = m_logBox.Text.Length;
                m_logBox.ScrollToCaret();
                m_logBox.Refresh();
            }), logText);

        }

        public async void writeResult(String logText)
        {
            try
            {
                await Task.Run(() =>
                {
                    UpdateResult(logText);
                });
            }
            catch (Exception e)
            {

            }
        }

        public async void endChecking()
        {
            try
            {
                await Task.Run(() =>
                {
                    UpdateEndStatus();
                });
            }
            catch (Exception e)
            {

            }
        }

        public void UpdateEndStatus()
        {
            m_startBtn.Enabled = true;
        }

        public void UpdateResult(String logText)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                if (m_checkResult.Text.Length > 1024 * 10)
                    m_checkResult.Text = "";
                m_checkResult.Text += o + System.Environment.NewLine;
                m_checkResult.SelectionStart = m_checkResult.Text.Length;
                m_checkResult.ScrollToCaret();
                m_checkResult.Refresh();

                m_checkedCount.Text = (++m_checked).ToString();
                if(logText.ToLower().Contains("live"))
                    m_liveCount.Text = (++m_live).ToString();
            }), logText);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                m_inputData.Text = sr.ReadToEnd();
                sr.Close();
            }
            m_startBtn.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //checker.Destroy();
            //checker2.Destroy();
            //checker3.Destroy();
            checker4.Destroy();
            System.Environment.Exit(1);
        }
    }
}
