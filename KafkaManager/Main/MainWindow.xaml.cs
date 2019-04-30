using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;

//using DigiLogKafka;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //DigiLogKafka.Manager mManager;

        public MainWindow()
        {
            InitializeComponent();            

            txtKafkaPServer.Text = "http://localhost:9092";
            txtKafkaPTopic.Text = "test";            
            
            if (chbKafkaSetting.IsChecked == true)
            {
                txtKafkaCServer.Text = txtKafkaPServer.Text;
                txtKafkaCTopic.Text = txtKafkaPTopic.Text;

                txtKafkaCServer.IsEnabled = false;
                txtKafkaCTopic.IsEnabled = false;

            }
        }

        private void ChbKafkaSetting_Click(object sender, RoutedEventArgs e)
        {
            if (chbKafkaSetting.IsChecked == true)
            {
                txtKafkaCServer.Text = txtKafkaPServer.Text;
                txtKafkaCTopic.Text = txtKafkaPTopic.Text;

                txtKafkaCServer.IsEnabled = false;
                txtKafkaCTopic.IsEnabled = false;
            }
            else if (chbKafkaSetting.IsChecked == false)
            {
                txtKafkaCServer.IsEnabled = true;
                txtKafkaCTopic.IsEnabled = true;
                txtKafkaCServer.Text = "";
                txtKafkaCTopic.Text = "";

            }
        }

        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            //DigiLogKafka.Manager mManager = new Manager();

            //this.Dispatcher.Invoke(() => { SendMessage(txtKafkaPServer.Text, txtKafkaPTopic.Text, txtMessage.Text); });

            //Thread t = new Thread(() => { SendMessage(txtKafkaPServer.Text, txtKafkaPTopic.Text, txtMessage.Text); });
            //t.Start();

            SendMessage(txtKafkaPServer.Text, txtKafkaPTopic.Text, txtMessage.Text);            

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

        public void GetMessages(string kafkaAddress, string topicName)
        {
            var options = new KafkaOptions(new Uri(kafkaAddress));
            var router = new BrokerRouter(options);

            var consumer = new Consumer(new ConsumerOptions(topicName, router));

            try
            {                
                foreach (var message in consumer.Consume())
                {
                    if (txtMessage.Text.Length > 0)
                    {
                        txtMessage.AppendText(Environment.NewLine);
                    }
                    txtMessage.AppendText("[" + DateTime.Now + "] " + Encoding.UTF8.GetString(message.Value));                    
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

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            GetMessages(txtKafkaCServer.Text, txtKafkaCTopic.Text);
        }
    }
}
