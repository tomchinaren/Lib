using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Tom.Lib.Observers
{
    /// <summary>
    /// 基于本地内存队列的订阅模式任务运行器
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class ObserverTaskRunner<TMessage> : IDequeuable<TMessage>
    {
        // 消息队列
        ConcurrentQueue<TMessage> messageQueue = new ConcurrentQueue<TMessage>();

        // 可被订阅的subject
        IObservable<TMessage> observable;

        // 订阅者（仅一个订阅者）
        AbstractTaskObserver<TMessage> observer;

        // 数据读取器
        IReader<TMessage> reader;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ObserverTaskRunner(IObservable<TMessage> observable, AbstractTaskObserver<TMessage> observer, IReader<TMessage> reader)
        {
            this.observable = observable;
            this.observer = observer;
            this.reader = reader;

            // 订阅
            this.observable.Subscribe(observer);

            // 设置订阅者可出列对象
            observer.SetDequeuable(this);
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Run()
        {
            // 循环处理任务
            while (true)
            {
                // 获取下一批消息
                var hasMessage = reader.ReadNext(out var message);

                // 没有消息，等待并再次尝试取消息
                if (!hasMessage)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                // 消息入列
                messageQueue.Enqueue(message);

                // 通知
                Notify(message);
            }
        }

        public bool TryDequeue(out TMessage message)
        {
            return messageQueue.TryDequeue(out message);
        }

        /// <summary>
        /// 通知
        /// </summary>
        protected void Notify(TMessage message)
        {
            // 仅一个订阅者
            observer.OnNext(message);
        }


    }



    public interface IReader<TData>
    {
        bool ReadNext(out TData data);
    }


    public interface IDequeuable<T>
    {
        bool TryDequeue(out T message);
    }
}
