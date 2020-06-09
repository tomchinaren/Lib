using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tom.Lib.Test.Utils;

namespace Tom.Lib.Test
{
    [TestClass]
    public class UnitTest1
    {
        private List<User> GetUsers()
        {
            var users = new List<User>();
            var i = 0;
            while (i < 100)
            {
                users.Add(new User { Id = i, Name = "tom" + i });
                i++;
            }
            return users;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var users = GetUsers();

            var allHandDocs = new ConcurrentBag<List<string>>();

            Tasks.TaskSpliter.Run<User>(10, users, t => {
                allHandDocs.Add(users.Select(s => $"{s.Id}.{s.Name}").ToList());
            });

            Assert.IsTrue(allHandDocs.Count == 10);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(allHandDocs));
        }

        [TestMethod]
        public void TestMethod_ZeroTaskCount()
        {
            var users = GetUsers();

            var allHandDocs = new ConcurrentBag<List<string>>();

            try
            {
                Tasks.TaskSpliter.Run<User>(0, users, t =>
                {
                    allHandDocs.Add(users.Select(s => $"{s.Id}.{s.Name}").ToList());
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            Assert.IsTrue(allHandDocs.Count == 0);
        }
    }
}
