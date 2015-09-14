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
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace Dot_Box_Platform
{
    /// <summary>
    /// Window_Game.xaml 的交互逻辑
    /// </summary>
    public partial class Window_Game : Window
    {
        static int gametype;
        static int turn;
        //************归属信息******************//
        static int[,] h;   //横边
        static int[,] v;   //纵边
        static int[,] box;    //格子
        //******************数据信息************//
        static int[,] box_edge; //格子剩余边数（自由度）
        static int box_num;  //剩余格子数
        static int play_score;
        static int com_score;
        static int step;
        //**************固定字段****************//
        static int player = 1;   //玩家=1
        static int comput = 2;    //电脑=2
        //static int test_mode = 3;
        static int none = 0;    //无归属=0
        static int horizon = 0;    //横边=0
        static int vertice = 1;     //纵边=1
        static Window search_w;
        static string datefile;
        //string date;
        //*****************************//
        public Window_Game(Color _color, int _type)
        {
            InitializeComponent();
            change_color(_color);
            initialize();
            draw();
            gametype = _type;
            if (gametype == 2)
            {
                //电脑先手
                turn = comput;
                comp_step();
            }
            else if (gametype == 1)
            {
                //玩家先手
                turn = player;
            }
        }
        public Window_Game(Color _color, int _type,string filename)
        {
            InitializeComponent();
            change_color(_color);
            initialize();
            fileload(filename);
            turn = player;
            draw();
        }
        private void initialize()
        {
            h = new int[6, 5];
            v = new int[5, 6];
            box = new int[5, 5];
            box_num = 25;
            box_edge = new int[5, 5];
            play_score = 0;
            com_score = 0;
            step = 0;
            search_w = new Searching();
            datefile = DateTime.Now.ToFileTime().ToString() + ".sav";
            //date = DateTime.Now.ToString();
            FileStream fs_cre = new FileStream(datefile, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            Settings1.Default.Current_Save3 = Settings1.Default.Current_Save2;
            Settings1.Default.Current_Save2 = Settings1.Default.Current_Save1;
            Settings1.Default.Current_Save1 = datefile;
            re_list[0] = new state(h, v, box, box_edge, box_num, play_score, com_score, step);
            fs_cre.Close();
        }
        private void change_color(Color _color)   //改变颜色
        {
            Border_Left.Background = new SolidColorBrush(_color);
            button2.Background = new SolidColorBrush(_color);
            label2.Background = new SolidColorBrush(_color);
            label1.Background = new SolidColorBrush(_color);
            label1_Copy.Background = new SolidColorBrush(_color);
            //label4.Background = new SolidColorBrush(_color);
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            this.DialogResult = true;
        }
        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //label4.Content = listBox_Copy.Items[listBox_Copy.Items.Count - 1].ToString();
            re_click();
        }
        private void draw()
        {
            Canvas_Game.Children.Clear();
            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    Line line1 = new Line();
                    line1.X1 = 64 * i;
                    line1.Y1 = 64 * j;
                    line1.X2 = 64 * (i + 1);
                    line1.Y2 = 64 * j;
                    if (h[j, i] == player)
                    {
                        line1.Stroke = Brushes.Red;
                    }
                    else if (h[j, i] == comput)
                    {
                        line1.Stroke = Brushes.Blue;
                    }
                    else if (h[j, i] == none)
                    {
                        line1.Stroke = Brushes.Lavender;
                    }
                    Canvas_Game.Children.Add(line1);
                }
            }
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    Line line1 = new Line();
                    line1.X1 = 64 * i;
                    line1.Y1 = 64 * j;
                    line1.X2 = 64 * i;
                    line1.Y2 = 64 * (j + 1);
                    if (v[j, i] == player)
                    {
                        line1.Stroke = Brushes.Red;
                    }
                    else if (v[j, i] == comput)
                    {
                        line1.Stroke = Brushes.Blue;
                    }
                    else if (v[j, i] == none)
                    {
                        line1.Stroke = Brushes.Lavender;
                    }
                    Canvas_Game.Children.Add(line1);
                }
            }
            for(int i=0;i<5;i++)
            {
                for(int j=0;j<5;j++)
                {
                    Rectangle box_ = new Rectangle();
                    Thickness margin = new Thickness(2+64*j, 2 + 64 * i, 0, 0);
                    box_.Margin= margin;
                    box_.Height = 60;
                    box_.Width = 60;
                    if (box[i, j] == comput)
                    {
                        box_.Fill = Brushes.Blue;
                    }
                    else if(box[i, j] == player)
                    {
                        box_.Fill = Brushes.Red;
                    }
                    else
                    {
                        box_.Fill = Brushes.White;
                    }
                    Canvas_Game.Children.Add(box_);
                }
            }
        }
        private void Canvas_Game_MouseMove(object sender, MouseEventArgs e)
        {
            //int[] array=get_position(e);
            //label1.Content = array[0];
            //label1_Copy.Content = array[1];
        }
        private int[] get_position(MouseEventArgs e)
        {
            Point p = e.GetPosition(Canvas_Game);
            int a;
            int b;
            //if(p.X%70)  
            if (p.X <= 20)
            {
                a = 0;
            }
            else if (p.X > 20 & p.X <= 60)
            {
                a = 1;
            }
            else if (p.X > 60 & p.X <= 80)
            {
                a = 2;
            }
            else if (p.X > 80 & p.X <= 120)
            {
                a = 3;
            }
            else if (p.X > 120 & p.X <= 140)
            {
                a = 4;
            }
            else if (p.X > 140 & p.X <= 180)
            {
                a = 5;
            }
            else if (p.X > 180 & p.X <= 200)
            {
                a = 6;
            }
            else if (p.X > 200 & p.X <= 240)
            {
                a = 7;
            }
            else if (p.X > 240 & p.X <= 260)
            {
                a = 8;
            }
            else if (p.X > 260 & p.X <= 300)
            {
                a = 9;
            }
            else if (p.X > 300 & p.X <= 330)
            {
                a = 10;
            }
            else
            {
                a = 11;
            }
            //************************//
            if (p.Y <= 20)
            {
                b = 0;
            }
            else if (p.Y > 20 & p.Y <= 60)
            {
                b = 1;
            }
            else if (p.Y > 60 & p.Y <= 80)
            {
                b = 2;
            }
            else if (p.Y > 80 & p.Y <= 120)
            {
                b = 3;
            }
            else if (p.Y > 120 & p.Y <= 140)
            {
                b = 4;
            }
            else if (p.Y > 140 & p.Y <= 180)
            {
                b = 5;
            }
            else if (p.Y > 180 & p.Y <= 200)
            {
                b = 6;
            }
            else if (p.Y > 200 & p.Y <= 240)
            {
                b = 7;
            }
            else if (p.Y > 240 & p.Y <= 260)
            {
                b = 8;
            }
            else if (p.Y > 260 & p.Y <= 300)
            {
                b = 9;
            }
            else if (p.Y > 300 & p.Y <= 330)
            {
                b = 10;
            }
            else
            {
                b = 11;
            }
            int[] rtn = new int[2] { a, b };
            return rtn;
        }

        private void Canvas_Game_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (turn == player)
            {
                int type = 0;
                int x = 0;
                int y = 0;
                int b = get_position(e)[0];
                int a = get_position(e)[1];
                if (a % 2 == 0)
                {
                    x = a / 2;
                    y = (b - 1) / 2;
                    type = horizon;

                }
                else if (a % 2 == 1)
                {
                    x = (a - 1) / 2;
                    y = b / 2;
                    type = vertice;
                }
                if (type == horizon)
                {
                    if (h[x, y] == none)
                    {
                        occ_line(type, x, y);
                        step++;
                    }
                }
                else if (type == vertice)
                {
                    if (v[x, y] == none)
                    {
                        occ_line(type, x, y);
                        step++;
                    }
                }
                draw();
            }
        }
        private void comp_step()
        {
            //label5.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            search_w.Show();
            nextmove nm = new nextmove(h, v, box_edge, step);
            int[] move = nm.get();
            if (move[1] == 0 && move[2] == 0 && box_edge[0, 0] >= 2)
            {
                MessageBox.Show("chucuo!");
            }
            else
            {
                occ_line(move[0], move[1], move[2]);
                //label5.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                listno++;
                re_list[listno] = new state(h, v, box, box_edge, box_num, play_score, com_score, step);
                search_w.Hide();
                draw();
            }
        }
        private void filesave()
        {
            FileStream fs_w = new FileStream(datefile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            StreamWriter sw = new StreamWriter(fs_w);
            int i, j;
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    sw.WriteLine(h[i, j].ToString());
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    sw.WriteLine(v[i, j].ToString());
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    sw.WriteLine(box[i, j].ToString());
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    sw.WriteLine(box_edge[i, j].ToString());
                }
            }
            sw.WriteLine(box_num.ToString());
            sw.WriteLine(play_score.ToString());
            sw.WriteLine(com_score.ToString());
            sw.WriteLine(step.ToString());
            sw.Close();
            fs_w.Close();
        }
        private void fileload(string filename)
        {
            FileStream loadfs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            StreamReader sr = new StreamReader(loadfs);
            int i, j;
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    h[i, j] = int.Parse(sr.ReadLine());
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    v[i, j] = int.Parse(sr.ReadLine());
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    box[i, j] = int.Parse(sr.ReadLine());
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    box_edge[i, j] = int.Parse(sr.ReadLine());
                }
            }
            box_num = int.Parse(sr.ReadLine());
            play_score = int.Parse(sr.ReadLine());
            com_score = int.Parse(sr.ReadLine());
            step = int.Parse(sr.ReadLine());
            sr.Close();
            loadfs.Close();
        }
        private void occ_line(int _type, int _x, int _y)
        {
            bool occ = false;
            if (_type == horizon)
            {
                if (h[_x, _y] == none)
                {
                    h[_x, _y] = turn;
                    if (_x == 0)
                    {
                        box_edge[_x, _y]++;
                        if (box_edge[_x, _y] == 4)
                        {
                            occ = occ_box(_x, _y);
                        }
                    }
                    else if (_x == 5)
                    {
                        box_edge[_x - 1, _y]++;
                        if (box_edge[_x - 1, _y] == 4)
                        {
                            occ = occ_box(_x - 1, _y);
                        }
                    }
                    else
                    {
                        box_edge[_x, _y]++;
                        box_edge[_x - 1, _y]++;
                        if (box_edge[_x - 1, _y] == 4)
                        {
                            occ = occ_box(_x - 1, _y);
                        }
                        if (box_edge[_x, _y] == 4)
                        {
                            occ = occ_box(_x, _y);
                        }
                    }
                }
            }
            else if (_type == vertice)
            {
                if (v[_x, _y] == none)
                {
                    v[_x, _y] = turn;
                    if (_y == 0)
                    {
                        box_edge[_x, _y]++;
                        if (box_edge[_x, _y] == 4)
                        {
                            occ = occ_box(_x, _y);
                        }
                    }
                    else if (_y == 5)
                    {
                        box_edge[_x, _y - 1]++;
                        if (box_edge[_x, _y - 1] == 4)
                        {
                            occ = occ_box(_x, _y - 1);
                        }
                    }
                    else
                    {
                        box_edge[_x, _y]++;
                        box_edge[_x, _y - 1]++;
                        if (box_edge[_x, _y - 1] == 4)
                        {
                            occ = occ_box(_x, _y - 1);
                        }
                        if (box_edge[_x, _y] == 4)
                        {
                            occ = occ_box(_x, _y);
                        }
                    }
                }
            }
            //listBox.Items.Add(lib_add(_type, _x, _y));
            //listBox_Copy.Items.Add(lib_add2(_type, _x, _y));
            filesave();
            if (!occ)
            {
                if (turn == player)
                {
                    turn = comput;
                    comp_step();
                }
                else if (turn == comput)
                {
                    turn = player;
                }
            }
            else
            {
                if (turn == comput)
                {
                    comp_step();
                }
            }
        }
        private bool occ_box(int _x, int _y)
        {
            box[_x, _y] = turn;
            box_num--;
            if (turn == player)
            {
                play_score++;
                label1.Content = play_score.ToString();
            }
            else
            {
                com_score++;
                label1_Copy.Content = com_score.ToString();
            }
            if (box_num == 0)
            {
                //label5.Content = "游戏结束！！";
                //label5.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
            return true;
        }
        private int[] itemtostep(string item)
        {
            int[] rtn = new int[4];
            int i = 0;
            //string regexStr1 = "(\\d)-(\\d)-()";
            Regex regex = new Regex(@"\d+", RegexOptions.ECMAScript);
            Match match = regex.Match(item);
            while (match.Value.Length != 0)
            {
                rtn[i] = int.Parse(match.Value);
                i++;
                match = regex.Match(item, match.Index + match.Value.Length);
            }
            //rtn[0] = int.Parse(regex.Match(item).Groups[0].Value);
            //rtn[1] = int.Parse(regex.Match(item).Groups[1].Value);
            //rtn[2] = int.Parse(regex.Match(item).Groups[2].Value);
            //rtn[3] = int.Parse(regex.Match(item).Groups[3].Value);
            return rtn;
        }
        private void re_step()
        {
            re_list[listno - 1].restep(h, v, box, box_edge, box_num, play_score, com_score, step);
            listno--;
        }
        private void re_click()
        {
            re_step();
            filesave();
            draw();
        }
        private static state[] re_list = new state[600];
        private static int listno = 0;
    }
    class state
    {
        int[,] h=new int[6,5];
        int[,] v=new int[5,6];
        int[,] box=new int[5,5];
        int[,] box_edge = new int[5, 5];
        int box_num;
        int play_score;
        int com_score;
        int step;
        public state(int[,] _h,int[,] _v,int[,] _box,int[,] _boxedge,int boxnum,int players,int coms,int _step)
        {
            int i, j;
            for (i=0;i<6;i++)
            {
                for(j=0;j<5; j++)
                {
                    h[i,j] = _h[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    v[i,j] = _v[i,j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    box[i, j] = _box[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    box_edge[i, j] = _boxedge[i, j];
                }
            }
            box_num=boxnum;
            play_score=players;
            com_score=coms;
            step=_step;
        }
        public void restep(int[,] _h, int[,] _v, int[,] _box, int[,] _boxedge, int boxnum, int players, int coms, int _step)
        {
            int i, j;
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    _h[i, j] = h[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    _v[i, j] = v[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    _box[i, j] = box[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    _boxedge[i,j] = box_edge[i,j];
                }
            }
            boxnum = box_num;
            players = play_score;
            coms = com_score;
            _step = step;
        }
    }
}
