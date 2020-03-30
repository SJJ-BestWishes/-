using System;

namespace duoTai
{
    class Animal
    {
        public virtual void Bark()
        {
            Console.WriteLine("...");
        }
    }
    class Cat:Animal
    {
        public override void Bark()
        {
            base.Bark();
            Console.WriteLine("喵");
        }
        public void Sleep()
        {
            Console.WriteLine("sleep");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Animal animal = new Animal();
            animal.Bark();
            Animal cat = new Cat();
            cat.Bark();
            //不能因为Animal类型中没有Sleep,这里只是把Cat类型对象装在了Animal类这个盒子里，但是只能装下Animal中的方法，Cat独有的方法装不下，所以用不了Cat的方法
            //cat.Sleep();
        }
    }
}
