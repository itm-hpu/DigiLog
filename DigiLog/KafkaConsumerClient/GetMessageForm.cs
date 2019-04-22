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

        private  void GetMessageData()
        {
            ////Sample Code Link https://www.codeguru.com/csharp/.net/producer-and-consumer-for-kafka-in-.net-an-exploration.html
            var options = new KafkaOptions(new Uri("PLAINTEXT://kafkabroker.northeurope.cloudapp.azure.com:9092"));
            var router = new BrokerRouter(options);

            OffsetPosition[] offsetPositions = new OffsetPosition[]
              {
                new OffsetPosition()
                   {
                     Offset = 44,
                     PartitionId = 0
                   }
              };
         var consumer = new KafkaNet.Consumer(new ConsumerOptions("Testtopic11", new BrokerRouter(options)), offsetPositions);

            try
            {
                //Consume returns a blocking IEnumerable (ie: never ending stream)
                foreach (var message in consumer.Consume())
                {
                      if (Message.Text.Length > 0)
                    {
                        Message.AppendText(Environment.NewLine);
                    }

                    Message.AppendText(Encoding.UTF8.GetString(message.Value));

                    //Console.WriteLine("Response: P{0},O{1} : {2}",
                    //   message.Meta.PartitionId, message.Meta.Offset,
                    //   Encoding.UTF8.GetString(message.Value));
                }
            }
            catch (ThreadAbortException ex)
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Concat(ex.InnerException,ex.Message));

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
