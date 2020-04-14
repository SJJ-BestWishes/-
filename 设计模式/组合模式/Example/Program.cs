using System;
using System.Collections.Generic;

namespace Example
{
    class Program
    {
        //安全模式
        interface Component
        {
            public float Charge();
        }

        class Composite : Component
        {
            string name;
            public Composite(string name)
            {
                this.name = name;
            }
            private List<Component> children = new List<Component>();
            public void ADD(Component child)
            {
                children.Add(child);
            }
            public void Remove(Component c)
            {
                children.Remove(c);
            }
            public Component GetChild(int i)
            {
                return children[i];
            }
            public float Charge()
            {
                float total = 0;
                Console.WriteLine(name + "装了: ");
                foreach (Component item in children)
                {
                    total += item.Charge();
                    Console.WriteLine();
                }               
                return total;
            }
        }

        class Leaf : Component
        {
            private string name;
            public string Name
            {
                get { return name; }
            }
            private float price;
            public float Price
            {
                get
                { return price; }
            }
            public Leaf(string name , float price)
            {
                this.name = name;
                this.price = price;
            }
            public float Charge()
            {
                Console.Write(name + " 单价: " + price + " ");               
                return price;
            }
        }

        static void Main(string[] args)
        {
            Leaf apple = new Leaf("苹果", 5);
            Leaf banana = new Leaf("香蕉", 2);
            Leaf pineapple = new Leaf("菠萝", 20);
            Leaf pear = new Leaf("梨", 4);

            Composite red = new Composite("红袋子");
            Composite blue = new Composite("蓝袋子");
            Composite black = new Composite("黑袋子");

            red.ADD(apple);
            red.ADD(banana);

            blue.ADD(pineapple);
            blue.ADD(apple);
            blue.ADD(pear);

            black.ADD(red);
            black.ADD(blue);
            black.ADD(pear);
            Console.WriteLine("总价" + black.Charge());
        }
    }
}
