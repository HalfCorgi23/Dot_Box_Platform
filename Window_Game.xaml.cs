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
        static int none = 0;    //无归属=0
        static int horizon = 0;    //横边=0
        static int vertice = 1;     //纵边=1
        static Window search_w;
        static string datefile;
        string date;
        //*****************************//
        public Window_Game(Color _color,int _type)
        {
            InitializeComponent();
            change_color(_color);
            initialize();
            drawline();             
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
        public void initialize()
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
            date = DateTime.Now.ToString();
        }
        public void change_color(Color _color)   //改变颜色
        {
            Border_Left.Background = new SolidColorBrush(_color);
            button2.Background = new SolidColorBrush(_color);
            label2.Background = new SolidColorBrush(_color);
            label1.Background = new SolidColorBrush(_color);
            label1_Copy.Background = new SolidColorBrush(_color);
            label4.Background = new SolidColorBrush(_color);
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
        public void drawline()
        {
            Canvas_Game.Children.Clear();
            for (int j=0;j<6;j++)
            {
                for(int i=0;i<5;i++)
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
                    else if(h[j, i] == none)
                    {
                        line1.Stroke = Brushes.Gray;
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
                        line1.Stroke = Brushes.Gray;
                    }
                    Canvas_Game.Children.Add(line1);
                }
            }
            drawbox();
        }
        public void drawbox()
        {

        }

        private void Canvas_Game_MouseMove(object sender, MouseEventArgs e)
        {
            //int[] array=get_position(e);
            //label1.Content = array[0];
            //label1_Copy.Content = array[1];
        }
        public int[] get_position(MouseEventArgs e)
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
                occ_line(type, x, y);
                step++;
                drawline();
            }
        }
        public void comp_step()
        {
            label5.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            search_w.Show();
            movestep mst = new movestep(h, v, box_edge, step);
            int[] move = mst.getmove();
            occ_line(move[0], move[1], move[2]);
            label5.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            search_w.Hide();
            drawline();
        }
        public void filesave()
        {
            using (StreamWriter sw = File.CreateText(datefile))
            {
                //sw.WriteLine(date);
                //sw.WriteLine("-----------------------------------------------");
                for (int i = 0; i < this.listBox_Copy.Items.Count; i++)
                {
                    sw.WriteLine(this.listBox_Copy.Items[i]);
                }
            }
        }
        public string lib_add(int _type, int _x, int _y)
        {
            string type=null;
            string x, y;
            string _turn = null;
            if (turn == player)
            {
                _turn = "玩家-";
            }
            else
            {
                _turn = "电脑-";
            }
            if (_type == horizon)
            {
                type = "横边-";
            }
            else
            {
                type = "纵边-";
            }

            x = _x.ToString()+"-";
            y = _y.ToString();
            string rtn = _turn+type + x + y;
            return rtn;
        }
        public string lib_add2(int _type, int _x, int _y)  //listBox_Copy用
        {
            string type = null;
            string x, y;
            string _turn = null;
            if (turn == player)
            {
                _turn = player.ToString()+"-";
            }
            else
            {
                _turn = comput.ToString()+"-";
            }
            if (_type == horizon)
            {
                type = horizon.ToString()+"-";
            }
            else
            {
                type = vertice.ToString()+"-";
            }

            x = _x.ToString() + "-";
            y = _y.ToString();
            string rtn = _turn + type + x + y;
            return rtn;
        }
        public void occ_line(int _type,int _x,int _y)
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
                            occ = occ_box( _x-1, _y);
                        }
                    }
                    else
                    {
                        box_edge[_x, _y]++;
                        box_edge[_x - 1, _y]++;
                        if (box_edge[_x - 1, _y] == 4)
                        {
                            occ = occ_box( _x - 1, _y);
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
            listBox.Items.Add(lib_add(_type, _x, _y));
            listBox_Copy.Items.Add(lib_add2(_type, _x, _y));
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
        public bool occ_box(int _x, int _y)
        {
            box[_x, _y] = turn;
            box_num--;
            if(turn == player)
            {
                play_score++;
                label1.Content = play_score.ToString();
            }
            else
            {
                com_score++;
                label1_Copy.Content = com_score.ToString();
            }
            if(box_num==0)
            {
                label5.Content = "游戏结束！！";
                label5.Foreground= new SolidColorBrush(Color.FromRgb(0,0,0));
            }
            return true;
        }
        public int[] itemtostep(string item)
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
        public void re_step(int re_turn,int re_type,int re_x,int re_y)
        {
            int count= this.listBox_Copy.Items.Count;
            this.listBox.Items.Remove(this.listBox.Items[count-1]);
            this.listBox_Copy.Items.Remove(this.listBox_Copy.Items[count - 1]);
            if (re_type == horizon)
            {
                h[re_x, re_y] = none;
                if(re_x == 0)
                {
                    box[re_x, re_y] = none;
                    box_edge[re_x, re_y]--;
                    if (box_edge[re_x, re_y] == 4)
                    {
                        box_num++;
                        if (re_turn == player)
                        {
                            play_score--;
                        }
                        else
                        {
                            com_score--;
                        }
                    }
                }
                else if (re_x == 5)
                {
                    box[re_x - 1, re_y] = none;
                    box_edge[re_x - 1, re_y]--;
                    if (box_edge[re_x - 1, re_y] == 4)
                    {
                        box_num++;
                        if (re_turn == player)
                        {
                            play_score--;
                        }
                        else
                        {
                            com_score--;
                        }
                    }
                }
                else
                {
                    if(box_edge[re_x, re_y]==4)
                    {
                        box_num++;
                        if (re_turn == player)
                        {
                            play_score--;
                        }
                        else
                        {
                            com_score--;
                        }
                    }
                    if (box_edge[re_x - 1, re_y] == 4)
                    {
                        box_num++;
                        if (re_turn == player)
                        {
                            play_score--;
                        }
                        else
                        {
                            com_score--;
                        }
                    }
                    box[re_x, re_y] = none;
                    box_edge[re_x, re_y]--;
                    box[re_x - 1, re_y] = none;
                    box_edge[re_x - 1, re_y]--;
                }
            }
            else 
            {
                v[re_x, re_y] = none;
                if (re_y == 0)
                {
                    box[re_x, re_y] = none;
                    box_edge[re_x, re_y]--;
                    if (box_edge[re_x, re_y] == 4)
                    {
                        box_num++;
                        if (re_turn == player)
                        {
                            play_score--;
                        }
                        else
                        {
                            com_score--;
                        }
                    }
                }
                else if (re_y == 5)
                {
                    box[re_x, re_y - 1] = none;
                    box_edge[re_x, re_y - 1]--;
                    if (box_edge[re_x, re_y - 1] == 4)
                    {
                        box_num++;
                        if (re_turn == player)
                        {
                            play_score--;
                        }
                        else
                        {
                            com_score--;
                        }
                    }
                }
                else
                {
                    if (box_edge[re_x, re_y] == 4)
                    {
                        box_num++;
                        if (re_turn == player)
                        {
                            play_score--;
                        }
                        else
                        {
                            com_score--;
                        }
                    }
                    if (box_edge[re_x, re_y - 1] == 4)
                    {
                        box_num++;
                        if (re_turn == player)
                        {
                            play_score--;
                        }
                        else
                        {
                            com_score--;
                        }
                    }
                    box[re_x, re_y] = none;
                    box_edge[re_x, re_y]--;
                    box[re_x, re_y - 1] = none;
                    box_edge[re_x, re_y - 1]--;
                }
            }
        }
        public void re_click()
        {
            int[] step = new int[4];
            do
            {  
                step = itemtostep(listBox_Copy.Items[listBox_Copy.Items.Count-1].ToString());
                re_step(step[0], step[1], step[2], step[3]);
                listBox_Copy.Items.Remove(listBox_Copy.Items[listBox_Copy.Items.Count - 1]);
                listBox.Items.Remove(listBox.Items[listBox.Items.Count - 1]);
            }
            while (step[0] == comput);
            //step = itemtostep(listBox_Copy.Items[listBox_Copy.Items.Count - 1].ToString());
            //re_step(step[0], step[1], step[2], step[3]);
            //listBox_Copy.Items.Remove(listBox_Copy.Items[listBox_Copy.Items.Count - 1]);
            //listBox.Items.Remove(listBox.Items[listBox.Items.Count - 1]);
            turn = player;
            filesave();
            drawline();
        }
    }
}
