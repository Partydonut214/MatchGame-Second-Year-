using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
using System.Windows.Threading;

namespace MatchGame2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        float bestTime = 9999;
        bool DeathTimerActive = false;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (DeathTimerActive == true)
            {
                tenthsOfSecondsElapsed--;
                timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
                if (matchesFound == 8)
                {
                    timer.Stop();
                    float timertodouble = (tenthsOfSecondsElapsed / 10F);
                    if (timertodouble < bestTime)
                    {
                        bestTime = bestTime - (tenthsOfSecondsElapsed / 10F);
                        BestTimeTextBox.Text = "Best: " + bestTime.ToString("0.0s");
                        MessageBox.Show($"Wow! You Survived and shaved {timertodouble.ToString("0.0s")} off your Best!", "Notice: You did it!", MessageBoxButton.OK);
                    }
                    timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
                }
                if (tenthsOfSecondsElapsed == 0)
                {
                    timer.Stop();
                    MessageBox.Show("The Timer Ran Out!", "Uh Oh", MessageBoxButton.OK, MessageBoxImage.Stop);
                    SetUpGame();
                }
            }
            if (DeathTimerActive == false)
            {
                tenthsOfSecondsElapsed++;
                timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
                if (matchesFound == 8)
                {
                    timer.Stop();
                    if ((tenthsOfSecondsElapsed / 10F) < bestTime)
                    {
                        bestTime = (tenthsOfSecondsElapsed / 10F);
                        BestTimeTextBox.Text = "Best: " + bestTime.ToString("0.0s");
                        MessageBox.Show("Congratulations! You got a new best score!", "Notice: New Best", MessageBoxButton.OK, MessageBoxImage.Information);
                        MessageBoxResult deathtimercheck = MessageBox.Show("Do you want to enable the Death Timer?", "Notice: Death Timer", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (deathtimercheck == MessageBoxResult.Yes)
                        {
                            DeathTimerActive = true;
                            SetUpGame();
                        }
                    }
                    timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
                }
            }
        }

        private void SetUpGame()
        {
            Random random = new Random();

            List<string> o = new List<string>()
            {

                "о", "ȯ", "ọ", "ỏ", "ơ", "ó", "ò", "ö",
                "о", "ȯ", "ọ", "ỏ", "ơ", "ó", "ò", "ö",
            };
            List<string> animalEmoji = new List<string>()
            {
                "🐿", "🐿",
                "🐜", "🐜",
                "🦒", "🦒",
                "🐯", "🐯",
                "🐳", "🐳",
                "🐙", "🐙",
                "🦉", "🦉",
                "🐥", "🐥",
            };
            List<string> EasyList = new List<string>()
            {
                "𒐫", "𒐫",
                "𒐫", "𒐫",
                "𒐫", "𒐫",
                "𒐫", "𒐫",
                "𒐫", "𒐫",
                "𒐫", "𒐫",
                "𒐫", "𒐫",
                "𒐫", "𒐫",
            };
            List<string> THELIST = new List<string>();
            string[] lists = new string[3] {"o", "animalEmoji", "EasyList"};

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock" && textBlock.Name != "BestTimeTextBox")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            timer.Start();
            if (DeathTimerActive == true)
            {
                tenthsOfSecondsElapsed = Convert.ToInt32(bestTime * 10F);
            }
            if (DeathTimerActive == false)
            {
                tenthsOfSecondsElapsed = 0;
            }
            matchesFound = 0;

        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock; if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
