using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tom.Lib.Clusters;
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
        public void TestMethod_StringResult()
        {
            var users = GetUsers();
            var taskCount = 10;
            var allHandDocs = new ConcurrentBag<List<string>>();

            Tasks.TaskSpliter.Run<User>(taskCount, users, t => {
                allHandDocs.Add(users.Select(s => $"{s.Id}.{s.Name}").ToList());
            });

            Assert.IsTrue(allHandDocs.Count == taskCount);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(allHandDocs));
        }

        [TestMethod]
        public void TestMethod_AllTrue()
        {
            var users = GetUsers();
            var taskCount = 10;
            var boolResults = new ConcurrentBag<bool>();

            Tasks.TaskSpliter.Run<User>(taskCount, users, t => {
                boolResults.Add(t.Count > 0);
            });

            Assert.IsTrue(boolResults.Count(t=>t) == taskCount);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(boolResults));
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
