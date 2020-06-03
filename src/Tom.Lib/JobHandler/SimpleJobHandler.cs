namespace Tom.Lib.JobHandler
{
    // v1
    public abstract class SimpleJobHandler : IJobHandler
    {
        private bool isRunning;

        protected abstract void RunInner();

        public virtual void Run(int interval = 30000)
        {
            var timer = new System.Timers.Timer
            {
                Enabled = true,
                Interval = interval
            };

            timer.Elapsed += (sender, e) =>
            {
                // 已运行则跳过
                if (isRunning)
                {
                    return;
                }

                try
                {
                    // 标记正在运行
                    isRunning = true;

                    // 执行
                    RunInner();
                }
                finally
                {
                    // 标记未运行
                    isRunning = false;
                }
            };

            timer.Start();
        }

    }
}
