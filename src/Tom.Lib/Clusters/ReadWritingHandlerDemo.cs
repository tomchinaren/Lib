using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tom.Lib.Clusters
{
    public class ReadWritingHandlerDemo : IReadWritingHandler
    {
        ConcurrentQueue<List<int>> queue = new ConcurrentQueue<List<int>>();

        public void AllStop()
        {
            // stop                            
            Console.WriteLine("all stopped");
        }

        public void Read(INode node)
        {
            var guid = Guid.NewGuid().GetHashCode();

            // read
            while (true)
            {
                Thread.Sleep(1000);

                if (queue.Count() > 10)
                {
                    Console.WriteLine("read too more,count: " + queue.Count());
                    continue;
                }

                if (node.IsStopped)
                {
                    Console.WriteLine("return read when IsStopped");
                    return;
                }

                var rand = new Random(guid);
                var list = new List<int>();
                list.Add(rand.Next(100));
                list.Add(rand.Next(100));
                list.Add(rand.Next(100));

                queue.Enqueue(list);

                Console.WriteLine("read: " + string.Join(",", list));
            }
        }

        public void Write(INode node)
        {
            // write
            while (true)
            {
                Thread.Sleep(2000);

                if (node.IsStopped)
                {
                    Console.WriteLine("return write when IsStopped");
                    return;
                }

                var list = new List<int>();

                if (queue.TryDequeue(out var oneList))
                {
                    list.AddRange(oneList);

                    Console.WriteLine("write: " + string.Join(",", list));
                }
            }
        }
    }


}
