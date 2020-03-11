using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitiativeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Tuple<string, string>> characterList = new List<Tuple<string, string>>() {
            new Tuple<string, string>("Hella", "https://www.dndbeyond.com/character/22078134/json"),
            new Tuple<string, string>("Leedle", "https://www.dndbeyond.com/character/22081606/json"),
            new Tuple<string, string>("Key", "https://www.dndbeyond.com/character/22960105/json"),
            new Tuple<string, string>("Jon", "https://www.dndbeyond.com/character/21973481/json"),
            new Tuple<string, string>("Shazlight", "https://www.dndbeyond.com/character/21211796/json")};

        private List<Character> Characters = new List<Character>();
        private static System.Timers.Timer localTimer;

        public MainWindow()
        {
            InitializeComponent();
            lstCharacters.ItemsSource = Characters;
            localTimer = new System.Timers.Timer(1000);
            localTimer.Elapsed += LocalTimer_Elapsed;
            localTimer.AutoReset = true;
            localTimer.Enabled = true;
        }

        private bool firstRun = true;
        private void LocalTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (firstRun)
            {
                localTimer.Interval = 120000;
                firstRun = false;
            }

            foreach (var character in characterList)
            {
                int maxHealth = 0;
                int constitution = 0;
                int level = 0;
                int removedPoints = 0;
                bool hasInspiration = false;

                using (var client = new WebClient())
                using (var stream = client.OpenRead(character.Item2))
                using (var reader = new StreamReader(stream))
                {
                    var jObject = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadLine());

                    maxHealth = (int)jObject.SelectToken("baseHitPoints");
                    constitution = (int)jObject.SelectToken("stats")[2].SelectToken("value");
                    level = (int)jObject.SelectToken("classes")[0].SelectToken("level");
                    removedPoints = (int)jObject.SelectToken("removedHitPoints");
                    hasInspiration = (bool)jObject.SelectToken("inspiration");

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

                using (StreamWriter write = new StreamWriter("C:\\DnDStuff\\" + character.Item1 + "Health.txt"))
                {
                    write.WriteLine(maxHealth);
                }

                using (StreamWriter write = new StreamWriter("C:\\DnDStuff\\" + character.Item1 + "HealthCurrent.txt"))
                {
                    write.WriteLine((maxHealth - removedPoints));
                }

                using (StreamWriter write = new StreamWriter("C:\\DnDStuff\\" + character.Item1 + "Inspiration.txt"))
                {
                    if (hasInspiration)
                        write.WriteLine("INS");
                    else
                        write.WriteLine("");
                }

                this.Dispatcher.Invoke(() =>
                {
                    txtBlock.Text += "Char: " + character.Item1 + " ( " + level + " )" + Environment.NewLine;
                    txtBlock.Text += "     HP: " + (maxHealth - removedPoints) + " / " + maxHealth + Environment.NewLine;
                    txtBlock.Text += "     Inspiration: " + hasInspiration + Environment.NewLine;
                });

                System.Threading.Thread.Sleep(2000);
            }

            this.Dispatcher.Invoke(() =>
            {
                txtBlock.Text += Environment.NewLine + DateTime.Now.ToLongTimeString() + ": Retrieved" + Environment.NewLine;
            });
        }

        private void BtnAddChar_Click(object sender, RoutedEventArgs e)
        {
            int dexterity;
            if (txtCharInput.Text != "" && Int32.TryParse(txtDexterity.Text, out dexterity))
            {
                Characters.Add(new Character(txtCharInput.Text, dexterity, true));
                lstCharacters.ItemsSource = null;
                lstCharacters.ItemsSource = Characters;
            }
        }

        private void BtnRoll_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            foreach (Character character in Characters)
            {
                character.Initiative = rnd.Next(1, 20) + character.Dexterity;
                character.GoneThisRound = false;
            }
            
            Characters.Sort((a, b) => b.Initiative.CompareTo(a.Initiative));
            
            lstCharacters.ItemsSource = null;
            lstCharacters.ItemsSource = Characters;
        }

        private void BtnNPCInput_Click(object sender, RoutedEventArgs e)
        {
            int dexterity;
            if (txtCharInput.Text != "" && Int32.TryParse(txtDexterity.Text, out dexterity))
            {
                int count = 1;
                foreach (Character character in Characters)
                {
                    if (character.Name.Contains(txtCharInput.Text))
                    {
                        count++;
                    }
                }

                Characters.Add(new Character(txtCharInput.Text + count.ToString(), dexterity, false));
                lstCharacters.ItemsSource = null;
                lstCharacters.ItemsSource = Characters;
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Characters.RemoveAll(s => !s.IsPC);

            lstCharacters.ItemsSource = null;
            lstCharacters.ItemsSource = Characters;
        }

        private void delete_char_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)lstCharacters.ItemContainerGenerator.ContainerFromItem(((Button)sender).DataContext);
            selectedItem.IsSelected = true;

            Characters.RemoveAt(lstCharacters.SelectedIndex);

            lstCharacters.ItemsSource = null;
            lstCharacters.ItemsSource = Characters;
        }

        #region Watermark
        private void txtCharInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtCharInput.Text == "Character Name")
            {
                txtCharInput.Text = "";
            }
        }

        private void txtCharInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCharInput.Text))
            {
                txtCharInput.Text = "Character Name";
            }
        }

        private void txtDexterity_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtDexterity.Text == "Initiative")
            {
                txtDexterity.Text = "";
            }
        }

        private void txtDexterity_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDexterity.Text))
            {
                txtDexterity.Text = "Initiative";
            }
        }
        #endregion
    }

    public class Character
    {
        public Character(string name, int dext, bool ispc)
        {
            Name = name;
            IsPC = ispc;
            Dexterity = dext;
        }

        public string Name { get; set; }
        public bool IsPC { get; set; }
        public int Initiative { get; set; }
        public int Dexterity { get; set; }
        public bool GoneThisRound { get; set; } = false;
    }
}
