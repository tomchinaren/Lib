using System;

namespace Tom.Lib.Token
{

    public abstract class AbstractKeyToken : IKeyToken
    {
        // 过期间隔
        private readonly TimeSpan expiredSpan = TimeSpan.FromMinutes(30);
        // 过期时间
        private DateTime expiredTime => DateTime.Now.Add(expiredSpan);
        // 提前过期时间
        private DateTime preExpiredTime => expiredTime.AddMinutes(-1);
        private bool isExpired => DateTime.Now > preExpiredTime;

        // token
        private string token;
        /// <summary>
        /// 获取token
        /// </summary>
        public string GetToken(string key)
        {
            if (isExpired)
            {
                token = RefalshToken(key);
            }
            return token;
        }
        protected abstract string RefalshToken(string key);
    }
}
