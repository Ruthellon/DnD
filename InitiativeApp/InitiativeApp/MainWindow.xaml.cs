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

namespace InitiativeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Character> Characters = new List<Character>();

        public MainWindow()
        {
            InitializeComponent();
            lstCharacters.ItemsSource = Characters;
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
