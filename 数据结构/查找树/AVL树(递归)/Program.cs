using System;

namespace AVL树
{
    /*
     * where T : struct,IComparable ,T既是值类型，又继承Comparable接口
     * result = a1.CompareTo(a2); -1即a1<a2 , 0相等, 1即a1>a2
    */
    class AvlTree<T> where T : struct, IComparable
    {
        public class AvlNode
        {
            public T date;
            public AvlNode left;
            public AvlNode right;
            //子树的高度(新增)
            public int height;
            public AvlNode(T date, AvlNode left = null, AvlNode right = null, int height = 0)
            {
                this.date = date;
                this.left = left;
                this.right = right;
                this.height = height;
            }
        }
        private AvlNode root;
        //上一次查找到数据的Find执行次数
        public int count = 0;
        public AvlTree(AvlNode node = null)
        {
            root = node;
        }
        public AvlNode Find(T x)
        {
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
        //二叉搜索树中Find
        /*private AvlNode Find(T x, AvlNode root, ref int count)
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
        }*/
        private AvlNode Find(T x, AvlNode root, ref int count)
        {
            count = 1;
            while (root != null && root.date.CompareTo(x) != 0)
            {
                if (x.CompareTo(root.date) > 0)
                    root = root.right;
                else
                    root = root.left;
                count++;
            }
            if (root == null)
            {
                count = -1;
                Console.Write("没找到该节点 ");
                return this.root;
            }
            else
                return root;
        }
        private void Insert(T x, ref AvlNode root)
        {
            //！！看清执行顺序很重要
            if (root == null)
            {
                root = new AvlNode(x);
            }
            else if (x.CompareTo(root.date) < 0)//左子树插入，左孩子高度肯定大于右孩子
            {
                //总是先Insert，更新height后，调整
                Insert(x, ref root.left);
                //用GetHeight是为了避免出现指向null的状况，因为没有null.height = -1
                if (GetHeight(root.left) - GetHeight(root.right) == 2)//平衡度
                {
                    if (x.CompareTo(root.left.date) < 0)
                    {
                        LL(ref root);
                    }
                    else
                    {
                        LR(ref root);
                    }
                }
            }
            else//右子树插入，右孩子高度肯定大于左孩子
            {
                Insert(x, ref root.right);
                if (GetHeight(root.right) - GetHeight(root.left) == 2)
                {
                    if (x.CompareTo(root.right.date) > 0)
                    {
                        RR(ref root);
                    }
                    else
                    {
                        RL(ref root);
                    }
                }
            }
            root.height = max(GetHeight(root.left), GetHeight(root.right)) + 1;//每次都是优先insert，再一级一级往上计算height，插入玩每个节点的height都是正确的
        }
        private bool Remove(T x, ref AvlNode root)
        {
            bool stop = false;
            //分叉，1为删除的是左子树，2为右子树。默认值没有即他们以下节点不需要调整了，就是当前节点是删除节点，且是子节点或者是只有一个孩子,，而有两个孩子的节点实际上删除的不是它，而是它左子树中最大节点
            int bifurcate ;
            //递归出口
            if (root == null)
            {
                return true;
            }
            if (x.CompareTo(root.date) > 0)
            {
                stop = Remove(x, ref root.right);
                bifurcate = 2;
            }
            else if (x.CompareTo(root.date) < 0)
            {
                stop = Remove(x, ref root.left);
                bifurcate = 1;
            }
            else
            {
                
                //叶子节点
                if (root.left == null && root.right == null)
                {
                    //进入这一步的节点都是被删节点，这里不储存自己是上一级的左子树还是右子树的，需要返回上一级查找，但肯定是要调整的
                    root = null;
                    return false;
                }
                
                //有两个儿子，二个儿子的情况终究会回到叶子节点或者只有一个儿子的情况
                if (root.left != null && root.right != null)
                {
                    AvlNode node = root.left;
                    while (node.right != null)
                        node = node.right;
                    root.date = node.date;
                    //stop = Remove(root.date, ref node);//不能这么写了，没有考虑如果不是叶子节点
                    stop = Remove(root.date, ref root.left);
                    bifurcate = 1;
                }
                //只有一个儿子
                else
                {
                    //进入这一步的节点都是被删节点，这里不储存自己是上一级的左子树还是右子树的，需要返回上一级查找，但肯定是要调整的
                    root = (root.left != null) ? root.left : root.right;
                    return false;
                }
            }
            root.height = max(GetHeight(root.left), GetHeight(root.right)) + 1;


            //开始调整
            #region           
            //不需要调整了
            if (stop)
                return true;
            //平衡因子(删除前)
            int bal;
            switch (bifurcate)
            {
                case 1:
                    bal = GetHeight(root.left) - GetHeight(root.right) + 1;//+1代表被删除前，因为有递归优先是删除的
                    if (bal == 0)//平衡因子为0，即上一节点不需要调整;
                    {
                        return true;
                    }
                    else if (bal == 1)//从左侧较高的一侧删除,可能上一节点的平衡度发生变化
                    {
                        return false;
                    }
                    else if (bal == -1)//从比较矮的一侧左删除，必然要旋转
                    {
                        int Qbal = GetHeight(root.right.left) - GetHeight(root.right.right);//Q树的bal 即P节点被删除节点另一边的子树 不用+1因为这一边没被删除
                        switch (Qbal)
                        {
                            case 0:
                                RR(ref root);//root,高度为发生变化
                                return true;
                            //符号相同 -1 = -1
                            case -1:
                                RR(ref root);//root,高度-1
                                return false;
                            //case 1:
                            default:
                                RL(ref root);//root,高度-1
                                return false;
                        }
                    }
                    break;
                case 2:
                    bal = GetHeight(root.left) - GetHeight(root.right) - 1;//左-(右+1)
                    if (bal == 0)//平衡因子为0，即上一节点不需要调整;
                    {
                        return true;
                    }
                    else if (bal == -1)//从右侧较高的一侧删除,可能上一节点的平衡度发生变化
                    {
                        return false;
                    }
                    else if (bal == 1)//从矮的一侧右删除，要旋转
                    {
                        int Qbal = GetHeight(root.left.left) - GetHeight(root.left.right);
                        switch (Qbal)
                        {
                            case 0:
                                LL(ref root);
                                return true;
                            case 1:
                                LL(ref root);
                                return false;
                            //case -1:
                            default:
                                LR(ref root);
                                return false;
                        }
                    }
                    break;
            }

            //不会发生一下的事情，如果发生，程序错误
            Console.WriteLine("错误");
            return false;
            
            #endregion
        }
        #region//工具函数
        private int GetHeight(AvlNode node)
        {
            //当空节点为-1，因为新节点的高度为0，而每次New出来都要 root.height = max(GetHeight(root.left),GetHeight(root.right)) + 1，设为-1正好为0
            return node == null ? -1 : node.height;
        }
        private int max(int a, int b)
        {
            return a > b ? a : b;
        }
        private void LL(ref AvlNode node)
        {
            AvlNode p = node.left;
            node.left = p.right;
            p.right = node;
            //需要算插入后的高度(改变后p和node高度均发生变化)
            node.height = max(GetHeight(node.left), GetHeight(node.right)) + 1;
            p.height = max(GetHeight(p.left), GetHeight(node)) + 1;

            //将P设置为根
            node = p;
        }
        private void LR(ref AvlNode node)
        {
            RR(ref node.left);
            LL(ref node);
        }
        private void RL(ref AvlNode node)
        {
            LL(ref node.right);
            RR(ref node);
        }
        private void RR(ref AvlNode node)
        {
            AvlNode p = node.right;
            node.right = p.left;
            p.left = node;
            node.height = max(GetHeight(node.left), GetHeight(node.right)) + 1;
            p.height = max(GetHeight(p.right), GetHeight(node)) + 1;

            node = p;
        }
        #endregion
    }
    class Program
    {
        static void Main()
        {
            AvlTree<float> tree = new AvlTree<float>();
            //float[] vs = new float[] { 5, 7, 4, 8, 9, 7.5f };
            //for (int i = 0; i < vs.Length; i++)
            //{
            //    tree.Insert(vs[i]);
            //}

            //tree.Remove(7);
            //for (int i = 0; i < vs.Length; i++)
            //{
            //    AvlTree<float>.AvlNode avlNode = tree.Find(vs[i]);
            //    Console.WriteLine("数据 " + avlNode.date + " 孩子高度 " + avlNode.height + " 查找次数 " + tree.count);
            //}
            //Console.WriteLine("--------------------------------------------------------------");

            //tree.Remove(5);
            //for (int i = 0; i < vs.Length; i++)
            //{
            //    AvlTree<float>.AvlNode avlNode = tree.Find(vs[i]);
            //    Console.WriteLine("数据 " + avlNode.date + " 孩子高度 " + avlNode.height + " 查找次数 " + tree.count);
            //}
            //Console.WriteLine("--------------------------------------------------------------");

            //tree.Remove(8);
            //for (int i = 0; i < vs.Length; i++)
            //{
            //    AvlTree<float>.AvlNode avlNode = tree.Find(vs[i]);
            //    Console.WriteLine("数据 " + avlNode.date + " 孩子高度 " + avlNode.height + " 查找次数 " + tree.count);
            //}
            //Console.WriteLine("--------------------------------------------------------------");
            
            float[] vs = new float[] { 5, 4, 8, 9, 3, 4.5f, 4.3f, 4.6f};
            for (int i = 0; i < vs.Length; i++)
            {
                tree.Insert(vs[i]);
            }
            for (int i = 0; i < vs.Length; i++)
            {
                AvlTree<float>.AvlNode avlNode = tree.Find(vs[i]);
                Console.WriteLine("数据 " + avlNode.date + " 孩子高度 " + avlNode.height + " 查找次数 " + tree.count);
            }
            Console.WriteLine("--------------------------------------------------------------");
            tree.Remove(5f);
            for (int i = 0; i < vs.Length; i++)
            {
                AvlTree<float>.AvlNode avlNode = tree.Find(vs[i]);
                Console.WriteLine("数据 " + avlNode.date + " 孩子高度 " + avlNode.height + " 查找次数 " + tree.count);
            }
            Console.WriteLine("--------------------------------------------------------------");
            tree.Remove(3f);
            for (int i = 0; i < vs.Length; i++)
            {
                AvlTree<float>.AvlNode avlNode = tree.Find(vs[i]);
                Console.WriteLine("数据 " + avlNode.date + " 孩子高度 " + avlNode.height + " 查找次数 " + tree.count);
            }
            Console.WriteLine("--------------------------------------------------------------");

        }
    }
}
