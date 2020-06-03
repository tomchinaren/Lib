namespace Tom.Lib.Token
{
    public class KeyTokenProxy: IKeyToken
    {
        #region singleton

        public static KeyTokenProxy Instance { get; private set; }
        static KeyTokenProxy()
        {
            Instance = new KeyTokenProxy();
        }
        private KeyTokenProxy()
        {
        }

        #endregion

        IKeyToken token = new DemoKeyToken();

        public string GetToken(string key)
        {
            return token.GetToken(key);
        }
    }

    public class DemoKeyToken : AbstractKeyToken
    {
        protected override string RefalshToken(string key)
        {
            return "it's a demo";
        }
    }
}
