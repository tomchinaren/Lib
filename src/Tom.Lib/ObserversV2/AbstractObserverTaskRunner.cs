using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tom.Lib.ObserversV2
{
    /// <summary>
    /// 基于本地内存队列的订阅模式任务运行器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractObserverTaskRunner<T>
    {
        // 观察者
        DefaultObservable<T> observable = new DefaultObservable<T>();

        // 订阅者
        AbstractTaskObserver<T> observer = null;

        // 读入消息
        public abstract bool ReadNext(out T data);

        public void Run()
        {
            // 订阅
            observable.Subscribe(observer);

            while (true)
            {
                // 读取消息的，如果没有消息，则等待并再次尝试读取消息
                if (!ReadNext(out T data)){
                    Thread.Sleep(1000);
                    continue;
                }

                // 发布消息，订阅者将定时自动拉取数据
                observable.Publish(data);
            }
        }
    }

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
            foreach(var observer in observers)
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


    /// <summary>
    /// 扩展IObserver，以支持设置observalbe
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractTaskObserver<T> : IObserver<T>
    {
        // 观察者，用于主动拉取数据
        DefaultObservable<T> observable;

        public AbstractTaskObserver(DefaultObservable<T> observable)
        {

            this.observable = observable;
        }

        // 设置观察者，以便主动拉取数据
        public void SetObservable(DefaultObservable<T> observable)
        {
        }


        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(T value)
        {
            // 忽略通知，使用定时异步出列
        }


        /// <summary>
        /// 定时执行
        /// </summary>
        protected void Run()
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));

            var task = Task.Run(() => {
                while (true)
                {
                    try
                    {
                        while (observable.TryDequeue(out T data))
                        {
                            HandleMessage(data);
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO
                    }

                    // 队列没有数据时，休眠1秒后继续尝试出列
                    Thread.Sleep(1000);
                }
            });
        }

        protected abstract void HandleMessage(T message);

    }


}
