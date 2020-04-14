using System;
using System.Collections.Generic;

namespace CompositeMode
{
    //透明模式
    class Program
    {
        //抽象构件
        interface Component
        {
            public void Add(Component c);
            public void Remove(Component c);
            public Component GetChild(int i);
            public void Operation();
        }
        //树枝构件
        class Composite : Component
        {
            private List<Component> children = new List<Component>();
            public void Add(Component c)
            {
                children.Add(c);
            }
            public void Remove(Component c)
            {
                children.Remove(c);
            }
            public Component GetChild(int i)
            {
                return children[i];
            }
            public void Operation()
            {
                foreach (Component item in children)
                {
                    item.Operation();
                }
            }
        }
        //树叶构件
        class Leaf : Component
        {
            private String name;
            public Leaf(String name)
            {
                this.name = name;
            }
            public void Add(Component c) { }
            public void Remove(Component c) { }
            public Component GetChild(int i)
            {
                return null;
            }
            public void Operation()
            {
                Console.WriteLine("树叶" + name + "：被访问！");
            }
        }
        static void Main(string[] args)
        {
            Component c0 = new Composite();
            Component c1 = new Composite();
            Component leaf1 = new Leaf("1");
            Component leaf2 = new Leaf("2");
            Component leaf3 = new Leaf("3");
            c0.Add(leaf1);
            c0.Add(c1);
            c1.Add(leaf2);
            c1.Add(leaf3);
            c0.Operation();
        }
    }
}
