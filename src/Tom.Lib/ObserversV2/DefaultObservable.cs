using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Tom.Lib.ObserversV2
{
    /// <summary>
    /// 基于本地队列的观察者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultObservable<T> : IObservable<T>
    {
        // 订阅者列表
        public List<IObserver<T>> observers = new List<IObserver<T>>();

        // 本地消息队列
        ConcurrentQueue<T> messageQueue = new ConcurrentQueue<T>();

        /// <summary>
        /// 订阅
        /// </summary>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            observers.Add(observer);

            return new UnSubscriber(observers, observer);
        }


        /// <summary>
        /// 发布消息
        /// </summary>
        public void Publish(T message)
        {
            // 入列
            Enqueue(message);

            // 通知订阅者
            Notify(message);
        }

        /// <summary>
        /// 通知订阅者
        /// </summary>
        protected virtual void Notify(T message)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(message);
            }
        }

        // 入列
        protected virtual void Enqueue(T message)
        {
            messageQueue.Enqueue(message);
        }

        /// <summary>
        /// 出列
        /// </summary>
        /// <returns></returns>
        public virtual bool TryDequeue(out T result)
        {
            return messageQueue.TryDequeue(out result);
        }

        /// <summary>
        /// 获取当前队列长度，用于监控队列以免过大
        /// </summary>
        /// <returns></returns>
        public int GetQueueLength()
        {
            return messageQueue.Count;
        }


        // 内部类，用于Subscribe方法返回IDisposable
        private class UnSubscriber : IDisposable
        {
            public List<IObserver<T>> observers;
            public IObserver<T> observer;

            public UnSubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (observers != null && observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
        }

    }

}
