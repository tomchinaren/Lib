using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tom.Lib.Observers;
using Tom.Lib.Po;

namespace Tom.Lib.ConsoleTest
{
    public class TestObserverTaskRunner
    {
        public static void Run()
        {
            DefaultObservable<User> observable = new DefaultObservable<User>();
            TaskObserver<User> observer = new DemoTaskObserver();
            IReader<User> reader = new DemoReader();
            var runner = new ObserverTaskRunner<User>(observable, observer, reader);

            // 开始运行
            runner.Run();
        }

        private class DemoTaskObserver : TaskObserver<User>
        {
            protected override void HandleData(User data)
            {
                Console.WriteLine($"{DateTime.Now.ToShortTimeString()} DemoClient.HandleData userId: {data.Id}");
            }
        }

        private class DemoReader : IReader<User>
        {
            List<User> list = new List<User>();
            int index = -1;

            public DemoReader()
            {
                Task.Run(() => {
                    while (true)
                    {
                        Thread.Sleep(50);

                        // 模拟添加用户
                        var rand = new Random(DateTime.Now.Millisecond);
                        var id = rand.Next(1000);
                        var user = new User
                        {
                            Id = id,
                            Name = id.ToString()
                        };
                        list.Add(user);

                        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} DemoReader 添加数据 userId={user.Id}");
                    }
                });
            }

            public bool ReadNext(out User data)
            {
                if ((index + 1) >= 0 && (index + 1) < list.Count)
                {
                    index++;
                    data = list[index];
                    return true;
                }

                data = null;
                return false;
            }
        }
    }
}
