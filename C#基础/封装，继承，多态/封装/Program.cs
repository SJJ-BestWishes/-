using System;

namespace packaging
{
    abstract class Animal
    {
        //用pritected代替private
        protected float hp;
        //只读
        public float HP 
        {
            get { return hp; }
        }
        /// <summary>
        /// HP改变的值
        /// </summary>
        /// <param name="change"></param>
        public virtual void HpChange(float change)
        {
            hp += change;
        }
        public abstract void Walk();
        public void HaHa()
        {
            Console.WriteLine("haha");
        }
    }
    class Player : Animal
    {
        public static int objectsCount = 0;
        public Player(float hp)
        {
            this.hp = hp;
            objectsCount++;
        }
        public override void Walk()
        {
            HpChange(-1);
        }
        public override void HpChange(float change)
        {
            base.HpChange(change);
            Console.WriteLine("Change");
        }
    }

    class Program
    {
        static void Main()
        {
            Player player = new Player(100);
            Player player1 = new Player(1);
            player.Walk();
            Console.WriteLine(player.HP+" "+Player.objectsCount);
            player.HaHa();
        }
    }
}
