//-----------------------------------------------------------------------
// <copyright file="CSharpInfoProcessor.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Takes care of the low level details of decoding the AssemblyInfo file.
    /// </summary>
    public class CSharpInfoProcessor : AbstractInfoProcessor
    {
        /// <summary>
        /// Beginning of the line containing the File version in the <c>AssemblyInfo.cs</c> file.
        /// </summary>
        private static readonly string AssemblyFileVersionSignature = "[assembly: AssemblyFileVersion(";

        /// <summary>
        /// File name of the <c>AssemblyInfo.cs</c> file
        /// </summary>
        private static readonly string AssemblyInfoName = "AssemblyInfo.cs";

        /// <summary>
        /// Beginning of the line containing the Assembly version in the <c>AssemblyInfo.cs</c> file.
        /// </summary>
        private static readonly string AssemblyVersionSignature = "[assembly: AssemblyVersion(";

        /// <summary>
        /// Name of the folder containing the <c>AssemblyInfo.cs</c> file.
        /// </summary>
        private static readonly string Properties = "Properties";

        /// <summary>
        /// Prefix string of a line that delimitates the to-do start area in the AssemblyInfo.cs file
        /// </summary>
        private static readonly string StartOfTodoSignature = "[assembly: AssemblyCulture(";


        /// <summary>
        /// True while loading the to do list
        /// </summary>
        private bool loadingTodoList;

        /// <summary>
        /// Gets the relative file path with respect to the folder whence the program is launched
        /// </summary>
        public override string RelativeInfoFilePath
        {
            get
            {
                return Path.Combine(Properties, AssemblyInfoName);
            }
        }

        /// <summary>
        /// Loads the info of the assembly from the AssemblyInfo.cs file.
        /// </summary>
        /// <param name="reader">TextReader of the <c>AssemblyInfo.cs</c> formatted file.</param>
        /// <param name="todoList">Class responsible for to-do storage</param>
        /// <returns>The <see cref="IInfoProcessor"/> instance where the loaded information is stored.</returns>
        public override IInfoProcessor LoadAssemblyInfo(TextReader reader, ITodoList todoList)
        {
            using (reader)
            {
                int lineNumber = 0;
                bool versionLoaded = false;
                bool todoLoaded = false;
                bool todoLoading = false;

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
                            string versioString = line.Substring(
                                AssemblyVersionSignature.Length + 1,
                                line.IndexOf('"', AssemblyVersionSignature.Length + 1) - AssemblyVersionSignature.Length - 1);
                            this.CurrentVersion = new AssemblyVersion(versioString);
                        }
                        else
                        {
                            throw new FileFormatException("Contains more than one readable version");
                        }

                        versionLoaded = true;
                    }
                    else
                    {
                        if (line.StartsWith(AssemblyFileVersionSignature))
                        {
                            continue;
                        }

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
            }

            return this;
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
            if (line.StartsWith(AssemblyVersionSignature))
            {
                writer.Write(AssemblyVersionSignature);
                writer.Write('"');
                writer.Write(version);
                writer.WriteLine("\")]");
            }
            else if (line.StartsWith(AssemblyFileVersionSignature))
            {
                writer.Write(AssemblyFileVersionSignature);
                writer.Write('"');
                writer.Write(version);
                writer.WriteLine("\")]");

                writer.WriteLine("//// " + version + "  Compiled by [" + App.TheUser+"] " + DateTime.Now);
                if (comment != null)
                {
                    foreach (string commentLine in comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        writer.WriteLine("//// " + commentLine);
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

        /// <summary>
        /// Computes a human readable representation of the class.
        /// </summary>
        /// <returns>A string containing the version.</returns>
        public override string ToString()
        {
            return this.CurrentVersion.ToString();
        }
    }
}