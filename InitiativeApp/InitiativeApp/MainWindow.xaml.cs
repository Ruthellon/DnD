using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitiativeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Character> characterList = new List<Character>() {
            //new Character("Azariah", "https://www.dndbeyond.com/character/25918583/json", Color.FromRgb(169,210,113)),
            //new Character("Whirlwind", "https://www.dndbeyond.com/character/25392280/json", Color.FromRgb(64,199,235)),
            //new Character("Sala", "https://www.dndbeyond.com/character/25719833/json", Color.FromRgb(255,245,105)),
            //new Character("FigHope", "Fig Hope", "https://www.dndbeyond.com/character/25881437/json", Color.FromRgb(255,125,10)),
            //new Character("Athorn", "https://www.dndbeyond.com/character/36209461/json", Color.FromRgb(135,135,237)),
            //new Character("Uquzax", "https://www.dndbeyond.com/character/38938237/json", Color.FromRgb(0,112,221)),
            new Character("Whirlwind", "https://www.dndbeyond.com/character/45150649/json", Color.FromRgb(64,199,235)),
            new Character("Amery", "https://www.dndbeyond.com/character/25889912/json", Color.FromRgb(0,112,221)),
            new Character("Ayla", "https://www.dndbeyond.com/character/45151496/json", Color.FromRgb(255,245,105)),
            new Character("Chompskee", "https://www.dndbeyond.com/character/45152992/json", Color.FromRgb(169,210,113)),
            new Character("Willin", "https://www.dndbeyond.com/character/45964961/json", Color.FromRgb(255,255,255)),
            //new Tuple<string, string>("Lorcan", "https://www.dndbeyond.com/character/25881241/json"),
            //new Tuple<string, string>("Shazdin", "https://www.dndbeyond.com/character/26202482/json"),
            //new Character("Ayla", "https://www.dndbeyond.com/character/26221573/json", Color.FromRgb(255,125,10)),
            //new Tuple<string, string>("Wiley", "https://www.dndbeyond.com/character/29677385/json")
        };

        private List<Character> Characters = new List<Character>();
        private static System.Timers.Timer localTimer;
        private static System.Timers.Timer countdownTimer;
        //private Label TimerLabel = new Label();
        
        public MainWindow()
        {
            InitializeComponent();
            
            charinfogrid.RowDefinitions.Add(new RowDefinition());
            charinfogrid.RowDefinitions.Add(new RowDefinition());
            charinfogrid.RowDefinitions.Add(new RowDefinition());
            charinfogrid.RowDefinitions.Add(new RowDefinition());
            int count = 0;
            foreach (var character in characterList)
            {
                charinfogrid.ColumnDefinitions.Add(new ColumnDefinition());
                character.CharacterDisplay = new CharacterDisplay();
                character.CharacterDisplay.LowHealthWarning = new Rectangle();
                character.CharacterDisplay.LowHealthWarning.Name = "LowHealthRect";
                character.CharacterDisplay.LowHealthWarning.Width = 190;
                character.CharacterDisplay.LowHealthWarning.Height = 190;
                character.CharacterDisplay.LowHealthWarning.Fill = new SolidColorBrush() { Color = Colors.Red, Opacity = 1.0f };
                character.CharacterDisplay.LowHealthWarning.Opacity = 0.0f;

                DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = 0.5;
                myDoubleAnimation.To = 0.0;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                myDoubleAnimation.AutoReverse = true;
                myDoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                
                character.CharacterDisplay.LowHealthAnimation = new Storyboard();
                character.CharacterDisplay.LowHealthAnimation.Children.Add(myDoubleAnimation);
                Storyboard.SetTarget(myDoubleAnimation, character.CharacterDisplay.LowHealthWarning);
                Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Rectangle.OpacityProperty));

                character.CharacterDisplay.Background = new Rectangle();
                character.CharacterDisplay.Background.Width = 200;
                character.CharacterDisplay.Background.Height = 200;
                character.CharacterDisplay.Background.Fill = new SolidColorBrush() { Color = character.BackgroundColor, Opacity = 1f };

                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(@"C:\\DnDStuff\\" + character.Name + "Small.png");
                img.EndInit();

                character.CharacterDisplay.CharImage = new Image();
                character.CharacterDisplay.CharImage.Source = img;
                character.CharacterDisplay.CharImage.Width = 200;
                character.CharacterDisplay.CharImage.Height = 200;

                BitmapImage imgd20 = new BitmapImage();
                imgd20.BeginInit();
                imgd20.UriSource = new Uri(@"C:\\DnDStuff\\d20small.png");
                imgd20.EndInit();

                character.CharacterDisplay.Inspiration = new Image();
                character.CharacterDisplay.Inspiration.Source = imgd20;
                character.CharacterDisplay.Inspiration.Width = 50;
                character.CharacterDisplay.Inspiration.Height = 50;

                BitmapImage shield = new BitmapImage();
                shield.BeginInit();
                shield.UriSource = new Uri(@"C:\\DnDStuff\\shieldsmall.png");
                shield.EndInit();

                character.CharacterDisplay.Shield = new Image();
                character.CharacterDisplay.Shield.Source = shield;
                character.CharacterDisplay.Shield.Width = 50;
                character.CharacterDisplay.Shield.Height = 50;

                BitmapImage nameplate = new BitmapImage();
                nameplate.BeginInit();
                nameplate.UriSource = new Uri(@"C:\\DnDStuff\\nameplatesmall.png");
                nameplate.EndInit();

                character.CharacterDisplay.Nameplate = new Image();
                character.CharacterDisplay.Nameplate.Source = nameplate;
                character.CharacterDisplay.Nameplate.Width = 150;
                character.CharacterDisplay.Nameplate.Height = 75;

                character.CharacterDisplay.TempHPLabel = new Label();
                character.CharacterDisplay.TempHPLabel.FontSize = 18;
                character.CharacterDisplay.TempHPLabel.Content = character.TempHP;
                character.CharacterDisplay.TempHPLabel.Width = 50;
                character.CharacterDisplay.TempHPLabel.HorizontalAlignment = HorizontalAlignment.Center;
                character.CharacterDisplay.TempHPLabel.HorizontalContentAlignment = HorizontalAlignment.Center;

                character.CharacterDisplay.NameLabel = new Label();
                character.CharacterDisplay.NameLabel.FontSize = 22;
                character.CharacterDisplay.NameLabel.Content = character.FullName;
                character.CharacterDisplay.NameLabel.Width = 150;
                character.CharacterDisplay.NameLabel.HorizontalAlignment = HorizontalAlignment.Center;
                character.CharacterDisplay.NameLabel.HorizontalContentAlignment = HorizontalAlignment.Center;

                //Rectangle absBg = new Rectangle();
                //absBg.Width = 200;
                //absBg.Height = 200;
                //absBg.Fill = new SolidColorBrush() { Color = Colors.Black, Opacity = 1.0f };

                Canvas canvas = new Canvas();
                canvas.Width = 200;
                canvas.Height = 200;

                //canvas.Children.Add(absBg);

                canvas.Children.Add(character.CharacterDisplay.Background);

                canvas.Children.Add(character.CharacterDisplay.CharImage);
                
                Canvas.SetLeft(character.CharacterDisplay.LowHealthWarning, 5);
                Canvas.SetTop(character.CharacterDisplay.LowHealthWarning, 5);
                canvas.Children.Add(character.CharacterDisplay.LowHealthWarning);

                Canvas.SetTop(character.CharacterDisplay.Nameplate, 150);
                Canvas.SetLeft(character.CharacterDisplay.Nameplate, 25);
                canvas.Children.Add(character.CharacterDisplay.Nameplate);

                Canvas.SetTop(character.CharacterDisplay.NameLabel, 165);
                Canvas.SetLeft(character.CharacterDisplay.NameLabel, 25);
                canvas.Children.Add(character.CharacterDisplay.NameLabel);

                Canvas.SetTop(character.CharacterDisplay.Inspiration, 150);
                canvas.Children.Add(character.CharacterDisplay.Inspiration);

                Canvas.SetLeft(character.CharacterDisplay.Shield, 150);
                canvas.Children.Add(character.CharacterDisplay.Shield);

                Canvas.SetLeft(character.CharacterDisplay.TempHPLabel, 150);
                Canvas.SetTop(character.CharacterDisplay.TempHPLabel, 5);
                canvas.Children.Add(character.CharacterDisplay.TempHPLabel);
                
                Border border = new Border();
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(5.0);
                border.Height = 210;
                border.Width = 210;
                border.Child = canvas;

                Grid.SetRow(border, 3);
                Grid.SetColumn(border, count);

                charinfogrid.Children.Add(border);
                count++;
            }

            charinfogrid.ColumnDefinitions.Add(new ColumnDefinition());
            charinfogrid.ColumnDefinitions.Add(new ColumnDefinition());
            
            //lstCharacters.ItemsSource = Characters;
            localTimer = new System.Timers.Timer(1000);
            localTimer.Elapsed += LocalTimer_Elapsed;
            localTimer.AutoReset = true;
            localTimer.Enabled = true;

            countdownTimer = new System.Timers.Timer(1000);
            countdownTimer.Elapsed += CountdownTimer_Elapsed;
            countdownTimer.AutoReset = true;
        }

        private int countdown = 120;
        private void CountdownTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                TimerLabel.Content = countdown;
            });
            countdown--;
        }

        private bool firstRun = true;
        private void LocalTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            countdown = 120;
            countdownTimer.AutoReset = false;
            countdownTimer.Enabled = false;
            if (firstRun)
            {
                localTimer.Interval = 120000;
                firstRun = false;
            }

            foreach (var character in characterList)
            {
                double maxHealth = 1;
                int constitution = 0;
                int level = 0;
                double removedPoints = 0;
                double currentHealth = 0;
                bool hasInspiration = false;
                int tempHealth = 0;

                using (var client = new WebClient())
                using (var stream = client.OpenRead(character.DnDBeyondLink))
                using (var reader = new StreamReader(stream))
                {
                    var jObject = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadLine());

                    maxHealth = (int)jObject.SelectToken("baseHitPoints");
                    constitution = (int)jObject.SelectToken("stats")[2].SelectToken("value");
                    level = (int)jObject.SelectToken("classes")[0].SelectToken("level");
                    removedPoints = (int)jObject.SelectToken("removedHitPoints");
                    hasInspiration = (bool)jObject.SelectToken("inspiration");
                    tempHealth = (int)jObject.SelectToken("temporaryHitPoints");

                    foreach (var item in jObject.SelectToken("modifiers").SelectToken("race"))
                    {
                        string subtype = (string)item.SelectToken("subType");
                        if (subtype.Contains("constitution"))
                        {
                            constitution += (int)item.SelectToken("value");
                        }
                    }
                }

                maxHealth = (maxHealth + (((constitution / 2) - 5) * level));
                currentHealth = maxHealth - removedPoints;

                this.Dispatcher.Invoke(() =>
                {
                    float height = 200f * ((float)currentHealth / (float)maxHealth);
                    character.CharacterDisplay.Background.Height = height;
                    character.CharacterDisplay.Background.Margin = new Thickness(0, (200 - height), 0, 0);

                    character.CharacterDisplay.Inspiration.Visibility = hasInspiration ? Visibility.Visible : Visibility.Hidden;
                    character.CharacterDisplay.TempHPLabel.Content = tempHealth.ToString();

                    if (currentHealth < (maxHealth * .10))
                        character.CharacterDisplay.LowHealthAnimation.Begin(this, true);
                    else
                        character.CharacterDisplay.LowHealthAnimation.Stop(this);
                });

                System.Threading.Thread.Sleep(2000); //2000
            }

            countdownTimer.AutoReset = true;
            countdownTimer.Enabled = true;
        }
    }

    public class Character
    {
        public Character(string name, string dndBeyondLink, System.Windows.Media.Color color)
        {
            Name = name;
            FullName = name;
            DnDBeyondLink = dndBeyondLink;
            BackgroundColor = color;
        }

        public Character(string name, string fullname, string dndBeyondLink, System.Windows.Media.Color color)
        {
            Name = name;
            FullName = fullname;
            DnDBeyondLink = dndBeyondLink;
            BackgroundColor = color;
        }

        public string Name { get; set; }
        public string FullName { get; set; }
        public string DnDBeyondLink { get; set; }
        public Color BackgroundColor { get; set; }
        public int TotalHP { get; set; }
        public int CurrentHP { get; set; }
        public int TempHP { get; set; }
        public bool HasInspiration { get; set; }

        public CharacterDisplay CharacterDisplay { get; set; }

        public bool IsPC { get; set; }
        public int Initiative { get; set; }
        public int Dexterity { get; set; }
        public bool GoneThisRound { get; set; } = false;
    }

    public class CharacterDisplay
    {
        public Rectangle Background { get; set; }
        public Rectangle LowHealthWarning { get; set; }
        public Image CharImage { get; set; }
        public Image Inspiration { get; set; }
        public Image Shield { get; set; }
        public Image Nameplate { get; set; }
        public Label TempHPLabel { get; set; }
        public Label NameLabel { get; set; }
        public Storyboard LowHealthAnimation { get; set; }
    }
}
