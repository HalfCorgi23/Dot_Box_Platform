using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
                             
						      _ooOoo_
                             o8888888o
                             88" . "88
                             (| -_- |)
                             O\  =  /O
                          ____/`---'\____
                        .'  \\|     |//  `.
                       /  \\|||  :  |||//  \
                      /  _||||| -:- |||||-  \
                      |   | \\\  -  /// |   |
                      | \_|  ''\---/''  |   |
                      \  .-\__  `-`  ___/-. /
                    ___`. .'  /--.--\  `. . __
                 ."" '<  `.___\_<|>_/___.'  >'"".
                | | :  `- \`.;`\ _ /`;.`/ - ` : | |
                \  \ `-.   \_ __\ /__ _/   .-` /  /
           ======`-.____`-.___\_____/___.-`____.-'======
                              `=---='
           ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                       ********************
					   佛祖保佑       永无BUG
					 ************************
					  本注释已经开光处理，修改无效
					 ************************
					   佛法无边       普度众生
					   ********************

*/

namespace Dot_Box_Killer
{
    public class Search
    {
        public int[] computerstep(int[,] h,int[,] v)   //通过算法获取下步
        {
            int[] array = randstep3(h, v);
            return array;
        }
        public int[] randstep3(int[,] h, int[,] v)      //找被占边数大于2的格子的随机下发
        {
            Random rd = new Random();
            bool judge;
            bool judge2;
            int box_x;
            int box_y;
            int e_type;
            int e_x;
            int e_y;
            data game = new data(h, v);
            do
            {
                box_x = rd.Next(4);
                box_y = rd.Next(4);
                if(game.box[box_x,box_y]>0)
                {
                    judge = true;
                }
                else
                {
                    judge = false;
                }
            } while (judge);
            int[] rtn = new int[3];
            do
            {
                int type = rd.Next(4);
                switch(type)
                {
                    case 0:
                        judge2 = (h[box_x, box_y] != 0);
                        e_type = game.horizon;
                        e_x = box_x;
                        e_y = box_y;
                        break;
                    case 1:
                        judge2 = (h[box_x, box_y + 1] != 0);
                        e_type = game.horizon;
                        e_x = box_x;
                        e_y = box_y+1;
                        break;
                    case 2:
                        judge2 = (v[box_x, box_y] != 0);
                        e_type = game.vertice;
                        e_x = box_x;
                        e_y = box_y;
                        break;
                    default:
                        judge2 = (v[box_x + 1, box_y] != 0);
                        e_type = game.vertice;
                        e_x = box_x + 1;
                        e_y = box_y;
                        break;
                }             
            }while(judge2);
            rtn[0] = e_type;
            rtn[1] = e_x;
            rtn[2] = e_y;
            return rtn;
        }
    }
    
}
