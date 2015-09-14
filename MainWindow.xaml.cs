using Microsoft.Win32;
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

namespace Dot_Box_Platform
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        static Color colorset = Color.FromArgb(100, 14, 153, 83);
        public MainWindow()
        {
            InitializeComponent();
            if(Settings1.Default.UI_Color==2)
            {
                colorset = Color.FromArgb(100, 76, 161, 185);
            }
            else if(Settings1.Default.UI_Color == 1)
            {
                colorset = Color.FromArgb(100, 209, 65, 129);
            }
            else
            {
                colorset = Color.FromArgb(100, 14, 153, 83);
            }
            changeuicolor(colorset);
            label4.Content = Settings1.Default.Current_Save1;
            label4_Copy.Content = Settings1.Default.Current_Save1;
            label4_Copy1.Content = Settings1.Default.Current_Save2;
            label4_Copy2.Content = Settings1.Default.Current_Save3;
        }

        private void button_Copy4_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void button_Copy3_Click(object sender, RoutedEventArgs e)
        {
            Window1 wd1 = new Window1();
            wd1.ShowDialog();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void button2_Copy1_Click(object sender, RoutedEventArgs e)
        {
            colorset = Color.FromArgb(100, 14, 153, 83);
            Settings1.Default.UI_Color = 0;
            Settings1.Default.Save();
            changeuicolor(colorset);
        }
        public void changeuicolor(Color _color)
        {
            button.Background = new SolidColorBrush(_color);
            button_Copy.Background = new SolidColorBrush(_color);
            button_Copy1.Background = new SolidColorBrush(_color);
            Grid1.Background = new SolidColorBrush(_color);
            button_Copy2.Background = new SolidColorBrush(_color);
            button_Copy3.Background = new SolidColorBrush(_color);
            button_Copy4.Background = new SolidColorBrush(_color);
        }

        private void button2_Copy_Click(object sender, RoutedEventArgs e)
        {
            colorset = Color.FromArgb(100, 209, 65, 129);
            Settings1.Default.UI_Color = 1;
            Settings1.Default.Save();
            changeuicolor(colorset);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            colorset = Color.FromArgb(100, 76, 161, 185);
            Settings1.Default.UI_Color = 2;
            Settings1.Default.Save();
            changeuicolor(colorset);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
            Window wd = new Window_Game(colorset, 1);
            wd.Owner = this;
            wd.ShowDialog();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
            Window wd = new Window_Game(colorset, 2);
            wd.Owner = this;
            wd.ShowDialog();
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "存盘文件|*.sav";
            fd.ShowReadOnly = true;
            fd.ShowDialog();
            string path = fd.SafeFileName;
            Window wd = new Window_Game(colorset, 1,path);
            this.Visibility = System.Windows.Visibility.Hidden;
            wd.Owner = this;
            wd.ShowDialog();
        }
    }
}
