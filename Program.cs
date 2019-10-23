using System;

namespace Korniienko_Task3
{
    class Program
    {
        private static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        static void Main(string[] args)
        {
            DateTime dt1 = new DateTime(2019, 09, 15);

            BinaryTree<StudentTests> binaryTree = new BinaryTree<StudentTests>();

            binaryTree.Notify += DisplayMessage;

            StudentTests stud1 = new StudentTests("Vlad", "Adding", DateTime.Now, 5.0);
            StudentTests stud2 = new StudentTests("Dima", "Substracting", DateTime.Now, 9.0);
            StudentTests stud3 = new StudentTests("Vova", "Multiplying", DateTime.Now, 2.0);
            StudentTests stud4 = new StudentTests("Oleg", "Adding", DateTime.Now, 11.5);

            binaryTree.Add(stud1);
            binaryTree.Add(stud2);
            binaryTree.Add(stud3);
            binaryTree.Add(stud4);

            binaryTree.PrintTree();

            binaryTree.TraversalOrder = TraversalMode.PostOrder;
            foreach (var value in binaryTree)
            {
                Console.WriteLine($"Value:{value}");
            }



            Console.ReadKey();
        }
    }
}
