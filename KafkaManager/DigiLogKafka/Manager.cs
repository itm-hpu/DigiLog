using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using System.Windows.Forms;


namespace DigiLogKafka
{
    public class Manager
    {
        public Manager()
        {
        }
        public void SendMessage(string kafkaAddress, string topicName, string message)
        {
            try
            {
                var options = new KafkaOptions(new Uri(kafkaAddress));
                var router = new BrokerRouter(options);
                var client = new Producer(router);

                client.SendMessageAsync(topicName, new[] { new KafkaNet.Protocol.Message(message + " " + DateTime.Now) }).Wait();
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
