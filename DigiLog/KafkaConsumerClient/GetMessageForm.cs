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
using KafkaNet.Protocol;

namespace KafkaConsumerClient
{
    public partial class GetMessage : Form
    {
        Thread t;
        public GetMessage()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            t = new Thread(() => { GetMessageData(); });
            t.Start();
        }

        private void GetMessageData()
        {
            ////Sample Code Link https://www.codeguru.com/csharp/.net/producer-and-consumer-for-kafka-in-.net-an-exploration.html

            string kafkaAddress = txtKafkaAddress.Text;
            string topicName = txtTopicName.Text;

            //PLAINTEXT://kafkabroker.northeurope.cloudapp.azure.com:9092
            //Testtopic11

            var options = new KafkaOptions(new Uri(kafkaAddress));
            var router = new BrokerRouter(options);

            var consumer = new Consumer(new ConsumerOptions(topicName, router));

            try
            {
                //Consume returns a blocking IEnumerable (ie: never ending stream)
                foreach (var message in consumer.Consume())
                {
                    if (txtMessage.Text.Length > 0)
                    {
                        txtMessage.AppendText(Environment.NewLine);
                    }
                    txtMessage.AppendText("[" + DateTime.Now + "] " + Encoding.UTF8.GetString(message.Value));

                    toolStripStatusLabel1.Text = "The messages were received successfully.";
                }
            }
            catch (ThreadAbortException ex)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Concat(ex.InnerException, ex.Message));
            }

        }

        private void GetMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(t.IsAlive)
                t.Abort();       
        }

        private void GetMessage_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (t.IsAlive)
                t.Abort();
        }
    }
}
