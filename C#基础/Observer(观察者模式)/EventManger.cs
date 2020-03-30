//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Observer
//{
//    //本质，把监视对象中的EventHander 换成了Event;还是在类外监听，类内触发
//    public enum Event
//    {
//        Hp_Change,
//    }
//    public class EventManager
//    {
//        private static EventManager instance;
//        public static EventManager Instance
//        {
//            get
//            {
//                if (instance == null)
//                    instance = new EventManager();
//                return instance;
//            }
//        }
//        private EventManager()
//        { }
//        private readonly Dictionary<Event, EventHandler> eventDictionary = new Dictionary<Event, EventHandler>();
//        /// <summary>
//        /// 给Event添加监视者
//        /// </summary>
//        /// <param name="eventName">事件名称</param>
//        /// <param name="listenerAction">监视者,On....Change</param>
//        public void AddListener(Event eventName, Action<object, EventArgs> listenerAction)
//        {
//            if (!eventDictionary.ContainsKey(eventName))
//            {
//                eventDictionary.Add(eventName, new List<Action<object, EventArgs>>());
//            }
//            eventDictionary[eventName].Add(listenerAction);
//        }
//        /// <summary>
//        /// 事件发布者触发
//        /// </summary>
//        /// <param name="eventName">事件名称</param>
//        public void Fire(Event eventName,object obj, EventArgs eventArgs)
//        {
//            if (eventDictionary.ContainsKey(eventName))
//            {
//                foreach (Action<Object, EventArgs> item in eventDictionary[eventName])
//                {
//                    item(obj, eventArgs);
//                }
//            }
//        }
//    }

//    class Player1
//    {
//        private float hp;
//        private float maxHp;
//        public float Hp
//        {
//            get
//            { return hp; }
//            set
//            {
//                if (value <= 0)
//                {
//                    hp = 0;
//                }
//                else if (value >= maxHp)
//                {
//                    hp = maxHp;
//                }
//                else
//                {
//                    hp = value;
//                }
//            }
//        }
//        public Player1(float hp, float maxHp)
//        {
//            this.maxHp = maxHp;
//            Hp = hp;
//        }
        
//        public void Damege(float damage)
//        {
//            Hp -= damage;
//            EventManager.Instance.Fire(Event.Hp_Change, this, new HPChangeEventArgs(maxHp));
//        }        
//    }
//    class HPChangeEventArgs : EventArgs
//    {
//        public readonly float maxHp;
//        public HPChangeEventArgs(float maxHp)
//        {
//            this.maxHp = maxHp;
//        }
//    }
//    class GamePanelView1
//    {
//        public void OnHpChange(object sender, HPChangeEventArgs hPChangeEventArgs)
//        {
//            Player1 player = (Player1)sender;
//            Console.WriteLine(player.Hp);
//            Console.WriteLine(hPChangeEventArgs.maxHp);
//        }
//        public GamePanelView1()
//        {
//            EventManager.Instance.AddListener(Event.Hp_Change, OnHpChange);
//        }
//    }
//    class Enemy1
//    {
//        public float damage { get; set; }
//        public Enemy1(float damage)
//        {
//            this.damage = damage;
//        }
//    }

//    class Start
//    {
//        static void Main()
//        {
//            Player1 player = new Player1(120, 100);
//            GamePanelView1 gamePanelView = new GamePanelView1();
//            Enemy1 enemy = new Enemy1(5);
//            player.Damege(5);
//        }
//    }
//}
