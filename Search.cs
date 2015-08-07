using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Dot_Box_Platform
{
    class movestep
    {
        public static int no_p = 0;
        public static int com = 1;   //电脑为1
        public static int pla = 2;   //玩家为2
        public static int horizon = 0;
        public static int vertice = 1;
        /****************************/
        static int[,] h = new int[6, 5];
        static int[,] v = new int[5, 6];
        static int[,] boxedg = new int[5, 5];
        static int[] returnmove = new int[3];
        static int stepno = 0;
        public movestep(int[,] _h, int[,] _v, int[,] _boxedg, int step)
        {
            h = _h;
            v = _v;
            boxedg = _boxedg;
            stepno++;
        }
        public int[] getmove()  //获取下法
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

        /**************************以上程序不要改动*************************************************/
        static void startmove()
        {
            int[,] _h = h;
            int[,] _v = v;
            int[,] _boxedg = boxedg;
            int[] rtn = new int[3];
            /************************/
            //int[] druge=new int[3]{0,0,0};
            //int[] testrtn = rdmove2(_h, _v, _boxedg);
            //bool a = (testrtn[0] == 0);
            //bool b = (testrtn[1] == 0);
            //bool c = (testrtn[2] == 0);
            //bool d = (boxedg[0, 0] <= 2);            
            //if (a && b && c)
            //{
            // occup_mstat2(_h, _v, _boxedg);
            //}

            /*********************/
            //else
            //{ 
            if (occup_boxno(h, v, boxedg) != 0)
            {
                int[] occu_step = occup_mstat2(_h, _v, _boxedg);
                rtn[0] = occu_step[0];
                rtn[1] = occu_step[1];
                rtn[2] = occu_step[2];
            }
            else
            {
                int connum = 2000;
                int[,] testarr = new int[connum, 3];
                int[] arra = new int[3];
                for (int i = 0; i < connum; i++)
                {
                    arra = rdmove2(_h, _v, _boxedg);
                    testarr[i, 0] = arra[0];
                    testarr[i, 1] = arra[1];
                    testarr[i, 2] = arra[2];
                }
                bool same = false;
                for (int i = 0; i < connum - 1; i++)
                {
                    bool same1;
                    same1 = (testarr[i, 0] != testarr[i + 1, 0]);
                    if (same1)
                    {
                        same = false;
                    }
                    same1 = (testarr[i, 1] != testarr[i + 1, 1]);
                    if (same1)
                    {
                        same = false;
                    }
                    same1 = (testarr[i, 2] != testarr[i + 1, 2]);
                    if (same1)
                    {
                        same = false;
                    }
                }
                if (same && (arra[0] != 0) && (arra[1] != 0) && (arra[2] != 0))
                {
                    rtn = rdmove2(h, v, boxedg);
                }
                else if (same && (arra[0] == 0) && (arra[1] == 0) && (arra[2] == 0) && _h[0, 0] == 0)
                {
                    rtn[0] = 0;
                    rtn[1] = 0;
                    rtn[2] = 0;
                }
                else
                {
                    if (stepno < 10)
                    {
                        rtn = rdmove2(_h, _v, _boxedg);
                    }
                    else
                    {
                        rtn = stepno_move(_h, _v, _boxedg, 300000);//改变样本量
                    }
                }
            }
            returnmove[0] = rtn[0];
            returnmove[1] = rtn[1];
            returnmove[2] = rtn[2];
        }
        //改变参数
        static int[] rdmove(int[,] _h, int[,] _v, int[,] _boxedg)
        {
            int[,] __h = new int[6, 5];
            int[,] __v = new int[5, 6];
            int[,] __boxedg = new int[5, 5];
            int i, j;
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    __h[i, j] = _h[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    __v[i, j] = _v[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    __boxedg[i, j] = _boxedg[i, j];
                }
            }
            int count = 0;
            int[,] list = new int[60, 3];
            int[] rtn = new int[3];
            Random rd = new Random();
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    if (__h[i, j] == 0)
                    {
                        list[count, 0] = 0;
                        list[count, 1] = i;
                        list[count, 2] = j;
                        count++;
                    }
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    if (__v[i, j] == 0)
                    {
                        list[count, 0] = 1;
                        list[count, 1] = i;
                        list[count, 2] = j;
                        count++;
                    }
                }
            }
            int num = rd.Next(count);
            rtn[0] = list[num, 0];
            rtn[1] = list[num, 1];
            rtn[2] = list[num, 2];
            return rtn;
        }  //全部随机步
        static int[] rdmove2(int[,] __h, int[,] __v, int[,] __boxedg)
        {
            int[,] _h = new int[6, 5];
            int[,] _v = new int[5, 6];
            int[,] _boxedg = new int[5, 5];
            _h = __h;
            _v = __v;
            _boxedg = __boxedg;
            int i;
            int j;
            int count = 0;
            int[,] list = new int[60, 3];
            int[] rtn = new int[3];
            Random rd = new Random();
            for (j = 0; j < 5; j++)
            {
                if (_h[0, j] == 0 && _boxedg[0, j] < 2)
                {
                    list[count, 0] = 0;
                    list[count, 1] = 0;
                    list[count, 2] = j;
                    count++;
                }

                for (i = 1; i < 5; i++)
                {
                    if (_h[i, j] == 0 && _boxedg[i, j] < 2 && _boxedg[i - 1, j] < 2)
                    {
                        list[count, 0] = 0;
                        list[count, 1] = i;
                        list[count, 2] = j;
                        count++;
                    }
                }
                if (_h[5, j] == 0 && _boxedg[4, j] < 2)
                {
                    list[count, 0] = 0;
                    list[count, 1] = 5;
                    list[count, 2] = j;
                    count++;
                }
            }
            for (i = 0; i < 5; i++)
            {
                if (_v[i, 0] == 0 && _boxedg[i, 0] < 2)
                {
                    list[count, 0] = 1;
                    list[count, 1] = i;
                    list[count, 2] = 0;
                    count++;
                }

                for (j = 1; j < 5; j++)
                {
                    if (_v[i, j] == 0 && _boxedg[i, j] < 2 && _boxedg[i, j - 1] < 2)
                    {
                        list[count, 0] = 1;
                        list[count, 1] = i;
                        list[count, 2] = j;
                        count++;
                    }
                }
                if (_v[i, 5] == 0 && _boxedg[i, 4] < 2)
                {
                    list[count, 0] = 1;
                    list[count, 1] = i;
                    list[count, 2] = 5;
                    count++;
                }
            }
            int getcou = rd.Next(count);
            rtn[0] = list[getcou, 0];
            rtn[1] = list[getcou, 1];
            rtn[2] = list[getcou, 2];

            return rtn;
        }   //获取不让格的随机步

        public static int[] stepno_move(int[,] _h, int[,] _v, int[,] _boxedg, int sampleno)
        {
            int[,] __h = new int[6, 5];
            int[,] __v = new int[5, 6];
            int[,] __boxedg = new int[5, 5];
            int i, j;
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    __h[i, j] = _h[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    __v[i, j] = _v[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    __boxedg[i, j] = _boxedg[i, j];
                }
            }
            int stepnum;
            int[,] h_num = new int[6, 5];
            int[,] v_num = new int[5, 6];
            int times = 0;
            int[,] situation = new int[6, 11];
            for (int k = 0; k < sampleno; k++)
            {
                times = 0;
                do
                {
                    situation = makestat2(__h, __v, __boxedg);
                    stepnum = 0;
                    for (i = 0; i < 6; i++)
                    {
                        for (j = 1; j < 5; j++)
                        {
                            if (situation[i, j] == 3)
                            {
                                stepnum++;
                            }
                        }
                    }
                    for (i = 0; i < 5; i++)
                    {
                        for (j = 5; j < 11; j++)
                        {
                            if (situation[i, j] == 3)
                            {
                                stepnum++;
                            }
                        }
                    }
                    times++;
                    if (times > 2000000)
                    {
                        //Console.WriteLine("循环过多");
                        //Console.ReadLine();
                        int[] getarr = rdmove2(_h, _v, _boxedg);
                        if (getarr[0] == 0 && getarr[1] == 0 && getarr[2] == 0)
                        {
                            if (_h[0, 0] == 0)
                            {
                                return getarr;
                            }
                            else
                            {
                                return rdmove(h, v, boxedg);
                            }
                        }
                        else
                        {
                            return rdmove2(_h, _v, _boxedg);
                        }
                    }
                }
                while (stepnum % 2 == 0);
                //int[,] situation2 = situation;
                for (i = 0; i < 6; i++)
                {
                    for (j = 1; j < 5; j++)
                    {
                        if (situation[i, j] == 3)
                        {
                            h_num[i, j]++;
                        }
                    }
                }
                for (i = 0; i < 5; i++)
                {
                    for (j = 5; j < 11; j++)
                    {
                        if (situation[i, j] == 3)
                        {
                            v_num[i, j - 5]++;
                        }
                    }
                }
            }
            int[] rtn = new int[3];
            rtn[0] = 0;
            rtn[1] = 0;
            rtn[2] = 0;
            int step_r = h_num[0, 0];
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    if (h_num[i, j] > step_r)
                    {
                        rtn[0] = 0;
                        rtn[1] = i;
                        rtn[2] = j;
                        step_r = h_num[i, j];
                    }
                    if (j == 4 && i < 5)
                    {
                        if (h_num[i + 1, 0] > step_r)
                        {
                            rtn[0] = 0;
                            rtn[1] = i + 1;
                            rtn[2] = 0;
                            step_r = h_num[i + 1, 0];
                        }
                    }
                }

            }
            if (v_num[0, 0] > step_r)
            {
                rtn[0] = 1;
                rtn[1] = 0;
                rtn[2] = 0;
                step_r = v_num[0, 0];
            }

            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    if (v_num[i, j] > step_r)
                    {
                        rtn[0] = 1;
                        rtn[1] = i;
                        rtn[2] = j;
                        step_r = v_num[i, j];
                    }
                    if (j == 5 && i < 4)
                    {
                        if (v_num[i + 1, 0] > step_r)
                        {
                            rtn[0] = 1;
                            rtn[1] = i + 1;
                            rtn[2] = 0;
                            step_r = v_num[i + 1, 0];
                        }
                    }
                }

            }
            if (times >= 1000000)
            {

            }
            return rtn;
        }  //按步数获取下步
        static int[,] makestat2(int[,] _h, int[,] _v, int[,] _boxedg)
        {
            int[,] h_m = new int[6, 5];
            int[,] v_m = new int[5, 6];
            int[,] boxed_m = new int[5, 5];
            int i, j;
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    h_m[i, j] = _h[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    v_m[i, j] = _v[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    boxed_m[i, j] = _boxedg[i, j];
                }
            }
            bool tobeend = true;
            do
            {
                int[] gemove = rdmove2(h_m, v_m, boxed_m);
                int type = gemove[0];
                int x = gemove[1];
                int y = gemove[2];
                //int times = 0;
                if (type == horizon)
                {
                    h_m[x, y] = 3;
                }
                else if (type == vertice)
                {
                    v_m[x, y] = 3;
                }
                if (type == horizon)
                {
                    switch (x)
                    {
                        case 0:
                            boxed_m[x, y]++;
                            break;
                        case 5:
                            boxed_m[x - 1, y]++;
                            break;
                        default:
                            boxed_m[x, y]++;
                            boxed_m[x - 1, y]++;
                            break;
                    }
                }
                if (type == vertice)
                {
                    switch (y)
                    {
                        case 0:
                            boxed_m[x, y]++;
                            break;
                        case 5:
                            boxed_m[x, y - 1]++;
                            break;
                        default:
                            boxed_m[x, y]++;
                            boxed_m[x, y - 1]++;
                            break;
                    }
                }
                if (x == 0 || y == 0)
                {
                    if (h_m[0, 0] == 0 && v_m[0, 0] == 0)
                    {
                        tobeend = true;
                    }
                    else
                    {
                        tobeend = false;
                    }
                }
            }
            while (tobeend);
            int[,] rtn = new int[6, 11];
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    rtn[i, j] = h_m[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    rtn[i, j + 5] = v_m[i, j];
                }
            }
            return rtn;
        }//建立1个局面  
        static int[] occup_mstat(int[,] _h, int[,] _v, int[,] _boxedg)
        {
            int[] rtn = new int[3];
            int[,] h_m = new int[6, 5];
            int[,] v_m = new int[5, 6];
            int[,] boxed_m = new int[5, 5];
            int i, j;
            int getscore = 0;
            int lostscore = 0;
            int _turn = 0;
            int rema_step = 0;
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    h_m[i, j] = _h[i, j];
                    if (_h[i, j] == 0)
                    {
                        rema_step++;
                    }
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    v_m[i, j] = _v[i, j];
                    if (_v[i, j] == 0)
                    {
                        rema_step++;
                    }
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    boxed_m[i, j] = _boxedg[i, j];
                }
            }
            bool tobeend = true;
            int count1 = 1;
            do
            {
                int[] gemove = rdmove(h_m, v_m, boxed_m);
                int type = gemove[0];
                int x = gemove[1];
                int y = gemove[2];
                if (count1 == 1)
                {
                    count1 = 0;
                    rtn[0] = type;
                    rtn[1] = x;
                    rtn[2] = y;
                }
                if (type == horizon)
                {
                    h_m[x, y] = 3;
                }
                else if (type == vertice)
                {
                    v_m[x, y] = 3;
                }
                if (type == horizon)
                {
                    switch (x)
                    {
                        case 0:
                            boxed_m[x, y]++;
                            boxed_m[x, y]++;
                            break;
                        case 5:
                            boxed_m[x - 1, y]++;
                            boxed_m[x - 1, y]++;
                            break;
                        default:
                            boxed_m[x, y]++;
                            boxed_m[x - 1, y]++;
                            boxed_m[x, y]++;
                            boxed_m[x - 1, y]++;
                            break;
                    }
                }
                if (type == vertice)
                {
                    switch (y)
                    {
                        case 0:
                            boxed_m[x, y]++;
                            boxed_m[x, y]++;
                            break;
                        case 5:
                            boxed_m[x, y - 1]++;
                            boxed_m[x, y - 1]++;
                            break;
                        default:
                            boxed_m[x, y]++;
                            boxed_m[x, y - 1]++;
                            boxed_m[x, y]++;
                            boxed_m[x, y - 1]++;
                            break;
                    }
                }
                if (x == 0 && y == 0)
                {
                    if (h_m[0, 0] == 0 && v_m[0, 0] == 0)
                    {
                        tobeend = true;
                    }
                    else
                    {
                        tobeend = false;
                    }
                }
                for (i = 0; i < 5; i++)
                {
                    for (j = 0; j < 5; j++)
                    {
                        if (boxed_m[i, j] == 5 && _turn == 0)
                        {
                            getscore++;
                            boxed_m[i, j] = 9;
                        }
                        else if (boxed_m[i, j] == 5 && _turn == 1)
                        {
                            lostscore++;
                            boxed_m[i, j] = 9;
                        }
                        else
                        {
                            if (_turn == 0)
                            {
                                _turn = 1;
                            }
                            if (_turn == 1)
                            {
                                _turn = 0;
                            }
                        }
                    }
                }
            }
            while (tobeend);
            if (getscore > lostscore)
            {
                return rtn;
            }
            else
            {
                return occup_mstat(_h, _v, _boxedg);
            }
        }  //获取占格
        static int[] occup_mstat2(int[,] _h, int[,] _v, int[,] _boxedg)
        {
            int[] rtn = new int[3];
            int[,] h_m = new int[6, 5];
            int[,] v_m = new int[5, 6];
            int[,] boxed_m = new int[5, 5];
            int i, j;
            int box_num = 0;
            int[] retn = new int[2];
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    h_m[i, j] = _h[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    v_m[i, j] = _v[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    boxed_m[i, j] = _boxedg[i, j];
                    if (boxed_m[i, j] == 3)
                    {
                        retn[0] = i;
                        retn[1] = j;
                        box_num++;
                    }
                }
            }
            if (box_num >= 1)
            {
                if (h_m[retn[0], retn[1]] == 0)
                {
                    rtn[0] = 0;
                    rtn[1] = retn[0];
                    rtn[2] = retn[1];
                }
                if (h_m[retn[0] + 1, retn[1]] == 0)
                {
                    rtn[0] = 0;
                    rtn[1] = retn[0] + 1;
                    rtn[2] = retn[1];
                }
                if (v_m[retn[0], retn[1]] == 0)
                {
                    rtn[0] = 1;
                    rtn[1] = retn[0];
                    rtn[2] = retn[1];
                }
                if (v_m[retn[0], retn[1] + 1] == 0)
                {
                    rtn[0] = 1;
                    rtn[1] = retn[0];
                    rtn[2] = retn[1] + 1;
                }
            }
            else
            {
                rtn = rdmove(h_m, v_m, boxed_m);
            }
            return rtn;

        }
        static int occup_boxno(int[,] _h, int[,] _v, int[,] _boxedg)
        {
            int[] rtn = new int[3];
            int[,] h_m = new int[6, 5];
            int[,] v_m = new int[5, 6];
            int[,] boxed_m = new int[5, 5];
            int i, j;
            int box_num = 0;
            int[] retn = new int[2];
            for (i = 0; i < 6; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    h_m[i, j] = _h[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    v_m[i, j] = _v[i, j];
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    boxed_m[i, j] = _boxedg[i, j];
                    if (boxed_m[i, j] == 3)
                    {
                        retn[0] = i;
                        retn[1] = j;
                        box_num++;
                    }
                }
            }
            return box_num;
        }  //获取可占格数目，0，1，2

        /********************************************************************************************/
        static int[,] makestat(int[,] _h, int[,] _v, int[,] _boxedg)
        {
            int[,] h_m = _h;
            int[,] v_m = _v;
            int[,] boxed_m = _boxedg;
            bool tobeend = true;
            do
            {
                int[] gemove = rdmove2(h_m, v_m, boxed_m);
                int type = gemove[0];
                int x = gemove[1];
                int y = gemove[2];
                if (type == horizon)
                {
                    h_m[x, y] = 3;
                }
                else if (type == vertice)
                {
                    v_m[x, y] = 3;
                }
                if (type == horizon)
                {
                    switch (x)
                    {
                        case 0:
                            boxed_m[x, y]++;
                            break;
                        case 5:
                            boxed_m[x - 1, y]++;
                            break;
                        default:
                            boxed_m[x, y]++;
                            boxed_m[x - 1, y]++;
                            break;
                    }
                }
                if (type == vertice)
                {
                    switch (y)
                    {
                        case 0:
                            boxed_m[x, y]++;
                            break;
                        case 5:
                            boxed_m[x, y - 1]++;
                            break;
                        default:
                            boxed_m[x, y]++;
                            boxed_m[x, y - 1]++;
                            break;
                    }
                }
                //gemove = rdmove2(h_m, v_m, boxed_m);
                //type = gemove[0];
                //x = gemove[1];
                //y = gemove[2];
                if (x == 0 || y == 0)
                {
                    if (h_m[0, 0] == 0 && v_m[0, 0] == 0)
                    {
                        tobeend = true;
                    }
                    else
                    {
                        tobeend = false;
                    }
                }
            }
            while (tobeend);
            int[,] rtn = new int[11, 11];
            for (int i = 0; i < 5; i++)///////
            {
                for (int j = 0; j < 4; j++)////
                {
                    rtn[(i + 1) * 2, j * 2] = h_m[i, j];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    rtn[i * 2, (j + 1) * 2] = v_m[i, j];
                }
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    rtn[i * 2 + 1, j * 2 + 1] = boxed_m[i, j];
                }
            }
            return rtn;
        }   //建立1个局面
        public static int value(int[,] _situation)
        {
            int[,] chain = situa_analy(_situation);
            int stepnum = chain[0, 0];
            int chainnum = chain[0, 1];
            int longchainnum = chainnum;
            int shortchainnum = 0;
            for (int i = 1; i < chainnum; i++)
            {
                if (chain[i, 1] < 4)
                {
                    shortchainnum++;
                    longchainnum--;
                }
            }
            int sc = 30;
            if (stepnum % 2 == 1)
            {
                sc = sc + 40;
                if (chainnum < 7)
                {
                    sc = sc + 20;
                    if (shortchainnum < 3)
                    {
                        sc = sc + 10;
                    }
                    sc = sc - shortchainnum;
                }
                sc = sc - 2 * chainnum;
            }
            if (stepnum % 2 == 0)
            {
                if (chainnum > 7)
                {
                    sc = sc + 20;
                    if (shortchainnum > 4)
                    {
                        sc = sc + 10;
                    }
                    sc = sc + shortchainnum;
                }
                sc = sc + 2 * chainnum;
            }
            return sc;
        }
        public static int[,] situa_analy(int[,] _situation)
        {
            int[,] box = new int[5, 5];
            int longchainnum = 0;
            int[,] longchain = new int[20, 2]; //类型，长度，首行为步数，长链数
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (_situation[2 * i + 1, 2 * j + 1] == 0)
                    {
                        longchainnum = longchainnum + 3;
                        if (_situation[2 * i + 1, 2 * j + 1] == 1)
                        {
                            longchainnum = longchainnum + 1;
                        }
                    }
                }
            }
            return longchain;
        }
        public static int[,] longchain_anal(int[,] _situation, int x, int y, int longchainnum, bool _start, int[,] longchain)
        {

            if (_situation[x, y] == 0)
            {
                _start = false;
                longchainnum++;
                _situation[x, y] = 3;
                longchain[longchainnum, 1]++;
                longchain_anal(_situation, x - 2, y, longchainnum, false, longchain);
                longchain[longchainnum, 1]++;
                longchain_anal(_situation, x, y - 2, longchainnum, false, longchain);
                longchain[longchainnum, 1]++;
                longchain_anal(_situation, x, y - 2, longchainnum, false, longchain);
                longchain_anal(_situation, x + 2, y, longchainnum, false, longchain);
            }
            //十字双交
            if (_situation[x, y] == 1)
            {
                if (_start == true)
                {
                    longchainnum = longchainnum + 2;
                }
                else
                {
                    longchainnum = longchainnum + 1;
                    longchain[longchainnum, 1]++;
                    longchain_anal(_situation, x - 2, y, longchainnum, false, longchain);
                }
            }
            //T字双交
            if (_situation[x, y] == 2)
            {
                if (x - 1 == 0 && y - 1 == 0)
                {
                    if (_situation[x + 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x + 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x, y + 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y + 2, longchainnum, false, longchain);
                    }
                }
                if (x + 1 == 10 && y + 1 == 10)
                {
                    if (_situation[x - 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x - 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x, y - 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y - 2, longchainnum, false, longchain);
                    }
                }
                if (x + 1 == 10 && y - 1 == 0)
                {
                    if (_situation[x + 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x + 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x, y + 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y + 2, longchainnum, false, longchain);
                    }
                }
                if (x - 1 == 0 && y + 1 == 10)
                {
                    if (_situation[x - 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x - 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x, y + 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y + 2, longchainnum, false, longchain);
                    }
                }
                if (x - 1 == 0 && y + 1 != 10 && y - 1 != 0)
                {
                    if (_situation[x + 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x + 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x, y + 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y + 2, longchainnum, false, longchain);
                    }
                    if (_situation[x, y - 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y - 2, longchainnum, false, longchain);
                    }
                }
                if (y - 1 == 0 && x + 1 != 10 && x - 1 != 0)
                {
                    if (_situation[x + 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x + 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x - 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x - 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x, y + 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y + 2, longchainnum, false, longchain);
                    }
                }
                if (y + 1 == 10 && x + 1 != 10 && x - 1 != 0)
                {
                    if (_situation[x + 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x + 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x - 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x - 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x, y - 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y - 2, longchainnum, false, longchain);
                    }
                }
                if (x + 1 == 10 && y + 1 != 10 && y - 1 != 0)
                {
                    if (_situation[x - 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x - 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x, y + 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y + 2, longchainnum, false, longchain);
                    }
                    if (_situation[x, y - 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y - 2, longchainnum, false, longchain);
                    }
                }
                else
                {
                    if (_situation[x - 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x - 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x + 1, y] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x + 2, y, longchainnum, false, longchain);
                    }
                    if (_situation[x, y + 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y + 2, longchainnum, false, longchain);
                    }
                    if (_situation[x, y - 1] == 0)
                    {
                        longchain[longchainnum, 1]++;
                        _situation[x, y] = 3;
                        longchain_anal(_situation, x, y - 2, longchainnum, false, longchain);
                    }
                }

            }//链
            if (_situation[x, y] == 3)
            {

            }
            int[,] rtn = new int[20, 2];
            return rtn;
        }

    }
}
