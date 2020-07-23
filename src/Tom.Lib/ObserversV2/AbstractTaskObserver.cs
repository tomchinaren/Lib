using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tom.Lib.ObserversV2
{
    /// <summary>
    /// 可开启定时执行的订阅者，支持设置observalbe以便能主动拉取消息
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
        /// 开启定时执行
        /// </summary>
        public void StartAutoRun()
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
                        HandleDequeueException(ex);
                    }

                    // 队列没有数据时，休眠1秒后继续尝试出列
                    Thread.Sleep(1000);
                }
            });
        }

        protected abstract void HandleMessage(T message);

        protected abstract void HandleDequeueException(Exception ex);

    }

}
