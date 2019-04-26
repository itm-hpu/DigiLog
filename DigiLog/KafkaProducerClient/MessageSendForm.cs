using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
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

namespace KafkaProducerClient
{
    public partial class MessageSendForm : Form
    {
        public MessageSendForm()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            Thread t = new Thread(() => { sendmessage();});
            t.Start(); 

        }

        private void sendmessage()
        {
            try
            {
                string kafkaAddress = txtKafkaAddress.Text;
                string topicName = txtTopicName.Text;

                var options = new KafkaOptions(new Uri(kafkaAddress));
                var router = new BrokerRouter(options);

                var client = new Producer(router);
                client.SendMessageAsync(topicName, new[] { new KafkaNet.Protocol.Message(send_Messages.Text) }).Wait();
                send_Messages.Clear();

                toolStripStatusLabel1.Text = "The message was successfully sent.";

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Concat(ex.InnerException, ex.Message));
            }
            finally
            {
                Thread.CurrentThread.Abort();
            }
        }


    }
}
