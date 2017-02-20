using System;
using CustomNetworking;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BoggleServer;
using System.Windows.Threading;

namespace BoggleClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TextBox> grid;
        private List<TextBox> gridGreen;
        private Client clientWork;
        String[] quotes = { "\"Like Skyrim with guns.\"", "\"It's like the board game, but on the computer.\"", "\"It was ok.\"", 
                              "\"If you were a potato, you'd be a good potato.\"", "\"How can there be mirrors if our eyes aren't real?\"", "\"Boggle\"",
                          "\"Now in HD!\"", "\"The second mouse gets the cheese.\"", "\"I used to be indecisive. Now I'm not sure.\""};
        String[] quoters = { "10/10 -IGN", "-People", "11/10 -Gamespot", "-My love life", "-Crazy people", "-Boggle", "-Boggle", "-Smart people", "-Indecisive people"};
        Random rand;

        public MainWindow()
        {
            rand = new Random();

            clientWork = new Client();
            clientWork.Terminate += Terminated;
            clientWork.StartGame += StartGame;
            clientWork.UpdateTime += UpdateTime;
            clientWork.UpdateScores += UpdateScore;
            clientWork.UpdateWordList += UpdateListOfWords;
            clientWork.EndGame += EndGame;

            InitializeComponent();
            grid = new List<TextBox>() { Board1, Board2, Board3, Board4, Board5, Board6
            , Board7, Board8, Board9, Board10, Board11, Board12, Board13, Board14, Board15
            , Board16};
            gridGreen = new List<TextBox>() { Board17, Board18, Board19, Board20, Board21, Board22
            , Board23, Board24, Board25, Board26, Board27, Board28, Board29, Board30, Board31
            , Board32};
            CancelButton.Visibility = Visibility.Hidden;

            int random = rand.Next(0, 9);
            Dispatcher.Invoke(new Action(() => QuoteTextbox.Content = quotes[random]));
            Dispatcher.Invoke(new Action(() => QuotersTextbox.Content = quoters[random]));

            //Set focus to name text box.
            NameTextbox.Focus();

            //Hide tab controls
            Style s = new Style();
            s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            tabControl.ItemContainerStyle = s;
            System.Diagnostics.Debug.WriteLine("Sending " + WordTextbox.Text);
        }

        /// <summary>
        /// Event handler for dealing with the event when the game is starting.
        /// </summary>
        /// <param name="boggleBoard"></param>
        /// <param name="opponentName"></param>
        private void StartGame(String boggleBoard, String opponentName)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                tabControl.SelectedIndex = 1;

                int i = 0;
                //TODO: Begin the game once the START string has been received.
                foreach(TextBox textbox in grid) {
                    textbox.Text = boggleBoard.ElementAt(i).ToString();
                    i++;
                }
                OpponenetNameTextbox.Content += opponentName;
            }));
        }

        /// <summary>
        /// Event handler for dealing with the even when the time needs to be updated.
        /// </summary>
        /// <param name="time"></param>
        private void UpdateTime(string time)
        {
            //TODO: Create textbox to hold time, and update it here.
            Dispatcher.BeginInvoke(new Action(() => TimeTextBox.Text = time));
        }

        /// <summary>
        /// Event handler for dealing with the event when the scores need to be updated.
        /// </summary>
        /// <param name="score1"></param>
        /// <param name="score2"></param>
        private void UpdateScore(String score1, String score2)
        {
            //TODO: Create textboxes to hold scores, and update them here.
            Dispatcher.BeginInvoke(new Action(() => YourScore.Text = score1.ToString()));
            Dispatcher.BeginInvoke(new Action(() => OpponentScore.Text = score2.ToString()));
        }

        /// <summary>
        /// Event Handler for updating the word list.
        /// </summary>
        private void UpdateListOfWords(String word)
        {
            Dispatcher.BeginInvoke(new Action(() => WordListTextbox.Items.Add(word)));
            Dispatcher.BeginInvoke(new Action(() => WordListTextbox.ScrollIntoView(WordListTextbox.Items.Count-1)));
        }

        /// <summary>
        /// Event handler for dealing with the event when the client loses connection to the server.
        /// </summary>
        /// <param name="msg"></param>
        private void LostConnection(String msg)
        {
            MessageBox.Show(msg);
        }

        /// <summary>
        /// Event handler for dealing with the event when the game is concluding.
        /// </summary>
        /// <param name="myLegalWords"></param>
        /// <param name="oppLegalWords"></param>
        /// <param name="commonWords"></param>
        /// <param name="myIllegalWords"></param>
        /// <param name="oppIllegalWords"></param>
        private void EndGame(String[] myLegalWords, String[] oppLegalWords, String[] commonWords, String[] myIllegalWords, String[] oppIllegalWords)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                foreach(String word in myLegalWords) {
                    myUniqueWordsListBox.Items.Add(word);
                }
                if(myLegalWords.Length == 0)
                    myUniqueWordsListBox.Items.Add("No unique words played.");

                foreach(String word in oppLegalWords) {
                    oppUniqueWordsListBox.Items.Add(word);
                }
                if(oppLegalWords.Length == 0)
                    oppUniqueWordsListBox.Items.Add("No unique words played.");

                foreach(String word in commonWords) {
                    commonWordsListBox.Items.Add(word);
                }
                if(commonWords.Length == 0)
                    commonWordsListBox.Items.Add("No same words played.");

                foreach(String word in myIllegalWords) {
                    myIllegalWordsListBox.Items.Add(word);
                }
                if(myIllegalWords.Length == 0)
                    myIllegalWordsListBox.Items.Add("No illegal words played.");

                foreach(String word in oppIllegalWords) {
                    oppIllegalWordsListBox.Items.Add(word);
                }
                if(oppIllegalWords.Length == 0)
                    oppIllegalWordsListBox.Items.Add("No illegal words played.");

                tabControl.SelectedIndex = 2;

            }));
        }

        /// <summary>
        /// Event handler for dealing with the event when the opposing client disconnects from the server.
        /// </summary>
        /// <param name="msg"></param>
        private void Terminated(String msg)
        {

            int random = rand.Next(0, 9);
            Dispatcher.Invoke(new Action(() => QuoteTextbox.Content = quotes[random]));
            Dispatcher.Invoke(new Action(() => QuotersTextbox.Content = quoters[random]));

            MessageBox.Show(msg);
            Dispatcher.Invoke(new Action(() => tabControl.SelectedIndex = 0));

            Dispatcher.Invoke(new Action(() => CancelButton.Visibility = Visibility.Hidden));
            Dispatcher.Invoke(new Action(() => PlayButton.Content = "Play"));
            Dispatcher.Invoke(new Action(() => PlayButton.IsEnabled = true));
            Dispatcher.Invoke(new Action(() => NameTextbox.Text = ""));
            Dispatcher.Invoke(new Action(() => PlayButton.IsEnabled = true));
            Dispatcher.Invoke(new Action(() => NameTextbox.IsEnabled = true));
            Dispatcher.Invoke(new Action(() => IPTextbox.IsEnabled = true));
            Dispatcher.Invoke(new Action(() => OpponenetNameTextbox.Content = OpponenetNameTextbox.Content.ToString().Substring(0, 9)));


        }


        /// <summary>
        /// Executes when the player clicks the Play button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_Click(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new Action(() => NameTextbox.IsEnabled = false));
            Dispatcher.Invoke(new Action(() => IPTextbox.IsEnabled = false));
            Dispatcher.Invoke(new Action(() => CancelButton.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => PlayButton.IsEnabled = false));
            Dispatcher.Invoke(new Action(() => PlayButton.Content = "Connected"));
            clientWork.Connect(NameTextbox.Text, IPTextbox.Text);
        }

        /// <summary>
        /// Event handler for hitting the enter key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && WordTextbox.IsFocused)
            {
                ResetBoardGreen();
            }

            if (e.Key == Key.Enter && NameTextbox.IsFocused)
            {
                IPTextbox.Focus();
            }
            else if (e.Key == Key.Enter && IPTextbox.IsFocused)
            {
                PlayButton_Click(sender, e);
            }
            else if(e.Key == Key.Enter && WordTextbox.IsFocused)
            {
                if (WordTextbox.Text != "")
                {
                    System.Diagnostics.Debug.WriteLine("Sending " + WordTextbox.Text);
                    clientWork.SendWord(WordTextbox.Text);
                }
                ResetBoardGreen();

            }
        }

        /// <summary>
        /// Event for clicking the cancel button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelButton.Visibility = Visibility.Hidden;

            Dispatcher.Invoke(new Action(() => NameTextbox.IsEnabled = true));
            Dispatcher.Invoke(new Action(() => IPTextbox.IsEnabled = true));

            clientWork.CleanUpConnection();

            Dispatcher.Invoke(new Action(() => PlayButton.IsEnabled = true));
            Dispatcher.Invoke(new Action(() => PlayButton.Content = "Play"));
            Dispatcher.Invoke(new Action(() => YourScore.Text = "0"));
            Dispatcher.Invoke(new Action(() => OpponentScore.Text = "0"));
        }

        /// <summary>
        /// Called when the connection to the server has failed.
        /// </summary>
        public void ConnectionFailed()
        {
            MessageBox.Show("Connection failed. Check IPAddress and try again. Ch33ky skrub.");
            CancelButton_Click(null, null);
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SystemSounds.Exclamation.Play();
            MessageBoxResult saving = MessageBox.Show(this, "Are you sure you want to quit?", "Warning", MessageBoxButton.YesNo);
            if (saving.Equals(MessageBoxResult.No))
            {
                return;
            }
            else
            {
                Dispatcher.Invoke(new Action(() => tabControl.SelectedIndex = 0));

                Dispatcher.Invoke(new Action(() => CancelButton.Visibility = Visibility.Hidden));
                Dispatcher.Invoke(new Action(() => PlayButton.Content = "Play"));
                Dispatcher.Invoke(new Action(() => PlayButton.IsEnabled = true));
                Dispatcher.Invoke(new Action(() => NameTextbox.Text = ""));
                Dispatcher.Invoke(new Action(() => PlayButton.IsEnabled = true));
                Dispatcher.Invoke(new Action(() => NameTextbox.IsEnabled = true));
                Dispatcher.Invoke(new Action(() => IPTextbox.IsEnabled = true));
                Dispatcher.Invoke(new Action(() => OpponenetNameTextbox.Content = OpponenetNameTextbox.Content.ToString().Substring(0, 9)));
                clientWork.CleanUpConnection();


                Dispatcher.Invoke(new Action(() => myIllegalWordsListBox.Items.Clear()));
                Dispatcher.Invoke(new Action(() => myUniqueWordsListBox.Items.Clear()));
                Dispatcher.Invoke(new Action(() => oppUniqueWordsListBox.Items.Clear()));
                Dispatcher.Invoke(new Action(() => oppIllegalWordsListBox.Items.Clear()));
                Dispatcher.Invoke(new Action(() => commonWordsListBox.Items.Clear()));
                Dispatcher.Invoke(new Action(() => WordListTextbox.Items.Clear()));


                int random = rand.Next(0, 9);
                Dispatcher.Invoke(new Action(() => QuoteTextbox.Content = quotes[random]));
                Dispatcher.Invoke(new Action(() => QuotersTextbox.Content = quoters[random]));
            }
            
        }

        private void ResetBoardGreen()
        {
            foreach (TextBox box in gridGreen)
            {
                Dispatcher.BeginInvoke(new Action(() => box.Visibility = Visibility.Hidden));
            }
            Dispatcher.BeginInvoke(new Action(() => WordTextbox.Text = ""));
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => tabControl.SelectedIndex = 0));

            Dispatcher.Invoke(new Action(() => CancelButton.Visibility = Visibility.Hidden));
            Dispatcher.Invoke(new Action(() => PlayButton.Content = "Play"));
            Dispatcher.Invoke(new Action(() => PlayButton.IsEnabled = true));
            Dispatcher.Invoke(new Action(() => NameTextbox.Text = ""));
            Dispatcher.Invoke(new Action(() => PlayButton.IsEnabled = true));
            Dispatcher.Invoke(new Action(() => NameTextbox.IsEnabled = true));
            Dispatcher.Invoke(new Action(() => IPTextbox.IsEnabled = true));
            Dispatcher.Invoke(new Action(() => OpponenetNameTextbox.Content = OpponenetNameTextbox.Content.ToString().Substring(0, 9)));
            clientWork.CleanUpConnection();

            Dispatcher.Invoke(new Action(() => myIllegalWordsListBox.Items.Clear()));
            Dispatcher.Invoke(new Action(() => myUniqueWordsListBox.Items.Clear()));
            Dispatcher.Invoke(new Action(() => oppUniqueWordsListBox.Items.Clear()));
            Dispatcher.Invoke(new Action(() => oppIllegalWordsListBox.Items.Clear()));
            Dispatcher.Invoke(new Action(() => commonWordsListBox.Items.Clear()));
            Dispatcher.Invoke(new Action(() => WordListTextbox.Items.Clear()));

            int random = rand.Next(0, 6);
            Dispatcher.Invoke(new Action(() => QuoteTextbox.Content = quotes[random]));
            Dispatcher.Invoke(new Action(() => QuotersTextbox.Content = quoters[random]));
            
        }

        private void Board1_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board17.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board1.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }

        private void Board2_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board18.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board2.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board3_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board19.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board3.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board4_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board20.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board4.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board5_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board21.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board5.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board6_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board22.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board6.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board7_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board23.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board7.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board8_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board24.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board8.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board9_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board25.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board9.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board10_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board26.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board10.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board11_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board27.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board11.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board12_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board28.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board12.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board13_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board29.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board13.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board14_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board30.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board14.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board15_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board31.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board15.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }
        private void Board16_GotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => Board32.Visibility = Visibility.Visible));
            Dispatcher.Invoke(new Action(() => WordTextbox.Text += Board16.Text));
            Dispatcher.Invoke(new Action(() => WordTextbox.Focus()));
            Dispatcher.Invoke(new Action(() => WordTextbox.Select(WordTextbox.Text.Length, 0)));
        }






    }
}
