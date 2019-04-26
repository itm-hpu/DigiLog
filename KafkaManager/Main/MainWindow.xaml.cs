using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

using Manager;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtKafkaPServer.Text = "PLAINTEXT://kafkabroker.northeurope.cloudapp.azure.com:9092";
            txtKafkaPTopic.Text = "Testtopic11";

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
    }
}
