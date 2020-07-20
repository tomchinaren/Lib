using System.Collections.Generic;
using Tom.Lib.Po;

namespace Tom.Lib.Observers
{
    public class DemoUserSet
    {
        public int SetId { get; set; }
        public List<User> Users { get; set; }
        public int State { get; set; }
    }
}
