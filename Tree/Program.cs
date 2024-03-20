using tree_lab_twelve;
namespace Tree
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //You can use own comparer class
            BinaryTree<string> tree = new(new OwnComparer());
            tree.Add("Aa");
            for (char c = 'A'; c <= 'Z'; c++ /*ya, c++ in C# xd*/)
            {
                tree.Add((c + c).ToString());
                tree.Add(c.ToString());
            }


            tree.PrintTree();
        }
    }
}
