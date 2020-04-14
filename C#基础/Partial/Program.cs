using System;

namespace Partial
{
    partial class A
    {
        public void Speak()
        {
            Console.WriteLine("11");
        }
        public void say(object param)
        {
            string[] vs = (string[])param;
            foreach (var item in vs)
            {
                Console.WriteLine(item);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string[] vs = new string[] { "haha", "xixi", "dongdong" };
            A a = new A();
            a.Speak();
            a.Speak2();
            a.say(vs);
        }
    }
}
