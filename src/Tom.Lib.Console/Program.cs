using System;
using Tom.Lib.Observers;

namespace Tom.Lib.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----BEGIN");

            // TestReadWritingCluster();

            TestObserverTaskRunner.Run();


            Console.WriteLine("press ENTER to exit");
            Console.ReadLine();
            Console.WriteLine("-----END");
        }

        static void TestReadWritingCluster()
        {
            Clusters.IReadWritingHandler handler = new Clusters.ReadWritingHandlerDemo();
            var cluster = new Clusters.ReadWritingCluster(handler);
            cluster.Start();
        }

    }
}
