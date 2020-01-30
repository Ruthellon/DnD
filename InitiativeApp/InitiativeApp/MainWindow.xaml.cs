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
        }

        private void BtnAddChar_Click(object sender, RoutedEventArgs e)
        {
            Characters.Add(new Character(txtCharInput.Text, Convert.ToInt32(txtDexterity.Text), true));
            txtResults.Text += txtCharInput.Text + Environment.NewLine;
        }

        private void BtnRoll_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            foreach (Character character in Characters)
            {
                character.Initiative = rnd.Next(1, 20) + character.Dexterity;
            }

            txtResults.Text = "";

            Characters.Sort((a, b) => a.Initiative.CompareTo(b.Initiative));

            foreach (Character character in Characters)
            {
                txtResults.Text += character.Name + Environment.NewLine;
            }
        }

        private void BtnNPCInput_Click(object sender, RoutedEventArgs e)
        {
            Characters.Add(new Character(txtCharInput.Text, Convert.ToInt32(txtDexterity.Text), false));
            txtResults.Text += txtCharInput.Text + Environment.NewLine;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                if (!Characters[i].IsPC)
                {
                    Characters.RemoveAt(i);
                    i--;
                }
            }

            txtResults.Text = "";

            foreach (Character character in Characters)
            {
                txtResults.Text += character.Name + Environment.NewLine;
            }
        }
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
    }
}
