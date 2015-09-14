using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Dot_Box_Platform
{
    public class CPPDLL
    {
        [DllImport("Dot_Box_DLL.dll", EntryPoint = "Getmove",CallingConvention =CallingConvention.Cdecl)]
        public static extern void Getmove (int[,] input,int[] output);
    }
    public class nextmove
    {
        /// <summary>
        /// 相关参数
        /// </summary>
        private static int[] returnmove = new int[3];  //返回的招法
        private static int[,] state = new int[11, 11];  //生成的分析用数组
        //private static int[][] state2 = new int[11][];
        /// <summary>
        /// 类的实例化
        /// </summary>
        public nextmove(int[,] _h, int[,] _v, int[,] _boxedg, int step)
        {
            state = sta_tran(_h, _v, _boxedg);
        }
        /// <summary>
        /// 获取下法
        /// </summary>
        public int[] get()  //
        {
            Thread trd = new Thread(startmove);
            trd.Start();
            bool isalive = false;
            do
            {
                isalive = trd.IsAlive;
            }
            while (isalive);
            return returnmove;
        }
        /// <summary>
        /// 相关方法
        /// </summary>
        private static int[,] sta_tran(int[,] _h, int[,] _v, int[,] _box_ed)  //转换为分析用数组
        {
            int[,] sta_4_analy = new int[11, 11];
            int i, j, x, y;
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    x = 2 * i;
                    y = 2 * j + 1;
                    sta_4_analy[x, y] = _h[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    x = 2 * i + 1;
                    y = 2 * j;
                    sta_4_analy[x, y] = _v[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    x = 2 * i + 1;
                    y = 2 * j + 1;
                    sta_4_analy[x, y] = _box_ed[i, j];
                }
            }      
            return sta_4_analy;
        }
        private void startmove()
        {
            CPPDLL.Getmove(state, returnmove);
        }

    }
}
