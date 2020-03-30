using System;

namespace 接口
{
    interface IA
    {
        void Fly();
        public void Speak();
    }
    interface IB
    {
        void Fly();
    }
    public interface IC
    {
        void Speak1();
    }
    class Bird : IA, IB, IC
    {
        void IA.Fly()
        {
            Console.WriteLine("A");
        }
        void IB.Fly()
        {
            Console.WriteLine("B");
        }

        void IA.Speak()
        {
            Console.WriteLine("Speak");
        }

        void IC.Speak1()
        {
            Console.WriteLine("Speak");
        }
    }
    class Bird2 : IC, IB, IA
    {
        public void Fly()
        {
            throw new NotImplementedException();
        }

        public void Speak()
        {
            throw new NotImplementedException();
        }

        public void Speak1()
        {
            throw new NotImplementedException();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Bird bird = new Bird();
            IA a = new Bird();
            IB b = new Bird();
            a.Fly();
            b.Fly();
        }
    }
}
