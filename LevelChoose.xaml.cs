using System;
using System.Collections;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SokobanGraph
{
    /// <summary>
    /// Logika interakcji dla klasy LevelChoose.xaml
    /// </summary>
    public partial class LevelChoose : Window
    {
        public static List<User> userList = new List<User>();
        string currentNick;

        public LevelChoose(string nick)
        {
            InitializeComponent();
            currentNick = nick;

            welcomeLabel.Content = nick + ", wybierz level:";

            ReadUsers();

            if (!userIsExists(nick))
            {
                AddNewUser(new User(nick));
            }

            var user = userList.Where(u => u.Nick == nick).FirstOrDefault();

            for (int i=0; i<Level.levelCount(); i++)
            {
                var tb = new Button();
                tb.Tag = (i+1);
                tb.Width = 120;
                tb.Height = 30;
                tb.Background = new SolidColorBrush(Color.FromArgb(255,191,217,255));
                tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 14, 37, 114));
                tb.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 8, 23, 93));
                tb.BorderThickness = new Thickness(1);
                tb.FontSize = 16;
                tb.FontWeight = FontWeights.Bold;
                tb.FontFamily = new FontFamily("Microsoft Sans Serif");
                tb.Cursor = Cursors.Hand;
                if (user.LastLevel < (i + 1)) tb.IsEnabled = false;
                tb.Click += runLevel_Click;
                canvas.Children.Add(tb);
                tb.Content = "Level " + (i+1);
                Canvas.SetTop(tb, (i + 1) * 35);
            }

            var tbBack = new Button();
            tbBack.Width = 120;
            tbBack.Height = 30;
            tbBack.Background = new SolidColorBrush(Color.FromArgb(255, 191, 217, 255));
            tbBack.Foreground = new SolidColorBrush(Color.FromArgb(255, 14, 37, 114));
            tbBack.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 8, 23, 93));
            tbBack.BorderThickness = new Thickness(1);
            tbBack.FontSize = 16;
            tbBack.FontWeight = FontWeights.Bold;
            tbBack.FontFamily = new FontFamily("Microsoft Sans Serif");
            tbBack.Cursor = Cursors.Hand;
            tbBack.Click += backMenu_Click;
            canvas.Children.Add(tbBack);
            tbBack.Content = "Powrót";
            Canvas.SetTop(tbBack, (Level.levelCount() + 1) * 35);

            canvas.Height = (Level.levelCount()+1) * 35 + 50;

            //for(int i=3; i<10; i++)
            //{
            //    var tb = new Button();
            //    tb.Tag = (i + 1);
            //    tb.Width = 100;
            //    tb.Height = 25;
            //    tb.IsEnabled = false;
            //    canvas.Children.Add(tb);
            //    tb.Content = "Level " + (i + 1);
            //    Canvas.SetTop(tb, (i + 1) * 30);
            //}

            //canvas.Height = 10 * 30+50;
        }

        public void runLevel_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            int level = Convert.ToInt32(btn.Tag);
            MainWindow mw = new MainWindow(level,currentNick);
            mw.Show();
            this.Close();
        }

        public void backMenu_Click(object sender, RoutedEventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            this.Close();
        }

        public void AddNewUser(User u)
        {
            userList.Add(u);
            string json = JsonConvert.SerializeObject(userList);
            System.IO.File.WriteAllText("users.json", json);
        }

        public void ReadUsers()
        {
            string jsonConv = System.IO.File.ReadAllText("users.json");
            if (jsonConv == "") return;
            List<User> newUsers = JsonConvert.DeserializeObject<List<User>>(jsonConv);
            userList.Clear();
            userList = newUsers;
        }

        public bool userIsExists(string nick)
        {
            bool isExists = false;
            if (userList==null) return false;
            User u = userList.Where(us => us.Nick == nick).FirstOrDefault();
            if (userList.Contains(u)) isExists = true;
            return isExists;
        }
    }
}
