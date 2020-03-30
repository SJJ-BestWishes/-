using System;
using System.Collections.Generic;

namespace 红黑树
{
    class RedBlackTree<T> where T : struct, IComparable
    {
        public enum Color
        {
            red,
            black
        }
        public class RedBlackNode
        {
            public T date;
            public RedBlackNode left;
            public RedBlackNode right;
            public Color color;
            public RedBlackNode(T date, RedBlackNode left = null, RedBlackNode right = null, Color color = Color.red)
            {
                this.date = date;
                this.left = left;
                this.right = right;
                this.color = color;
            }
        }
        private RedBlackNode root;
        public RedBlackTree(RedBlackNode node =null)
        {
            root = node;
        }
        //上一次查找到数据的Find执行次数
        public int count = 0;
        public RedBlackNode Find(T x)
        {
            return Find(x, root , ref count);
        }
        private RedBlackNode Find(T x, RedBlackNode root, ref int count)
        {
            count = 1;
            while (root != null && x.CompareTo(root.date) != 0)
            {
                if (x.CompareTo(root.date) > 0)
                {
                    root = root.right;
                }
                else
                {
                    root = root.left;
                }
                count++;
            }
            if (root == null)
            {
                count = -1;
                Console.WriteLine("没有找到节点" + x);
                return this.root;
            }
            else
            {
                return root;
            }
        }
        public void Insert(T x)
        {
            //1.递归
            //栈存放 插入的路径
            Stack<RedBlackNode> path = new Stack<RedBlackNode>();
            Insert(x, ref root ,ref path);

            //2.迭代
            //Insert(x, root);
        }
        /// <summary>
        /// 递归
        /// </summary>
        private bool Insert(T x, ref RedBlackNode root , ref Stack<RedBlackNode> path)
        {
            bool stop;
            int bifurcate;           
            //如果根节点为空
            if (this.root == null)
            {
                this.root = new RedBlackNode(x, null, null, Color.black);
                return true;//直接返回,下面的不用执行了
            }
            if (root == null)
            {
                root = new RedBlackNode(x, null, null, Color.red);
                //黑节点直接插入，就完成
                if (path.Pop().color == Color.black)//path.Pop(),把他的父节点POP掉，因为return，操作的是他的父节点
                {
                    return true;
                }
                else
                {
                    return false;
                }               
            }
            else if (x.CompareTo(root.date) == 0)
            {
                return true;
            }
            else if (x.CompareTo(root.date) > 0)
            {
                path.Push(root);
                stop = Insert(x, ref root.right,ref path);
                bifurcate = 2;
            }
            else
            {
                path.Push(root);
                stop = Insert(x, ref root.left, ref path);
                bifurcate = 1;
            }
            //执行到这里的时候,
            if (stop)
            {
                return true;
            }
            else//这里一定是插入节点的父节点是红色
            {              
                RedBlackNode notRefRoot = root;
                RedBlackNode uncle, father,rootOfSubTree;

                while (notRefRoot.color == Color.red)
                {
                    if (notRefRoot == this.root)
                    {
                        notRefRoot.color = Color.black;
                        return true;
                    }
                    father = rootOfSubTree =path.Pop();
                    //叔叔节点为插入节点父节点的兄弟节点
                    uncle = (father.date.CompareTo(notRefRoot.date)>0) ? father.right : father.left;
                    //情况1 叔叔节点为红色 
                    if (GetColor(uncle) == Color.red)
                    {
                        notRefRoot.color = Color.black;
                        father.color = Color.red;
                        uncle.color = Color.black;
                        if (father == this.root)
                        {
                            father.color = Color.black;
                            return true;//这里到头了，要返回
                        }
                        notRefRoot = path.Pop();
                    }
                    else
                    {
                        switch (bifurcate)
                        {
                            case 1:
                                if (father.left == root)
                                {
                                    notRefRoot.color = Color.black;
                                    father.color = Color.red;
                                    LL(ref father);
                                }
                                else
                                {
                                    notRefRoot.left.color = Color.black;
                                    father.color = Color.red;
                                    RL(ref father);
                                }
                                ReLink(rootOfSubTree,father,ref path);
                                return true;
                            case 2:
                                if (father.right == root)
                                {
                                    notRefRoot.color = Color.black;
                                    father.color = Color.red;
                                    RR(ref father);
                                }
                                else
                                {
                                    notRefRoot.left.color = Color.black;
                                    father.color = Color.red;
                                    LR(ref father);
                                }
                                ReLink(rootOfSubTree, father, ref path);
                                return true;
                        }
                    }
                }               
            }
            return true;
        }
        /// <summary>
        /// 迭代
        /// </summary>
        private void Insert(T x , RedBlackNode root)
        {
            //栈存放 插入的路径
            Stack<RedBlackNode> path = new Stack<RedBlackNode>();
            RedBlackNode father;

            //如果根节点为空
            if (this.root == null)
            {
                this.root = new RedBlackNode(x, null, null, Color.black);
                return;//直接返回,下面的不用执行了
            }
            //Find
            while (root != null && x.CompareTo(root.date) != 0)
            {
                //添加入路径
                path.Push(root);
                if (x.CompareTo(root.date) > 0)
                {                    
                    root = root.right;
                }
                else
                {
                    root = root.left;
                }
            }
            //找到相同值
            if (root != null)
            {
                return;
            }
            else
            {
                //查看第一个就是他的父亲
                father = path.Peek();
                //如果父亲节点是黑色，插入直接返回
                if (father.color == Color.black)
                {
                    root = new RedBlackNode(x);
                    //root 传的不是Ref，只是指向father.left/right 的一个指针,需要ReLink
                    if (x.CompareTo(father.date) < 0)
                    {
                        father.left = root;
                    }
                    else
                    {
                        father.right = root;
                    }
                    return;
                }
                //如果父亲节点是红色插入后还要调整
                else
                {
                    root = new RedBlackNode(x);
                    if (x.CompareTo(father.date) < 0)
                    {
                        father.left = root;
                    }
                    else
                    {
                        father.right = root;
                    }
                    InsertColorBalance(root, ref path);                   
                }
            }
        }
        /// <summary>
        /// 插入时如果父亲节点是红色,在插入后需要的调整。
        /// </summary>
        private void InsertColorBalance(RedBlackNode root, ref Stack<RedBlackNode> path)
        {
            //父亲节点,祖父节点，父亲的兄弟节点
            RedBlackNode father,grandfather, uncle , rootOfSubTree;
            father = path.Pop();
            while (father.color == Color.red)//一直为情况2
            {
                //如果父亲就是根节点,为了确保还有祖父节点
                if (father == this.root)
                {
                    father.color = Color.black;
                    return;
                }
                grandfather = rootOfSubTree =path.Pop();
                //如果父节点是祖父节点的左儿子
                if (grandfather.date.CompareTo(father.date) > 0)
                {
                    uncle = grandfather.right;
                }
                else
                {
                    uncle = grandfather.left;
                }
                //情况1 父亲兄弟节点为黑色
                //if (uncle == null || uncle.color == Color.black) //！uncle==null要写在前面
                if (GetColor(uncle) == Color.black)
                {
                    if (grandfather.left == father)
                    {
                        if (father.left == root)
                        {
                            father.color = Color.black;
                            grandfather.color = Color.red;
                            LL(ref grandfather);
                        }
                        else if (father.right == root)
                        {
                            root.color = Color.black;
                            grandfather.color = Color.red;
                            LR(ref grandfather);
                        }
                    }
                    else if (grandfather.right == father)
                    {
                        if (father.right == root)
                        {
                            father.color = Color.black;
                            grandfather.color = Color.red;
                            RR(ref grandfather);
                        }
                        else if (father.left == root)
                        {
                            root.color = Color.black;
                            grandfather.color = Color.red;
                            RL(ref grandfather);
                        }
                    }
                    ReLink(rootOfSubTree, grandfather, ref path);
                    return;//这里执行完就Over了，不需要再返回判断
                }
                //情况2  父亲兄弟节点为红色
                else if (uncle.color == Color.red)
                {
                    grandfather.color = Color.red;
                    father.color = Color.black;
                    uncle.color = Color.black;
                    if (this.root == grandfather)
                    {
                        grandfather.color = Color.black;
                        return;//这里到头了，要返回
                    }
                    root = grandfather;
                    father = path.Pop();
                }
            }
        }
        public void Remove(T x)
        {
            Remove(x, root);
        }
        private void Remove(T x, RedBlackNode root)
        {
            Stack<RedBlackNode> path = new Stack<RedBlackNode>();
            //old为之前root指向
            RedBlackNode father, old;
            while (root != null && x.CompareTo(root.date) != 0)//!!root!=null 写在前
            {
                path.Push(root);
                if (x.CompareTo(root.date) > 0)
                {
                    root = root.right;
                }
                else
                {
                    root = root.left;
                }
            }
            //没找到节点
            if (root == null)
            {
                return;
            }
            //被删除节点有两个儿子,转换为删除叶子节点或者一个孩子的节点
            if (root.right != null && root.left != null)
            {
                path.Push(root);
                old = root;                
                root = root.left;
                while (root.right != null)
                {
                    path.Push(root);
                    root = root.right;
                }
                old.date = root.date;
            }
            //因为有前面两个儿子,转换为删除叶子节点或者一个孩子的节点，所以只有当根节只有一个或者没有儿子并且要求删除根节点才会执行。
            //tips：被删除节点为根,1.根有非空孩子，那么就让这个孩子作为根，这个树变成了只有一个根节点的树。2.根没有非空孩子，就变成了空树
            if (root == this.root)
            {
                this.root = (root.left!=null ? root.left : root.right);
                if (root != null)
                {
                    root.color = Color.black;
                }
                return;
            }

            father = path.Peek();
            old = root;
            //删除的是叶子节点
            if (root.left == null && root.right == null)
            {
                //叶子节点为红色
                if (root.color == Color.red)
                {
                    //root = null;不能这么删除,root不是ref
                    if (root == father.left)
                    {
                        //father.left = null;不能只单独这么写，这样写就直接与root断了关系
                        root = null;
                        father.left = root;
                    }
                    else if (root == father.right)
                    {
                        root = null;
                        father.right = root;
                    }
                    return;
                }
                //叶子节点为黑色
                else if (root.color == Color.black)
                {
                    //root = null;//不能这么删除,root不是ref
                    if (root == father.left)
                    {
                        //father.left = null;不能只单独这么写，这样写就直接与root断了关系
                        root = null;
                        father.left = root;
                    }
                    else if (root == father.right)
                    {
                        root = null;
                        father.right = root;
                    }
                    Console.WriteLine("删除叶子节点为黑色\n");//TODO
                    RemoveColorBalance(root,ref path);
                    return;
                }
            }
            //只有一个子节点
            else
            {
                root = (root.left != null ? root.left : root.right);
                if (father.left == old)
                {
                    father.left = root;
                }
                else if(father.right == old)
                {
                    father.right = root;
                }
                root.color = Color.black;
                return;
            }
        }
        /// <summary>
        /// 删除时叶子节点为黑色时候调整
        /// </summary>
        private void RemoveColorBalance(RedBlackNode root, ref Stack<RedBlackNode> path)
        {
            //父亲，兄弟节点,在(4种旋转之后)定位当初Root用来Relink的
            RedBlackNode parent, sibling, rootOfSubTree;
            parent = rootOfSubTree = path.Pop();
            //循环里用来判断被删节点为黑色并且没有红儿子且父亲节点为黑色时
            while (parent != null)
            {
                if (parent.left == root)
                {
                    sibling = parent.right;
                }
                else
                {
                    sibling = parent.left;
                }
                //被删节点兄弟节点是黑色
                if (sibling.color == Color.black)
                {
                    //L0或者R0即兄弟节点没有红儿子
                    if ((GetColor(sibling.left) == Color.black) && (GetColor(sibling.right) == Color.black))
                    {
                        //节点父亲为红色，调整颜色即可
                        if (parent.color == Color.red)
                        {
                            sibling.color = Color.red;
                            parent.color = Color.black;
                            return;//调节完成
                        }
                        //节点父亲为黑色，调整颜色后，那么所有经过该父节点的节点的黑节点数量都将减少1，所以设置该节点为被调整节点
                        else if (parent.color == Color.black)
                        {
                            sibling.color = Color.red;
                            root = parent;
                            //假设调整节点为根，那么就无法调整了！！正应是根，所以所有经过该根节点的节点的黑节点数量都将减少1，还是平衡的，所以不用调整了。
                            if (root == this.root)
                            {
                                return;
                            }
                            else
                            {
                                parent = rootOfSubTree = path.Pop();//可能会跳到被删节点兄弟节点是红色
                            }
                        }
                    }
                    else
                    {
                        //其他的情况不在循环里
                        break;
                    }
                }
                //被删节点兄弟节点是红色，所以父亲节点一定是黑色，做单旋转变为-->被删节点兄弟节点是黑色这种情况
                else if (sibling.color == Color.red)
                {
                    sibling.color = Color.black;
                    parent.color = Color.red;
                    if (parent.left == root)
                    {
                        RR(ref parent);
                    }
                    else
                    {
                        LL(ref parent);
                    }
                    ReLink(rootOfSubTree, parent, ref path);
                    //因为本质上没有调整
                    path.Push(parent);
                    parent = rootOfSubTree;
                }
            }
            
            //一下全都是被删节点兄弟节点是黑色的情况啦
            //被删节点为左孩子
            if (parent.left == root)
            {
                sibling = parent.right;
                //兄弟有一个红儿子并且为内侧和两个红儿子即这里的R1L和R2情况是一样的
                if (GetColor(sibling.left) == Color.red)//R1L或者R2
                {
                    sibling.left.color = parent.color;
                    parent.color = Color.black;
                    LR(ref parent);
                    ReLink(rootOfSubTree, parent, ref path);
                }
                else//应为一共四种情况，没有儿子的情况While已经讨论过了，兄弟有一个红儿子并且为内侧和两个红儿子上面if也讨论过了，就只有一个儿子为外侧节点的情况了
                {
                    sibling.color = parent.color;
                    sibling.right.color = Color.black;
                    parent.color = Color.black;
                    RR(ref parent);
                    ReLink(rootOfSubTree, parent, ref path);
                }
            }
            //被删节点为右孩子
            else if (parent.right == root)
            {
                sibling = parent.left;
                if (GetColor(sibling.right) == Color.red)//L1R或者L2
                {
                    sibling.right.color = parent.color;
                    parent.color = Color.black;
                    LR(ref parent);
                    ReLink(rootOfSubTree, parent, ref path);
                }
                else//L1L
                {
                    sibling.color = parent.color;
                    sibling.left.color = Color.black;
                    parent.color = Color.black;
                    LL(ref parent);
                    ReLink(rootOfSubTree, parent, ref path);
                }
            }
        }
        #region//工具函数
        /// <summary>
        /// 向右旋转,这里传递的node是形参,只是一个指针,真实的node的父节点并没有指向这个node。所以最后node = p 并没有成为预期效果( node的父节点的left/right = p ),node的父节点还是指向 原来的node的内容，为不是现在的p或者node
        /// </summary>
        /// <param name="node">根节点</param>
        private void LL(ref RedBlackNode node)
        {
            RedBlackNode p = node.left;
            node.left = p.right;
            p.right = node;
            node = p;
        }
        private void RR(ref RedBlackNode node)
        {
            RedBlackNode p = node.right;
            node.right = p.left;
            p.left = node;
            node = p;
        }
        private void LR(ref RedBlackNode node)
        {
            RR(ref node.left);
            LL(ref node);
        }
        private void RL(ref RedBlackNode node)
        {
            LL(ref node.right);
            RR(ref node);
        }
        private Color GetColor(RedBlackNode node)
        {
            if (node == null)
                return Color.black;
            else
                return node.color;
        }
        /// <summary>
        /// 为了实现LL/RR 最后一句 node = p 预期实现的效果
        /// </summary>
        /// <param name="oldp">老的节点node(就是LL/RR 传递时候的node)</param>
        /// <param name="newp">新的节点node(就是LL/RR 执行完成之后的node)</param>
        /// <param name="path"></param>
        private void ReLink(RedBlackNode oldp, RedBlackNode newp, ref Stack<RedBlackNode> path)
        {
            if (path.Count == 0)
            {
                root = newp;
            }
            else
            {
                RedBlackNode grandfather = path.Peek();
                if (grandfather.left == oldp)
                    grandfather.left = newp;
                else
                    grandfather.right = newp;
            }
        }
        #endregion
    }
    class Program
    {
        static void Main(string[] args)
        {
            RedBlackTree<float> tree = new RedBlackTree<float>();
            List<float> vs = new List<float> { 14, 9, 28, 5, 12, 25, 50, 3, 20, 26, 30, 60, 15, 23 };
            foreach (float item in vs)
            {
                tree.Insert(item);
            }
            foreach (float item in vs)
            {
                RedBlackTree<float>.RedBlackNode node = tree.Find(item);
                Console.WriteLine("数据 " + node.date + " 颜色 " + node.color + " 查找次数 " + tree.count);
            }
            Console.WriteLine("-------------------------------------------------");

            //RedBlackTree<float> tree1 = new RedBlackTree<float>();
            //foreach (float item in vs)
            //{
            //    tree1.Insert1(item);
            //}
            //foreach (float item in vs)
            //{
            //    RedBlackTree<float>.RedBlackNode node = tree1.Find(item);
            //    Console.WriteLine("数据 " + node.date + " 颜色 " + node.color + " 查找次数 " + tree1.count);
            //}
            //Console.WriteLine("-------------------------------------------------");



            tree.Insert(1);
            vs.Add(1);
            Console.WriteLine("插入1:");
            foreach (float item in vs)
            {
                RedBlackTree<float>.RedBlackNode node = tree.Find(item);
                Console.WriteLine("数据 " + node.date + " 颜色 " + node.color + " 查找次数 " + tree.count);
            }
            Console.WriteLine("-------------------------------------------------");

            tree.Insert(55);
            vs.Add(55);
            Console.WriteLine("插入55:");
            foreach (float item in vs)
            {
                RedBlackTree<float>.RedBlackNode node = tree.Find(item);
                Console.WriteLine("数据 " + node.date + " 颜色 " + node.color + " 查找次数 " + tree.count);
            }
            Console.WriteLine("-------------------------------------------------");

            tree.Insert(24);
            vs.Add(24);
            Console.WriteLine("插入24:");
            foreach (float item in vs)
            {
                RedBlackTree<float>.RedBlackNode node = tree.Find(item);
                Console.WriteLine("数据 " + node.date + " 颜色 " + node.color + " 查找次数 " + tree.count);
            }
            Console.WriteLine("-------------------------------------------------");

            //tree.Remove(24);
            //Console.WriteLine("删除24:");
            //foreach (float item in vs)
            //{
            //    RedBlackTree<float>.RedBlackNode node = tree.Find(item);
            //    Console.WriteLine("数据 " + node.date + " 颜色 " + node.color + " 查找次数 " + tree.count);
            //}
            //Console.WriteLine("-------------------------------------------------");

            //tree.Remove(60); 
            //Console.WriteLine("删除60:");
            //foreach (float item in vs)
            //{
            //    RedBlackTree<float>.RedBlackNode node = tree.Find(item);
            //    Console.WriteLine("数据 " + node.date + " 颜色 " + node.color + " 查找次数 " + tree.count);
            //}
            //Console.WriteLine("-------------------------------------------------");

            //tree.Remove(26);//难
            //Console.WriteLine("删除26:");
            //foreach (float item in vs)
            //{
            //    RedBlackTree<float>.RedBlackNode node = tree.Find(item);
            //    Console.WriteLine("数据 " + node.date + " 颜色 " + node.color + " 查找次数 " + tree.count);
            //}
            //Console.WriteLine("-------------------------------------------------");

            //tree.Remove(14);
            //Console.WriteLine("删除14:");
            //foreach (float item in vs)
            //{
            //    RedBlackTree<float>.RedBlackNode node = tree.Find(item);
            //    Console.WriteLine("数据 " + node.date + " 颜色 " + node.color + " 查找次数 " + tree.count);
            //}
            //Console.WriteLine("-------------------------------------------------");
        }
    }
}
