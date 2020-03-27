using System;

namespace 二叉查找树
{
    /*
     * where T : struct,IComparable ,T既是值类型，又继承Comparable接口
     * result = a1.CompareTo(a2); -1即a1<a2 , 0相等, 1即a1>a2
    */
    class BinarySearchTree<T> where T : struct,IComparable
    {
        public class BinaryNode
        {
            public T date;
            public BinaryNode left;
            public BinaryNode right;
            public BinaryNode(T date, BinaryNode left = null, BinaryNode right = null)
            {
                this.date = date;
                this.left = left;
                this.right = right;
            }
        }
        private BinaryNode root;
        public int count = 0;
        public BinarySearchTree(BinaryNode node = null)
        {
            root = node;
        }
        public BinaryNode Find(T x)
        {
            count = 0;
            return Find(x, root, ref count);
        }
        public void Insert(T x)
        {
            Insert(x, ref root);
        }
        public void Remove(T x)
        {
            Remove(x, ref root);
        }
        private BinaryNode Find(T x, BinaryNode root, ref int count)
        {
            if (root == null)
            {
                count = -1;
                Console.Write("false ");
                return this.root;
            }
            else if (x.CompareTo(root.date) == 0)
            {
                count++;
                return root;
            }
            else if (x.CompareTo(root.date) > 0)
            {
                count++;
                return Find(x, root.right, ref count);
            }
            else
            {
                count++;
                return Find(x, root.left, ref count);
            }
        }
        /// <summary>
        /// 递归实现
        /// </summary>
        private void Insert(T x, ref BinaryNode root)//这里可能要改变当节点的左右子节点，所以要传引用
        {
            if (root == null)
            {
                root = new BinaryNode(x);
            }
            else if (x.CompareTo(root.date) == 0)
            {
                return;
            }
            else if (x.CompareTo(root.date) > 0)
            {
                Insert(x, ref root.right);
            }
            else
            {
                Insert(x, ref root.left);
            }
        }
        /// <summary>
        /// 迭代实现Insert
        /// </summary>
       /*
        private void Insert(T x, BinaryNode root)//这里可能要改变当节点的左右子节点，所以要传引用
        {
            while (root != null && x.CompareTo(root.date)!=0)
            {
                if (x.CompareTo(root.date) > 0)
                {
                    root = root.right;
                }
                else if (x.CompareTo(root.date) < 0)
                {
                    root = root.left;
                }
            }
            if (x.CompareTo(root.date) == 0)
            {
                return;
            }
            else//TODO
            {
                root = new BinaryNode(x);
                if (root.father.date < root.date)
                {
                    root.father.right = root;
                }
                else
                {
                    root.father.left = root;
                }
            }
        }
        */

        //！！！重要
        private void Remove(T x, ref BinaryNode root)
        {
            if (root == null)
            {
                return;
            }
            else if (x.CompareTo(root.date) > 0)
            {
                Remove(x, ref root.right);
            }
            else if (x.CompareTo(root.date) < 0)
            {
                Remove(x, ref root.left);
            }
            else
            {
                //左右都没有孩子
                if (root.left == null && root.right == null)
                {
                    root = null;
                }
                //左右都有孩子
                else if (root.left != null && root.right != null)
                {
                    BinaryNode node = root.left;
                    while (node.right != null)
                        node = node.right;
                    root.date = node.date;

                    //Remove(root.date, ref node);不能这么写了，没有考虑如果不是叶子节点
                    Remove(root.date, ref root.left);
                }
                //1边有孩子
                else
                {
                    //！！！巧妙
                    root = (root.left != null) ? root.left : root.right;
                }
            }
        }

    }
    class Program
    {
        static void Main()
        {
            BinarySearchTree<float> tree = new BinarySearchTree<float>();
            float[] a = new float[] { 5, 7, 4, 8, 9, 7.5f };
            foreach (float item in a)
            {
                tree.Insert(item);
            }
            tree.Remove(7);
            tree.Remove(5);
            tree.Remove(8);
            foreach (float item in a)
            {
                Console.WriteLine(tree.Find(item).date + " " + tree.count);
            }
            //tree.Remove(5);
            //Console.WriteLine(tree.Find(4.7f));
            //Console.WriteLine(tree.count);
            //tree.Remove(6);
            //Console.WriteLine(tree.Find(5.2f));
            //Console.WriteLine(tree.count);

            //Console.WriteLine(tree.Find(1));
            //Console.WriteLine(tree.count);
            //tree.Remove(3);
            //Console.WriteLine(tree.Find(1));
            //Console.WriteLine(tree.count);
        }
    }
}
