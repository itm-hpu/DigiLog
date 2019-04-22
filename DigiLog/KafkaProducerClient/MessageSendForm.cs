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
                var options = new KafkaOptions(new Uri("PLAINTEXT://kafkabroker.northeurope.cloudapp.azure.com:9092"));
                var router = new BrokerRouter(options);

                var client = new Producer(router);
                client.SendMessageAsync("Testtopic11", new[] { new KafkaNet.Protocol.Message(send_Messages.Text) }).Wait();
                send_Messages.Clear();
              
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
