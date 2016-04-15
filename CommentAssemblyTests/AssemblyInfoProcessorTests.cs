using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentAssemblyTests
{
    using CommentAssembly;
    using NUnit.Framework;
    using System.IO;

    [TestFixture]
    class AssemblyInfoProcessorTests
    {
        private class TestResults
        {
            public bool IsAppend;
            public bool IsDone;
            public string Content;

            public TestResults(bool isAppend, bool isDone, string content)
            {
                this.IsAppend = isAppend;
                this.IsDone = isDone;
                this.Content = content;
            }

            public override bool Equals(object obj)
            {
                TestResults other = obj as TestResults;
                if (other == null)
                {
                    return false;
                }
                return this.IsAppend == other.IsAppend && this.IsDone == other.IsDone && this.Content == other.Content;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        };

        private static TestResults[] testResults = new TestResults[]
        {
            new TestResults(false, false, "todo1"),
            new TestResults(true, false, "todo1.1"),
            new TestResults(false, true, "done")
        };
        
        private class TodoListTest : ITodoList
        {
            int counter = 0;
            public TodoListTest ()
            {
            }

            void ITodoList.AddTodo(bool done, string content)
            {
                TestResults tr = testResults[counter++];
                Assert.AreEqual(tr, new TestResults(false, done, content));
            }

            void ITodoList.AppendLineTodo(string content)
            {
                TestResults tr = testResults[counter++];
                Assert.AreEqual(tr, new TestResults(true, false, content));
            }
        }

        [Test]
        public void AssemblyInfoProcessor_LoadsTheTodoList_WhenSubmittedGoodAssemblyInfo()
        {
            TodoListTest tester = new TodoListTest();
            TextReader tr = new StringReader(infoFile);
            new AssemblyInfoProcessor(tr, new TodoListTest());
        }

        private static string infoFile =
            "[assembly: AssemblyCulture(\"\")]\n" +
            "// TODO [ ] todo1\n" +
            "// TODO todo1.1\n" +
            "// TODO [X] done\n" +
            "[assembly: AssemblyVersion(\"1.0.0.41\")]\n" +
            "[assembly: AssemblyFileVersion(\"1.0.0.41\")]\n";
    }
}
