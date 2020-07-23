using System.Threading;

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


}
