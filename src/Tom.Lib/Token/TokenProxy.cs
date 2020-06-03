namespace Tom.Lib.Token
{
    public class TokenProxy: IToken
    {
        #region singleton

        public static TokenProxy Instance { get; private set; }
        static TokenProxy()
        {
            Instance = new TokenProxy();
        }
        private TokenProxy()
        {
        }

        #endregion

        IToken token = new DemoToken();

        public string GetToken()
        {
            return token.GetToken();
        }
    }

    public class DemoToken : AbstractToken
    {
        protected override string RefalshToken()
        {
            return "it's a demo";
        }
    }
}
