//-----------------------------------------------------------------------
// <copyright file="CSharpInfoProcessor.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
using System;
using System.IO;
using System.Text;

namespace CommentAssembly
{
    /// <summary>
    /// It is frequently needed when developing for Windows, to use RC (Resource) files where the version is read and shown in the properties window
    /// of an executable.
    /// This <see cref="InfoProcessor"/> implements the CommentAssembly with an RC file format.
    /// https://docs.microsoft.com/en-us/windows/desktop/menurc/about-resource-files 
    /// </summary>
    public class RCInfoProcessor : AbstractInfoProcessor
    {

        /// <summary>
        /// Prefix string of a line that delimitates the to-do start area in the AssemblyInfo.cs file
        /// </summary>
        private static readonly string StartOfTodoSignature = "// STARTTODO";

        /// <summary>
        /// Beginning of the line containing the Assembly version in the <c>Version.rc</c> file.
        /// </summary>
        private static readonly string AssemblyVersionSignature = "#define VER_FILEVERSION";

        /// <summary>
        /// Beginning of the line containing the Assembly version in string format inside the <c>Version.rc</c> file.
        /// </summary>
        private static readonly string AssemblyVersionSignatureString = "#define THEFILEVERSION";

        /// <summary>
        /// True while loading the to do list
        /// </summary>
        private bool loadingTodoList;

        public override Encoding WriteEncoding => Encoding.ASCII;

        /// <summary>
        /// Gets the relative file path with respect to the folder whence the program is launched
        /// </summary>
        public override string RelativeInfoFilePath
        {
            get
            {
                return "Version.rc";
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
                            string[] versionNumbers = line.Substring(AssemblyVersionSignature.Length).Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            StringBuilder versionString = new StringBuilder();

                            foreach (string s in versionNumbers)
                            {
                                if (versionString.Length > 0)
                                {
                                    versionString.Append('.');
                                }

                                versionString.Append(s);
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

        private bool writtenComments;

        public override void InitLoading()
        {
            this.loadingTodoList = false;
            this.writtenComments = false;
        }

        public override void ProcessLine(TextWriter writer, string line, AssemblyVersion version, string comment)
        {
            if (line.Trim().StartsWith(AssemblyVersionSignature))
            {
                writer.WriteLine(string.Format("#define VER_FILEVERSION             {0},{1},{2},{3}", version.Major, version.Minor, version.Build, version.Revision));
                writer.WriteLine(string.Format("#define THEFILEVERSION             \"{0}.{1}.{2}.{3}\\0\"", version.Major, version.Minor, version.Build, version.Revision));
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
            else if (line.StartsWith(AssemblyVersionSignatureString))
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
                if (!this.writtenComments && line.Contains("Compiled by ["))
                {
                    writer.WriteLine("// " + version + "  Compiled by [" + App.TheUser + "] " + DateTime.Now);
                    if (comment != null)
                    {
                        foreach (string commentLine in comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            writer.WriteLine("// " + commentLine);
                        }
                    }

                    this.writtenComments = true;
                }
                writer.WriteLine(line);
            }
        }
    }
}
