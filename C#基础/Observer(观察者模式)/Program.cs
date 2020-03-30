using System;
using System.Collections.Generic;

namespace Observer
{    
    class Player
    {
        private float hp;
        private float maxHp;
        public float Hp {
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
                HPEvent?.Invoke(Hp);
            }
        }
        public Player(float hp,float maxHp)
        {
            this.maxHp = maxHp;
            Hp = hp;
        }
        public delegate void HPHandler(float param);
        public event HPHandler HPEvent;
        public void Damege(float damage)
        {
            Hp -= damage;         
        }   
    }
    class GamePanelView
    {
        public GamePanelView()
        {
            
        }
        public void HpChange(float hp)
        {
            Console.WriteLine(hp);
        }
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
            player.HPEvent += gamePanelView.HpChange;
            player.Damege(0);
            player.Damege(enemy.damage);
        }
    }
}
