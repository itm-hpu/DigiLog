using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using KafkaNet;
using KafkaNet.Model;

using DigiLogKafka;

namespace MainForm
{
    public partial class Main : Form
    {
        Thread tConsumer;

        public Main()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            CheckServerOption();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Manager mMnanager = new Manager();

            Thread t = new Thread(() => { mMnanager.SendMessage(txtPServer.Text,txtPTopic.Text,txtPMessage.Text); });
            t.Start();
        }

        private void CheckServerOption()
        {
            if (chbConsumer.Checked == true)
            {
                txtCServer.Text = txtPServer.Text;
                txtCTopic.Text = txtPTopic.Text;

                txtCServer.Enabled = false;
                txtCTopic.Enabled = false;
            }
            else if (chbConsumer.Checked == false)
            {
                txtCServer.Text = "";
                txtCTopic.Text = "";

                txtCServer.Enabled = true;
                txtCTopic.Enabled = true;
            }
        }

        private void chbConsumer_CheckedChanged(object sender, EventArgs e)
        {
            CheckServerOption();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Manager mMnanager = new Manager();
            
            tConsumer = new Thread(() => 
            {
                IEnumerable<KafkaNet.Protocol.Message> messages = mMnanager.GetMessage(txtCServer.Text, txtCTopic.Text);

                foreach (var message in messages)
                {
                    if (txtCMessage.Text.Length > 0)
                    {
                        txtCMessage.AppendText(Environment.NewLine);
                    }
                    txtCMessage.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff") + ", " + Encoding.UTF8.GetString(message.Value));                    
                }
            });
            tConsumer.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            tConsumer.Abort();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCMessage.Text = "";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Manager mManager = new Manager();
            string messages = txtCMessage.Text;

            mManager.ExportCSV(messages);

        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {

        }
    }
}
