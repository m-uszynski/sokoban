using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SokobanGraph
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameController gc;
        Level level;
        public static System.Windows.Threading.DispatcherTimer dispatcherTimer;
        public static int second = 0;

        public MainWindow(int choicedLevel, string nick)
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            level = new Level(choicedLevel);
            gc = new GameController(level,nick);
            level.generateLevel();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            second += 1;
            timer.Content = "Czas: " + second.ToString() + " s"; 
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            gc.updatePosition(e.Key);
        }

        private void backMoveButton_Click(object sender, RoutedEventArgs e)
        {
            var key = Key.P;
            var target = Keyboard.FocusedElement;
            var routedEvent = Keyboard.KeyDownEvent;

            target.RaiseEvent(
                new KeyEventArgs(
                    Keyboard.PrimaryDevice,
                    PresentationSource.FromVisual(this),
                    0,
                    key)
                { RoutedEvent = routedEvent }
            );
        }

        private void restartLevel_Click(object sender, RoutedEventArgs e)
        {
            var key = Key.R;
            var target = Keyboard.FocusedElement;
            var routedEvent = Keyboard.KeyDownEvent;

            target.RaiseEvent(
                new KeyEventArgs(
                    Keyboard.PrimaryDevice,
                    PresentationSource.FromVisual(this),
                    0,
                    key)
                { RoutedEvent = routedEvent }
            );
        }

        private void quitToMenu_Click(object sender, RoutedEventArgs e)
        {
            var key = Key.Q;
            var target = Keyboard.FocusedElement;
            var routedEvent = Keyboard.KeyDownEvent;

            target.RaiseEvent(
                new KeyEventArgs(
                    Keyboard.PrimaryDevice,
                    PresentationSource.FromVisual(this),
                    0,
                    key)
                { RoutedEvent = routedEvent }
            );
        }
    }
}
