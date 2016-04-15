//-----------------------------------------------------------------------
// <copyright file="AssemblyInfoProcessor.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;

    /// <summary>
    /// Processes and decodes an <c>AssemblyInfo.cs</c> loading the information needed by the application, namely the
    /// version and a list of the most recent commends and compilation time stamps.
    /// </summary>
    public class AssemblyInfoProcessor
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
        /// Prefix string of a line that delimitates the to-do start area in the AssemblyInfo.cs file
        /// </summary>
        private static readonly string StartOfTodoSignature = "[assembly: AssemblyCulture(";

        /// <summary>
        /// Prefix string for the first line of a new to-do in the AssemblyInfo.cs file
        /// </summary>
        private static readonly string StartOfTodoContent = "// TODO [";

        /// <summary>
        /// Prefix string for the following lines of the already started to-do's in the AssemblyInfo.cs file
        /// </summary>
        private static readonly string StartOfTodoFollowing = "// TODO";

        /// <summary>
        /// Ending string for the to-do zone in the AssemblyInfo.cs file
        /// </summary>
        private static readonly string EndOfTodos = "// ENDTODO";

        /// <summary>
        /// Number of lines containing the latest compilation comments to be loaded from <c>AssemblyInfo.cs</c>
        /// file.
        /// </summary>
        private static readonly int MaxComments = 50;

        /// <summary>
        /// Name of the folder containing the <c>AssemblyInfo.cs</c> file.
        /// </summary>
        private static readonly string Properties = "Properties";

        /// <summary>
        /// Backup field of the LastComments property.
        /// </summary>
        private List<string> lastComments = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfoProcessor" /> class.<para/>
        /// </summary>
        /// <param name="reader">TextReader of the <c>AssemblyInfo.cs</c> formatted file.</param>
        /// <param name="todoList">Class responsible for to-do storage</param>
        public AssemblyInfoProcessor(TextReader reader, ITodoList todoList = null)
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
        }

        /// <summary>
        /// Gets or sets the version contained in the file.
        /// </summary>
        public AssemblyVersion CurrentVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the project containing this AssemblyInfo;
        /// </summary>
        public string ProjectName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets an enumeration of the most recent comment lines inside the <c>AssemblyInfo</c> file.
        /// </summary>
        public IEnumerable<string> LastComments
        {
            get
            {
                return this.lastComments;
            }
        }

        /// <summary>
        /// Factory method that creates an AssemblyInfo object with the information loaded from the passed file.
        /// </summary>
        /// <param name="projectFolder">Folder containing the solution project <c>.sln</c></param>
        /// <returns>An AssemblyInfo object with the loaded information.</returns>
        public static AssemblyInfoProcessor LoadAssemblyInfo(string projectFolder)
        {
            string filePath = Path.Combine(projectFolder, Properties, AssemblyInfoName);

            AssemblyInfoProcessor result = null;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    TodoManager todoManager = new TodoManager();
                    result = new AssemblyInfoProcessor(reader, todoManager);
                }

                string[] parsedPath = Path.GetFullPath(filePath).Split(Path.DirectorySeparatorChar);
                result.ProjectName = parsedPath[parsedPath.Length - 3];
            }

            return result;
        }

        /// <summary>
        /// Updates the assembly info file with the new information passed, i.e. a new version number
        /// and a new comment.
        /// </summary>
        /// <param name="projectFolder">Pathname of the folder containing the project <c>.sln</c> file</param>
        /// <param name="version">Version to be inserted in the file.</param>
        /// <param name="comment">Comment about the current compilation.</param>
        public static void UpdateAssemblyInfo(string projectFolder, AssemblyVersion version, string comment)
        {
            string filePath = Path.Combine(projectFolder, Properties, AssemblyInfoName);
            string backupPath = filePath + ".bak";
            bool loadingTodoList = false;
            try
            {
                File.Copy(filePath, backupPath, true);

                using (TextReader reader = new StreamReader(backupPath))
                {
                    using (TextWriter writer = new StreamWriter(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
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
                                writer.WriteLine();

                                writer.WriteLine("//// " + version + "  Compiled " + DateTime.Now);
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
                                loadingTodoList = true;
                            }
                            else if (loadingTodoList)
                            {
                                if (line.StartsWith(EndOfTodos))
                                {
                                    loadingTodoList = false;

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
                                }
                            }
                            else
                            {
                                writer.WriteLine(line);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible to write AssemblyInfo file");
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
