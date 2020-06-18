using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tom.Lib.Tasks
{
    public class TaskSpliter
    {
        public static void Run<T>(int taskCount, List<T> models, Action<List<T>> action)
        {
            if (taskCount <= 0)
            {
                throw new Exception($"taskCount有误:{taskCount}");
            }

            // 拆分models到各task
            var splitedModels = SplitModels(taskCount, models);
            if (splitedModels.Count == 0)
            {
                return;
            }

            var tasks = new List<Task>();
            foreach(var tmodels in splitedModels)
            {
                var task = Task.Run(() => {
                    var obj = tmodels;
                    action(obj);
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
        }

        public static List<TOut> Run<T, TOut>(int taskCount, List<T> models, Func<List<T>, TOut> func)
        {
            if (taskCount <= 0)
            {
                throw new Exception($"taskCount有误:{taskCount}");
            }

            // 拆分models到各task
            var splitedModels = SplitModels(taskCount, models);
            if (splitedModels.Count == 0)
            {
                return null;
            }

            var tasks = new List<Task>();
            var resluts = new ConcurrentBag<TOut>();
            foreach (var tmodels in splitedModels)
            {
                var task = Task.Run(() =>
                {
                    var obj = tmodels;
                    var flag = func(obj);
                    resluts.Add(flag);
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
            return resluts.ToList();
        }

        protected static List<List<T>> SplitModels<T>(int taskCount, List<T> models)
        {
            var splitedModels = new List<List<T>>();
            if (taskCount == 0)
            {
                throw new Exception($"taskCount有误:{taskCount}");
            }

            // 每task要运行的model数量
            var perCount = models.Count / taskCount;

            // 最后一个task的model数量加上余数
            var lastCount = perCount + (models.Count % taskCount);

            if (perCount == 0 && lastCount == 0)
            {
                return splitedModels;
            }

            if(perCount==0 && lastCount > 0)
            {
                taskCount = 1;
            }

            for (var i = 0; i < taskCount; i++)
            {
                var beginIndex = i * perCount;
                var fetchCount = perCount;

                // 最后一个task
                if (i == taskCount - 1)
                {
                    fetchCount = lastCount;
                }

                var localModels = models.GetRange(beginIndex, fetchCount);
                splitedModels.Add(localModels);
            }

            return splitedModels;
        }
    }

}
