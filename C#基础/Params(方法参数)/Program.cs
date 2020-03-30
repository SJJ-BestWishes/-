using System;

namespace Params
{
    class Test
    {
        public void Add(int a)
        {
            a++;
        }
        public void Add(ref int a)
        {
            a++;
        }

        public void Div(out int a)
        {
            a = 10;
        }

        public void YingYongCanShu(int[] arr)
        {
            arr = new int[] { 1, 2, 3, 4 };
        }
        public void YingYongCanShu(ref int[] arr)
        {
            arr = new int[] { 1, 2, 3, 4 };
        }

        public int Plus(params int[] vs)
        {
            int result = 0;
            foreach (int item in vs)
            {
                result += item;
            }
            return result;
        }
    }
    class A
    {
        public string date;
        public A right;
        public A left;
        public A(string date, A left = null, A right = null)
        {
            this.date = date;
            this.left = left;
            this.right = right;
        }
        /// <summary>
        /// a = b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void Change(ref A a,ref A b)
        {
            a = b;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {       
            Test test = new Test();
           
            A d = new A("d");
            A c = new A("c",d);
            A b = new A("b",c);
            A a = new A("a",b);

            A.Change(ref b, ref c);
            Console.WriteLine(a.date + " " +a.left.date);
            Console.WriteLine(b.date + " " +b.left.date);
            Console.WriteLine(c.date + " " +c.left.date);
            Console.WriteLine(d.date);
            //int x = 1;
            //test.Add(ref x);
            //Console.WriteLine(x);

            //test.Div(out int y);
            //Console.WriteLine(y);

            //Console.WriteLine(test.Plus(1, 2, 3, 4, 5, 6, 8, 9));

            //int[] array = new int[] { 0,0,0,0 };
            //test.YingYongCanShu(array);
            //Console.WriteLine(array[0]);

            //int[] array1 = new int[] { 0, 0, 0, 0 };
            //test.YingYongCanShu(ref array1);
            //Console.WriteLine(array1[0]);
        }
    }
}
