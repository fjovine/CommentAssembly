using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentAssembly
{
    /// <summary>
    /// Class to generate an automatic config version for the CLanguage.
    /// It needs a file called version.h tha originally has the following layout
    /// 
    /// #ifndef VERSION_H
    /// #define VERSION_H
    /// 
    ///   #define CURRENT_VERSION "0.0.1"
    /// 
    /// #endif
    /// </summary>
    public class CLanguageInfoProcessor :AbstractInfoProcessor 
    {
        /// <summary>
        /// Prefix string of a line that delimitates the to-do start area in the <c>version.h</c> file
        /// </summary>
        private static readonly string StartOfTodoSignature = "#define VERSION_H";

        /// <summary>
        /// Beginning of the line containing the Assembly version in the <c>version.h</c> file.
        /// </summary>
        private static readonly string AssemblyVersionSignature = "#define CURRENT_VERSION";

        /// <summary>
        /// True while loading the to do list
        /// </summary>
        private bool loadingTodoList;

        public override string RelativeInfoFilePath
        {
            get
            {
                return "version.h";
            }
        }

        /// <summary>
        /// Must be called before starting loading the LoadAssembly info.
        /// </summary>
        public override void InitLoading()
        {
            this.loadingTodoList = false;
        }


        public override IInfoProcessor LoadAssemblyInfo(TextReader reader, ITodoList todoList)
        {
            bool versionLoaded = false;
            bool todoLoaded = false;
            bool todoLoading = false;

            using (reader)
            {
                int lineNumber = 0;
                while (true)
                {
                    string line = reader.ReadLine();
                    lineNumber++;
                    if (line == null)
                    {
                        if (!versionLoaded)
                        {
                            throw new FileFormatException("Does not contain a readable version");
                        }
                        else
                        {
                            break;
                        }
                    }

                    line = line.Trim();
                    if (line == string.Empty)
                    {
                        continue;
                    }

                    if (line.StartsWith(TodoParam))
                    {
                        string[] elems = line.Substring(TodoParam.Length).Trim().Split('=');
                        if (elems.Length != 2)
                        {
                            throw new FileFormatException("Wrong param line format: line " + lineNumber);
                        }

                        ProgramProperty.Set(elems[0], elems[1]);
                        continue;
                    }

                    if (line.StartsWith(StartOfTodoSignature))
                    {
                        if (todoLoaded)
                        {
                            throw new FileFormatException("More than a todo zone: line" + lineNumber);
                        }

                        if (todoLoading)
                        {
                            throw new FileFormatException("More than a todo zone: line" + lineNumber);
                        }

                        todoLoading = true;
                        continue;
                    }

                    if (todoLoading && line.StartsWith(StartOfTodoFollowing))
                    {
                        if (line.StartsWith(StartOfTodoContent))
                        {
                            bool done = line[StartOfTodoContent.Length] != ' ';
                            int startOfContent = line.IndexOf(']');
                            if (startOfContent < 0)
                            {
                                throw new FileFormatException("Error in todo line: line" + lineNumber);
                            }

                            if (todoList != null)
                            {
                                todoList.AddTodo(done, line.Substring(startOfContent + 1).TrimStart());
                            }
                        }
                        else
                        {
                            if (todoList != null)
                            {
                                todoList.AppendLineTodo(line.Substring(StartOfTodoFollowing.Length).TrimStart());
                            }
                        }

                        continue;
                    }

                    if (todoLoading && line.StartsWith(EndOfTodos))
                    {
                        todoLoading = false;
                        todoLoaded = true;
                        continue;
                    }

                    if (line.StartsWith(AssemblyVersionSignature))
                    {
                        if (!versionLoaded)
                        {
                            int startVersion = line.IndexOf('\"');
                            if (startVersion < 0)
                            {
                                throw new FileFormatException("The version format is wrong");
                            }

                            int endVersion = line.IndexOf('\"', startVersion + 1);
                            if (endVersion < 0)
                            {
                                throw new FileFormatException("The version format is wrong");
                            }
                            string versionString = line.Substring(startVersion + 1, endVersion - startVersion-1);

                            this.CurrentVersion = new AssemblyVersion(versionString);
                        }
                        else
                        {
                            throw new FileFormatException("Contains more than one readable version");
                        }

                        versionLoaded = true;
                    }
                    else
                    {
                        if (versionLoaded)
                        {
                            if (this.lastComments.Count < MaxComments)
                            {
                                this.lastComments.Add(line);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                }

                return this;
            }
        }

        /// <summary>
        /// Processes each single line in the AssemblyInfo source code.
        /// </summary>
        /// <param name="writer">The writer where the new info file must be stored.</param>
        /// <param name="line">The line to be processed.</param>
        /// <param name="version">The new version number.</param>
        /// <param name="comment">The comment to add.</param>
        public override void ProcessLine(TextWriter writer, string line, AssemblyVersion version, string comment)
        {
            if (line.Trim().StartsWith(AssemblyVersionSignature))
            {
                writer.WriteLine(string.Format("  #define CURRENT_VERSION \"{0}.{1}.{2}.{3}\"", version.Major, version.Minor, version.Build, version.Revision));
                writer.WriteLine("// " + version + "  Compiled by [" + App.TheUser + "] " + DateTime.Now);
                if (comment != null)
                {
                    foreach (string commentLine in comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        writer.WriteLine("// " + commentLine);
                    }
                }
            }
            else if (line.StartsWith(StartOfTodoSignature))
            {
                writer.WriteLine(line);
                this.loadingTodoList = true;
            }
            else if (line.StartsWith(TodoParam))
            {
                return;
            }
            else if (this.loadingTodoList)
            {
                if (line.StartsWith(EndOfTodos) || (!line.StartsWith(StartOfTodoFollowing)))
                {
                    this.loadingTodoList = false;

                    ThingTodo.ForEach((todo) =>
                    {
                        string[] todoLines = todo.Description.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        writer.Write(StartOfTodoContent);
                        writer.Write(todo.IsDone ? 'X' : ' ');
                        writer.Write("] ");
                        writer.WriteLine(todoLines[0]);
                        if (todoLines.Length > 1)
                        {
                            for (int i = 1; i < todoLines.Length; i++)
                            {
                                writer.Write(StartOfTodoFollowing);
                                writer.Write(' ');
                                writer.WriteLine(todoLines[i]);
                            }
                        }
                    });

                    writer.WriteLine(EndOfTodos);
                    ProgramProperty.ForEach((property, value) =>
                    {
                        writer.Write(TodoParam);
                        writer.Write(' ');
                        writer.Write(property);
                        writer.Write('=');
                        writer.WriteLine(value);
                    });
                }
                else
                {
                    this.loadingTodoList = line.StartsWith(StartOfTodoFollowing);
                }
            }
            else
            {
                writer.WriteLine(line);
            }
        }
    }
}
