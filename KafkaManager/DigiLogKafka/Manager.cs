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
using System.IO;

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

                client.SendMessageAsync(topicName, new[] { new KafkaNet.Protocol.Message(message + ", " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff")) }).Wait();
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
        public IEnumerable<KafkaNet.Protocol.Message> GetMessage(string kafkaAddress, string topicName)
        {

            var options = new KafkaOptions(new Uri(kafkaAddress));
            var router = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions(topicName, router));

            return consumer.Consume();
        }

        public void ExportCSV(string contents)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            saveFileDialog1.Filter = filter;

            //StreamWriter sw = new StreamWriter("test.csv", false, Encoding.UTF8);
            StreamWriter sw = null;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filter = saveFileDialog1.FileName;
                sw = new StreamWriter(filter,false,Encoding.UTF8);
                string[] stringSeparators = new string[] { "\r\n" };
                string[] lines = contents.Split(stringSeparators, StringSplitOptions.None);
                foreach (string s in lines)
                {
                    sw.WriteLine(s);
                }
                sw.Close();
            }            
        }
    }
}
