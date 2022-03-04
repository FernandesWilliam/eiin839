using System;

namespace ExeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
                Console.WriteLine("<html><body> Hello " + args[0] + " et " + args[1] + "</body></html>");
            else
                Console.WriteLine("ExeTest <string parameter>");
        }
    }
}
