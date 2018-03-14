//-----------------------------------------------------------------------
// <copyright file="PascalInfoProcessor.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommentAssembly
{
    public class PascalInfoProcessor : AbstractInfoProcessor
    {
        /// <summary>
        /// Prefix string of a line that delimitates the to-do start area in the AssemblyInfo.cs file
        /// </summary>
        private static readonly string StartOfTodoSignature = "unit";

        /// <summary>
        /// Beginning of the line containing the Assembly version in the <c>AssemblyInfo.cs</c> file.
        /// </summary>
        private static readonly string AssemblyVersionSignature = "TVersion.FVersion";

        /// <summary>
        /// True while loading the to do list
        /// </summary>
        private bool loadingTodoList;

        public override string RelativeInfoFilePath
        {
            get
            {
                return "uVersion.pas";
            }
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
                            int firstAssignment = line.IndexOf(":=");
                            if (firstAssignment < 0)
                            {
                                throw new FileFormatException("The version format is wrong");
                            }

                            int secondAssignment = line.IndexOf(":=", firstAssignment + 2);
                            if (secondAssignment < 0)
                            {
                                throw new FileFormatException("The version format is wrong");
                            }

                            /// Here we get the version in numeral format in what is between first assignment and the first ;
                            StringBuilder versionString = new StringBuilder();
                            for (int i=firstAssignment; i<secondAssignment; i++)
                            {
                                char c = line[i];
                                if (c==';')
                                {
                                    break;
                                }
                                if (c=='.' || Char.IsDigit(c))
                                {
                                    versionString.Append(c);
                                }
                            }

                            // in Pascal the thirt version number is 0 and the last one is the debug.
                            versionString.Append(".0.");

                            for (int i=secondAssignment; i<line.Length; i++)
                            {
                                char c = line[i];
                                if (c == ';')
                                {
                                    break;
                                }
                                if (c == '.' || Char.IsDigit(c))
                                {
                                    versionString.Append(c);
                                }
                            }
                            this.CurrentVersion = new AssemblyVersion(versionString.ToString());
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
        /// Must be called before starting loading the LoadAssembly info.
        /// </summary>
        public override void InitLoading()
        {
            this.loadingTodoList = false;
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
                writer.WriteLine(string.Format("  TVersion.FVersion := '{0}.{1}'; TVersion.FVersionDebug := {2};", version.Major, version.Minor, version.Revision));
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