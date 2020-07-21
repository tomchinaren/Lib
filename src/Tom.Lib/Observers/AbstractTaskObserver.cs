using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tom.Lib.Observers
{
    public abstract class AbstractTaskObserver<TMessage> : IObserver<TMessage>
    {
        Task task;
        IDequeuable<TMessage> dequeuable;

        public AbstractTaskObserver()
        {

            Run();
        }

        /// <summary>
        /// 设置订阅者可出列对象
        /// </summary>
        /// <param name="dequeuable"></param>
        public void SetDequeuable(IDequeuable<TMessage> dequeuable)
        {
            this.dequeuable = dequeuable;
        }

        /// <summary>
        /// 定时执行
        /// </summary>
        protected void Run()
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));

            task = Task.Run(() => {
                while (true)
                {
                    try
                    {
                        while (dequeuable.TryDequeue(out TMessage data))
                        {
                            HandleData(data);
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


        protected abstract void HandleData(TMessage data);

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(TMessage value)
        {
            // 忽略通知，使用定时异步出列
        }
    }
}
