using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<char> chars = new List<char>();//abcdefg
            // 小写26字母
            for (int i = 97; i < 123; i++) { chars.Add((char)i); }

            List<string> three = new List<string>();// 三位
            List<string> ten = new List<string>();// 十位
            for (int iin = 0; iin < chars.Count; iin++)
            {
                int a = chars[iin], b, c, nx = 1;
                int ia = 0, ib = 0;
                bool iscontinue = true;
                do
                {
                    ia = iin + nx;
                    ib = iin + (nx * 2);
                    if (ib >= chars.Count)
                    {
                        iscontinue = false;
                        break;
                    }
                    b = chars[ia];
                    c = chars[ib];
                    if ((a - b) == (b - c))
                    {
                        three.Add(((char)a).ToString() + ((char)b).ToString() + ((char)c).ToString());
                        nx++;
                    }
                } while (iscontinue);

            }
            Console.WriteLine("Three Number: {0}",three.Count);
            three.ForEach(x => Console.Write(x + "\t"));
            Console.WriteLine("\nFour Number: {0}", ten.Count);
            ten.ForEach(x => Console.Write(x + ","));

            //int maxy = 10001, maxx = 1000001, count = 0;
            //for (int i = 1; i < maxy; i++)
            //{
            //    double x = Math.Pow(i, 1.5);
            //    if (x % 1 == 0 && x < maxx)
            //        count++;
            //}
            //Console.Write(count);
            
            Console.Read();
        }
    }
}
