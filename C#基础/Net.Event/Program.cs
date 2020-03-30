using System;

namespace Net.Event
{
    /* .Net中提供的Event事件函数
    class A
    {
        //1.
        public EventHandler Boiled;等于

        public delegate void BoiledEventHandler(Object sender, EventArgs eventArgs);
        public event BoiledEventHandler Boiled;

        //2.
        public EventHandler<BoiledEventArgs> Boiled;等于

        public delegate void BoiledEventHandler(Object sender, BoiledEventArgs eventArgs);
        public event BoiledEventHandler Boiled;
        public class BoiledEventArgs : EventArgs
        { }

        tips:Object sender传递检查对象本身，如果要知道private 就需要EventArgs；
    }
    */
    class Player
    {
        private int number = 1;
        private float hp;
        public float maxHp;
        public float Hp
        {
            get
            { return hp; }
            set
            {
                if (value <= 0)
                {
                    hp = 0;
                }
                else if (value >= maxHp)
                {
                    hp = maxHp;
                }
                else
                {
                    hp = value;
                }
            }
        }
        public Player(float hp, float maxHp)
        {
            this.maxHp = maxHp;
            Hp = hp;
        }
        /*1.只需要知道HP
        public delegate void HPHandler(float param);
        public event HPHandler HPChange;
        public void Damege(float damage)
        {
            Hp -= damage;
            HPChange?.Invoke(Hp);
        }
        */
        /* 2.把这个对象传过去（但是私有的看不到）
        public EventHandler HPChange;
        public void Damege(float damage)
        {
            Hp -= damage;
            HPChange?.Invoke(this, EventArgs.Empty);
        }*/
        /* 3.把这个对象传过去 还可以利用EventArgs自定义传输Player私有字段或属性
        public EventHandler<HPChangeEventArgs> HPChange;
        public void Damege(float damage)
        {
            Hp -= damage;
            HPChange?.Invoke(this, new HPChangeEventArgs(number));
        }
        */
    }
    class HPChangeEventArgs : EventArgs
    {
        public readonly int number;
        public HPChangeEventArgs(int number)
        {
            this.number = number;
        }
    }
    class GamePanelView
    {
        /* 1
        public void HpChange(float hp)
        {
            Console.WriteLine(hp);
        }
        */
        /* 2
        public void HpChange(Object sender,EventArgs eventArgs)
        {
            Player player = (Player)sender;
            Console.WriteLine(player.Hp);
            Console.WriteLine(player.maxHp);
            //不行,因为player.number私有
            //Console.WriteLine(player.number);
        }
        */
        /* 3
        public void HpChange(Object sender, HPChangeEventArgs eventArgs)
        {
            Player player = (Player)sender;
            Console.WriteLine(player.Hp);
            Console.WriteLine(player.maxHp);
            //可get Player private属性但是不是真实的取到，改也无济于事
            Console.WriteLine(eventArgs.number);
        }
        */
    }
    class Enemy
    {
        public float damage { get; set; }
        public Enemy(float damage)
        {
            this.damage = damage;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player(120, 100);
            GamePanelView gamePanelView = new GamePanelView();
            Enemy enemy = new Enemy(5);
            /*1,2,3*/
            player.HPChange += gamePanelView.HpChange;
            player.Damege(0);
            player.Damege(enemy.damage);
            
        }
    }
}
