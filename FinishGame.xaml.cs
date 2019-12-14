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
    /// Logika interakcji dla klasy FinishGame.xaml
    /// </summary>
    public partial class FinishGame : Window
    {
        private int choice = 2;
        public FinishGame()
        {
            InitializeComponent();
            icon.Source = new BitmapImage(new Uri("pack://application:,,,/sprites/success.png"));
            time.Content = "Czas: " + MainWindow.second + " s";
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Q:
                    this.DialogResult = true;
                    choice = 1;
                    break;

                case Key.R:
                    this.DialogResult = true;
                    choice = 2;
                    break;
            }
        }

        private void one_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            choice = 1;
        }

        private void two_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            choice = 2;
        }

        public int Choice
        {
            get { return choice; }
        }
    }
}

