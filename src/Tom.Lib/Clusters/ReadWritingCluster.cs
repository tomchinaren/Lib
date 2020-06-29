using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tom.Lib.Clusters
{
    public interface IReadWritingHandler
    {
        void Read(INode node);
        void Write(INode node);
        void AllStop();
    }

    public class ReadWritingCluster
    {
        INode ReadingNode { get; set; }
        INode WritingNode { get; set; }
        IReadWritingHandler handler;

        public ReadWritingCluster(IReadWritingHandler handler)
        {
            ReadingNode = new Node();
            WritingNode = new Node();

            this.handler = handler;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                ReadingNode.Start(() =>
                {
                    handler.Read(ReadingNode);
                });
            });
            Task.Run(() =>
            {
                WritingNode.Start(() =>
                {
                    handler.Write(WritingNode);
                });
            });

            Monitor(handler.AllStop);
        }

        public void Stop()
        {
            ReadingNode.Stop();
            WritingNode.Stop();
        }

        protected void Monitor(ThreadStart stop)
        {
            var interval = TimeSpan.FromSeconds(5);

            while (true)
            {
                Thread.Sleep(interval);

                if (ReadingNode.IsStopped && WritingNode.IsStopped)
                {
                    // all stopped
                    if (stop != null)
                    {
                        stop();
                    }

                    return;
                }
            }

        }

    }
}
