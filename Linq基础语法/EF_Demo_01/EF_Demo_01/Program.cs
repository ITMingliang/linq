using System;
using System.Collections.Generic;
using System.Linq;

namespace EF_Demo_01
{
    public class Program
    {
        public static bool IsEven(int i)
        {
            return i%2==0?true:false;
        }
        
        static void Main(string[] args)
        {
            int[] arr1 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] arr2 = new int[] { 1, 3, 5, 7, 9 };
            int[] arr3 = new int[] { 2, 4, 6, 8, 10};

            #region 
            //from + select
            Console.WriteLine("====================from + select======================");
            var query1 = from n in arr1 
                         select n;

            Console.WriteLine(query1.GetType());

            //query1和query2类型一致
            IEnumerable<int> query2 = from n in arr1
                         select n;
            Console.WriteLine(query2.GetType());

            foreach (var item in query1)
            {
                Console.WriteLine(item);
            }

            //where
            Console.WriteLine("====================where======================");
            var query3 = from n in arr1
                         where n>8 || n<3
                         select n;

            foreach (var item in query3)
            {
                Console.WriteLine(item);
            }

            //运算
            Console.WriteLine("======================加法运算====================");
            //循环累加
            var query4 = from a in arr2
                         from b in arr3
                         select a + b;
            foreach (var item in query4)
            {
                Console.WriteLine(item);
            }

            //乘法
            Console.WriteLine("=====================乘法运算====================");
            var query5 = from a in arr3
                         select a * 10;
            foreach (var item in query5)
            {
                Console.WriteLine(item);
            }

            //匿名对象
            Console.WriteLine("=====================匿名对象====================");
            var query6 = from a in arr3
                         select new  //匿名对象初始化
                         {
                             id=a,
                             name =a.ToString()
                         };
            foreach (var item in query6)
            {
                Console.WriteLine(item);
            }

            //奇数偶数判断
            Console.WriteLine("=====================奇数偶数====================");
            var query7 = from a in arr1
                         where IsEven(a)//调用判断
                         select a;

            var query8 = from a in arr1
                         let n = a%2  //判断临时变量，let 存放临时变量
                         where n==0
                         select a;

            foreach (var item in query7)
            {
                Console.WriteLine(item);
            }
            //排序
            Console.WriteLine("=====================排序====================");
            int [] arr4=new int[] {1,4,9,2,5,6,8,7,3};  
            var query9 =from a in arr4 
                       orderby a ascending  //descending
                       select a;

            foreach (var item in query9)
            {
                Console.WriteLine(item);
            }

            //group by
            Console.WriteLine("=====================group by====================");
            var query10_1=from a in arr1
                        group a by a%2 into g //临时标识符
                        from g2 in g
                        select g2;

            foreach (var item in query10_1)
            {
                Console.WriteLine(item);
            }

            string[] language = { "JAVA", "C", "C++", "Python", "Go", "VC", "VB","Perl"};

            var query10_2 = from a in language
                            group a by a.Length into lengthGroups
                            orderby lengthGroups.Key
                            select lengthGroups;

            foreach (var item in query10_2)
            {
                Console.WriteLine("string of the length:{0} ",item.Key);
                foreach (var str in item)
                {
                    Console.WriteLine(str);
                }
            }

            //join on
            Console.WriteLine("=====================join on ====================");
            var query11= from a in arr1
                         where a<9
                         join b in arr2 on  a equals b
                         select a;

            foreach (var item in query11)
            {
                Console.WriteLine(item);
            }
            #endregion
            Console.ReadKey();    
        }
    }
}
