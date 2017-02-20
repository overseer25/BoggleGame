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
using System.Net.Sockets;

namespace BoggleClient {
    /// <summary>
    /// Interaction logic for Connect.xaml
    /// </summary>
    public partial class Connect : Page {
       private Client clientWork;

        public Connect()
        {
            InitializeComponent();
            CancelButton.Visibility = Visibility.Hidden;
        }

        private void StartGame(String boggleBoard, String opponentName) {

        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            CancelButton.Visibility = Visibility.Visible;
            PlayButton.IsEnabled = false;
            PlayButton.Content = "Connecting";
            try
            {
                clientWork = new Client(NameTextbox.Text, IPTextbox.Text);
                clientWork.Connect();
            }
            catch (SocketException)
            {
                ConnectionFailed();
            }
        }


        private void NameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void IPTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelButton.Visibility = Visibility.Hidden;

            PlayButton.IsEnabled = true;
            PlayButton.Content = "Play";
        }

        public void ConnectionFailed()
        {
            MessageBox.Show("Connection failed. Check IPAddress and try again. Ch33ky skrub.");
            CancelButton_Click(null, null);
        }
    }
}
