namespace CommentAssemblyTests
{
    using CommentAssembly;
    using NUnit.Framework;
    using System.IO;

    class PascalInfoProcessorTests
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
            public TodoListTest()
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
        public void PascalInfoProcessor_LoadsTheTodoList_WhenSubmittedGoodAssemblyInfo()
        {
            TextReader tr = new StringReader(infoFile);
            IInfoProcessor assemblyInfo = new PascalInfoProcessor();
            assemblyInfo.LoadAssemblyInfo(tr, new TodoListTest());
        }


        private static string infoFile =
            "unit uVersion;\n" +
            "// TODO [ ] todo1\n" +
            "// TODO todo1.1\n" +
            "// TODO [X] done\n" +
            "// ENDTODO\n" +
            "\n" +
            "interface\n" +
            "type\n" +
            "TVersion = class\n" +
                "private\n" +
                "class var FVersion : string;\n" +
                "class var FVersionDebug : integer;\n" +
                "public\n" +
                "class function get : string;\n" +
                "class function getDebug : string;\n" +
                "class function getAppName : string;\n" +
            "end;\n" +
            "\n" +
            "const\n" +
            "AppName : string = 'APS-Builder ';\n" +
            "\n" +
            "implementation\n" +
            "uses Sysutils;\n" +
            "class function TVersion.get : string;\n" +
            "begin\n" +
            "result := TVersion.FVersion;\n" +
            "end;\n" +
            "\n" +
            "class function TVersion.getDebug : string;\n" +
            "begin\n" +
            "result := get + '.' + inttostr(TVersion.FversionDebug);\n" +
            "end;\n" +
            "\n" +
            "class function TVersion.getAppName: string;\n" +
            "begin\n" +
            "result := AppName;\n" +
            "end;\n" +
            "\n" +
            "initialization\n" +
            "TVersion.FVersion := '0.99';  TVersion.FVersionDebug := 1235;\n" +
            "//  TVersion.FVersion := '0.99';  TVersion.FVersionDebug := 1234;\n" +
            "\n" +
            "end.";

    }
}
