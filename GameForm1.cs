using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Dot_Box_Killer
{
    public partial class GameForm1 : Form
    {
        static string datefile;
        string date;
        public GameForm1()
        {
            InitializeComponent();
        }       
        //*******************************************************************//
        int gametype;
        int pvc1 = 0;      //玩家对电脑，先手
        int pvc2 = 1;      //玩家对电脑，后手
        //*******************************************************************//
        int turn;             // 手，全局变量
        int player = 1;         //玩家  
        int computer = 2;     //电脑
        int noplayer = 0;      //未有玩家占格     
        //******************************************************************//
        public int[,] h = new int[6, 5];
        public int[,] v = new int[5, 6];
        int horizon = 0;
        int vertice = 1; 
        //******************************************************************//
        public int[,] box = new int[5, 5];      //格子被占边数
        public int[,] box_s = new int[5, 5];     //格子被占状态
        public int box_no = 25;    //剩余未被占格子数
        int second_com;
        int minute_com;
        int second_pla;
        int minute_pla;
        
        private void button_Click(object sender, EventArgs e)
        {
            /**********玩家操作*********/
            Button btn1;
            btn1 = (Button)sender;
            string name = btn1.Name;
            int[] array = nametoarray(name);
            int type = array[0];
            int x = array[1];
            int y = array[2];
            btn1.BackColor = Color.Blue;
            step(player, type, x, y);
            string step_p=name+"  玩家";
            listBox1.Items.Add(step_p);
            btn1.Enabled = false;
            timer_pla.Stop();
            /**********电脑操作*********/
            timer_com.Interval = 1000;
            timer_com.Start();
            Search search=new Search();
            int[] com_array = search.computerstep(h,v);
            int type2 = com_array[0];
            int x2 = com_array[1];
            int y2 = com_array[2];
            step(computer, type2, x2, y2);
            string etype;
            if(type2==horizon)
            {
                etype = "h";
            }
            else
            {
                etype = "v";
            }
            string name2 = etype + "_" + x2.ToString() + "_" + y2.ToString();
            foreach (Control control in this.Controls)
            {
                if (control.GetType() == typeof(Button))
                {
                    Button bt = control as Button;
                    if (control.Name == name2) //查找Name2的控件  
                    {
                        string step_c=name2 + "  电脑";
                        listBox1.Items.Add(step_c);
                        bt.BackColor = Color.Red;
                        bt.Enabled = false; 
                    }
                }
            }
            timer_com.Stop();
            timer_pla.Interval = 1000;
            timer_pla.Start();
            savegame();
            
        }
        public void initialize()       //初始化游戏数据（游戏类型）
        {
            int i, j;
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    h[i, j] = 0;
                }
            }
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    v[i, j] = 0;
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    box[i, j] = 0;
                }
            }
            if (gametype == pvc2)
            {
                turn = 1;
            }
            else if (gametype == pvc1)
            {
                turn = 0;
            }
            box_no = 25;
        }
        public void step(int _turn,int line_type,int line_x,int line_y)      //下步（手，边类型，横坐标，纵坐标）
         { 
            if(_turn==computer)
            {
                if(line_type==horizon)
                {
                    h[line_x, line_y] = computer;
                    switch(line_x)
                    {
                        case 0:
                            box[line_x, line_y]++;
                            break;
                        case 5:
                            int _x1 = line_x - 1;
                            box[_x1, line_y]++;
                            break;
                        default:
                            box[line_x, line_y]++;
                            int _x2 = line_x - 1;
                            box[_x2, line_y]++;
                            break;
                    }
                    occupy(_turn,line_type,line_x,line_y);
                }
                if (line_type == vertice)
                {
                    v[line_x, line_y] = computer;
                    switch (line_y)
                    {
                        case 0:
                            box[line_x, line_y]++;
                            break;
                        case 5:
                            int _y1 = line_y - 1;
                            box[line_x, _y1]++;
                            break;
                        default:
                            box[line_x, line_y]++;
                            int _y2 = line_y - 1;
                            box[line_x, _y2]++;
                            break;
                    }
                    occupy(_turn, line_type, line_x, line_y);
                }
                turn = player;
            }
            /*****************************************************/
            if (_turn == player)
            {
                if (line_type == horizon)
                {
                    h[line_x, line_y] = player;
                    switch (line_x)
                    {
                        case 0:
                            box[line_x, line_y]++;
                            break;
                        case 5:
                            int _x1 = line_x - 1;
                            box[_x1, line_y]++;
                            break;
                        default:
                            box[line_x, line_y]++;
                            int _x2 = line_x - 1;
                            box[_x2, line_y]++;
                            break;
                    }
                    occupy(_turn, line_type, line_x, line_y);
                }
                if (line_type == vertice)
                {
                    v[line_x, line_y] = player;
                    switch (line_y)
                    {
                        case 0:
                            box[line_x, line_y]++;
                            break;
                        case 5:
                            int _y1 = line_y - 1;
                            box[line_x, _y1]++;
                            break;
                        default:
                            box[line_x, line_y]++;
                            int _y2 = line_y - 1;
                            box[line_x, _y2]++;
                            break;
                    }
                    occupy(_turn, line_type, line_x, line_y);
                }
            }
            turn = computer;

        }

        public void occupy(int _turn, int line_type, int line_x, int line_y)   //判断占格（手）
        {
            
            if(line_type==horizon)
            {
                switch(line_x)
                {
                    case 0:
                        if (box[line_x, line_y] == 4)
                        {
                            box_s[line_x, line_y] = _turn;                                            
                            box_no--;
                        }
                        break;
                    case 5:
                        if (box[line_x - 1, line_y] == 4)
                        {
                            box_s[line_x - 1, line_y] = _turn;
                            box_no--;
                        }
                        break;
                    default:
                        if (box[line_x, line_y] == 4)
                        {
                            box_s[line_x - 1, line_y] = _turn;
                            box_no--;
                        }
                        if (box[line_x - 1, line_y] == 4)
                        {
                            box_s[line_x - 1, line_y] = _turn;
                            box_no--;
                        }
                        break;
                }
            }
            if(line_type==vertice)
            {
                switch (line_y)
                {
                    case 0:
                        if (box[line_x, line_y] == 4)
                        {
                            box_s[line_x, line_y] = _turn;
                            box_no--;
                        }
                        break;
                    case 5:
                        if (box[line_x, line_y - 1] == 4)
                        {
                            box_s[line_x, line_y - 1] = _turn;
                            box_no--;
                        }
                        break;
                    default:
                        if (box[line_x, line_y] == 4)
                        {
                            box_s[line_x, line_y - 1] = _turn;
                            box_no--;
                        }
                        if (box[line_x, line_y - 1] == 4)
                        {
                            box_s[line_x, line_y - 1] = _turn;
                            box_no--;
                        }
                        break;
                }
            }
            int[] box_sc = new int[3];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if(box_s[i,j]==computer)
                    {
                        box_sc[computer]++;
                    }
                    if(box_s[i,j]==player)
                    {
                        box_sc[player]++;
                    }
                }
            }
            for (int i = 0; i < 5;i++ )
            {
                for (int j = 0; j < 5;j++ )
                {
                    if(box_s[i,j]==player)
                    {
                        int[] boxa = new int[2] { i, j };
                        string boxname = arraytobox(boxa);
                        foreach (Control control in this.Controls)
                        {
                            if (control.GetType() == typeof(Label))
                            {
                                Label lb = control as Label;
                                if (control.Name == boxname) //查找Name2的控件  
                                {

                                    lb.Text = "玩家";
                                    lb.BackColor = Color.Blue;
                                }
                            }
                        }
                    }
                    if (box_s[i, j] == computer)
                    {
                        int[] boxa = new int[2] { i, j };
                        string boxname = arraytobox(boxa);
                        foreach (Control control in this.Controls)
                        {
                            if (control.GetType() == typeof(Label))
                            {
                                Label lb = control as Label;
                                if (control.Name == boxname) //查找Name2的控件  
                                {

                                    lb.Text = "电脑";
                                    lb.BackColor = Color.Red;
                                }
                            }
                        }
                    }
                }
            }
            this.label_computer_sc.Text = box_sc[computer].ToString();
            this.label_player_sc.Text = box_sc[player].ToString();
        }
        public bool who_win(int[,] _box_statue)   //判断胜负（局面的格子数据，剩余格子数），0为未分胜负，1为玩家，2为电脑
        {
            if (box_no == 0)
            {
                int player_box = 0;
                int computer_box = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (_box_statue[i, j] == player)
                        {
                            player_box++;
                        }
                        else
                        {
                            computer_box++;
                        }
                    }
                }
                if (computer_box < player_box)
                {
                    this.label13.Text = "玩家胜利!";
                    this.label13.ForeColor = Color.Blue;
                }
                else
                {
                    this.label13.Text = "电脑胜利!";
                    this.label13.ForeColor = Color.Red;
                }
                return true;
            }
            return false;
        }
        private void button_undo_Click(object sender, EventArgs e)  //悔棋
        {
            string line_computer = this.listBox1.Items[this.listBox1.Items.Count - 1].ToString();
            string line_player = this.listBox1.Items[this.listBox1.Items.Count - 2].ToString();
            this.listBox1.Items.Remove(this.listBox1.Items[this.listBox1.Items.Count - 1]);
            this.listBox1.Items.Remove(this.listBox1.Items[this.listBox1.Items.Count - 2]);
            foreach (Control control in this.Controls)
            {
                if (control.GetType() == typeof(Button))
                {
                    Button bt = control as Button;
                    if (control.Name == line_computer) //查找Name2的控件  
                    {
                        bt.BackColor = Color.Black;
                        bt.Enabled = true;
                    }
                    if (control.Name == line_player) //查找Name2的控件  
                    {
                        bt.BackColor = Color.Black;
                        bt.Enabled = true;
                    }
                }
            }
            int[] com_step = nametoarray(line_computer);
            int[] pla_step = nametoarray(line_player);
            if(com_step[0]==horizon)
            {
                int x=com_step[1];
                int y=com_step[2];
                h[x, y] = 0;
                if(box[x,y-1]==4)
                {
                    box[x, y - 1]--;
                    box_s[x, y - 1] = noplayer;
                }
                if (box[x, y] == 4)
                {
                    box[x, y]--;
                    box_s[x, y] = noplayer;
                }
            }
            if (com_step[0] == vertice)
            {
                int x = com_step[1];
                int y = com_step[2];
                h[x, y] = 0;
                if (box[x - 1, y] == 4)
                {
                    box[x - 1, y]--;
                    box_s[x - 1, y] = noplayer;
                }
                if (box[x, y] == 4)
                {
                    box[x, y]--;
                    box_s[x, y] = noplayer;
                }
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label14_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public void com_first()
        {
            Search search = new Search();
            int[] com_array = search.computerstep(h, v);
            int type2 = com_array[0];
            int x2 = com_array[1];
            int y2 = com_array[2];
            step(computer, type2, x2, y2);
            string etype;
            if (type2 == horizon)
            {
                etype = "h";
            }
            else
            {
                etype = "v";
            }
            string name2 = etype + "_" + x2.ToString() + "_" + y2.ToString();
            foreach (Control control in this.Controls)
            {
                if (control.GetType() == typeof(Button))
                {
                    Button bt = control as Button;
                    if (control.Name == name2) //查找Name2的控件  
                    {
                        string step_c = name2 + "  电脑";
                        listBox1.Items.Add(step_c);
                        bt.BackColor = Color.Red;
                        bt.Enabled = false;
                    }
                }
            }
            savegame();
        }

        private void 玩家先手ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameForm1 Formnew = new GameForm1();
            Formnew.Show();
        }

        public void savegame()     //保存下步
        {
            using (StreamWriter sw = File.CreateText(datefile))
            {
                sw.WriteLine(date);
                sw.WriteLine("-----------------------------------------------");
                for (int i = 0; i < this.listBox1.Items.Count; i++)
                {
                    sw.WriteLine(this.listBox1.Items[i]);
                }
            }
        }
        public void loadgame()
        {

        }
        private void button_load_Click(object sender, EventArgs e)
        {
            
            
        } 
        public int[] nametoarray(string _name)       //按键名转数组
        {
            string srcString = _name;
            int[] array = new int[3];
            string regexStr1 = @"^h_\d{1}_\d{1}$";
            Regex r1 = new Regex(regexStr1);
            Match m = r1.Match(srcString);
            if (m.Success)
            {
                array[0] = 0;
            }
            else
            {
                array[0] = 1;
            }
            Regex myregexX = new Regex("(h|v)_([^\"]*)_([^\"]*)");
            array[1] = int.Parse(myregexX.Match(srcString).Groups[2].Value);
            array[2] = int.Parse(myregexX.Match(srcString).Groups[3].Value);
            return array;
        }
        public string arraytoname(int[] array)     //数组转按键名
        {
            string type;
            switch (array[0])
            {
                case 0:
                    type = "h_"; break;
                default:
                    type = "v_"; break;
            }
            string name = type + array[1].ToString() + "_" + array[2].ToString();
            return name;
        }
        public string arraytobox(int[] _array)         //数组转格子
        {
            string name = "box"+_array[0].ToString() + "_" + _array[1].ToString();
            return name;
        }

        private void 退出游戏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            datefile = DateTime.Now.ToFileTime().ToString()+".sav";
            date = DateTime.Now.ToString();
        }

        private void 重新开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
            LoadForm renew = new LoadForm();
            renew.Show();
        }

        private void GameForm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            second_com++;
            if(second_com==60)
            {
                minute_com++;
                second_com = 0;
            }
            this.label6.Text = second_com.ToString();
            this.label9.Text = minute_com.ToString();
        }

        private void timer_pla_Tick(object sender, EventArgs e)
        {
            second_pla++;
            if (second_pla == 60)
            {
                minute_pla++;
                second_pla = 0;
            }
            this.label5.Text = second_pla.ToString();
            this.label2.Text = minute_pla.ToString();
        }
        
    }
}
