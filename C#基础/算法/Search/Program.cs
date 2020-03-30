using System;

namespace Search
{
    class Search
    {
        public int BinarySearch(float[] array , float target)
        {
            return BinarySearch(array, target, 0, array.Length);
        }
        private int BinarySearch(float[] array, float target, int low, int high)
        {
            int mid = (low + high) / 2;
            if (low >= high)
                return -1;
            else if (array[mid] == target)
            {
                return mid;
            }
            else if (array[mid] > target)
            {
                return BinarySearch(array, target, low, mid - 1);
            }
            else
            {
                return BinarySearch(array, target, mid + 1, high);
            }
        }
    }

    class Program
    {
        static void  Main()
        {
            Search search= new Search();
            float[] vs = { 4, 7, 10, 15, 17, 19, 20 ,19,12,45,70,0,4,7,-1,5,-20};
            Array.Sort(vs);
            foreach (var item in vs)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            Console.WriteLine(search.BinarySearch(vs, 600));
        }
    }
}
