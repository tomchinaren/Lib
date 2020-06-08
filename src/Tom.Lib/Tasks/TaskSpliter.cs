using System;
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
            }

            Task.WaitAll(tasks.ToArray());
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

            // 最后一个task的model数量（有余则取余，无余取平均）
            var lastCount = (models.Count % taskCount > 0 ? models.Count % taskCount : perCount);

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
