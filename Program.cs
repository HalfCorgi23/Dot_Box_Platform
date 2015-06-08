using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dot_Box_Killer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [MTAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoadForm());      
        }
    }
    public class data     //局面数据建立
    {
        int player = 1;         //玩家  
        int computer = 2;     //电脑
        int noplayer = 0;      //未有玩家占     
        //******************************************************************//
        int[,] h = new int[6, 5];
        int[,] v = new int[5, 6];
        public int horizon = 0;
        public int vertice = 1;
        //******************************************************************//
        public int[,] box = new int[5, 5];      //格子被占边数
        int[,] box_s = new int[5, 5];     //格子被占状态
        //int box_no = 25;    //剩余未被占格子数
        public data(int[,] _h, int[,] _v)
        {
            h = _h;
            v = _v;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (h[i, j] != 0)
                    {
                        box[i, j]++;
                    }
                    if (h[i + 1, j] != 0)
                    {
                        box[i, j]++;
                    }
                    if (v[i, j] != 0)
                    {
                        box[i, j]++;
                    }
                    if (v[i, j + 1] != 0)
                    {
                        box[i, j]++;
                    }
                }
            }
        }
        public bool edge3up(int _x, int _y)
        {
            int count = 0;
            if (h[_x, _y] == noplayer)
            {
                count++;
            }
            if (h[_x, _y + 1] == noplayer)
            {
                count++;
            }
            if (v[_x, _y] == noplayer)
            {
                count++;
            }
            if (v[_x + 1, _y] == noplayer)
            {
                count++;
            }
            if (count > 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }     //判断格子被占边数是否大于等于3（格子横坐标，格子纵坐标）
    }
}
