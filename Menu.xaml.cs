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
using System.Windows.Shapes;

namespace SokobanGraph
{
    /// <summary>
    /// Logika interakcji dla klasy Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
            logo.Source = new BitmapImage(new Uri("pack://application:,,,/sprites/logo.png"));
            if (System.IO.File.Exists("lastUsername.txt"))
            {
                string nick = System.IO.File.ReadAllText("lastUsername.txt");
                if(nick != "" && nick != null) nickLabel.Text = System.IO.File.ReadAllText("lastUsername.txt");
            }
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow mw = new MainWindow();
            //mw.Show();
            string nick = nickLabel.Text;
            System.IO.File.WriteAllText("lastUsername.txt", nick);
            LevelChoose lc = new LevelChoose(nick);
            lc.Show();
            this.Close();
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
