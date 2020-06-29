using System;

namespace Tom.Lib.Switches
{
    /// <summary>
    /// 切换开关
    /// </summary>
    public class TSwitch
    {
        private int minSwitch;
        private int maxSwitch;
        private int totalSwitch;
        private int switchCounter;
        private int triggerCounter;
        Action<int> switchNotify;

        public int CurrentSwitch { get; set; }

        public TSwitch(int maxSwitch, int initSwitch, int triggerCounter, Action<int> switchNotify)
        {
            this.minSwitch = 0;

            if (initSwitch > maxSwitch || initSwitch < minSwitch || minSwitch >= maxSwitch)
            {
                throw new System.ArgumentOutOfRangeException("firstSwitch out of range");
            }
            if (triggerCounter <= 0)
            {
                throw new System.ArgumentOutOfRangeException("triggerCounter must gt 0");
            }

            this.maxSwitch = maxSwitch;
            this.totalSwitch = maxSwitch - minSwitch + 1;

            CurrentSwitch = initSwitch;
            this.switchCounter = 0;
            this.triggerCounter = triggerCounter;
            this.switchNotify = switchNotify;
        }

        public int IncreaseCounter()
        {
            switchCounter++;

            TrySwitch();

            return switchCounter;
        }

        private void TrySwitch()
        {
            if (switchCounter >= triggerCounter)
            {
                CurrentSwitch = (CurrentSwitch + 1) % totalSwitch;
                switchCounter = 0;

                // 通知
                switchNotify?.Invoke(CurrentSwitch);
            }
        }

    }

}
