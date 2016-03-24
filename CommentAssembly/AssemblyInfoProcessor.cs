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
        public AssemblyInfoProcessor(TextReader reader)
        {
            using (reader)
            {
                bool versionLoaded = false;
                while (true)
                {
                    string line = reader.ReadLine();                    
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
                    result = new AssemblyInfoProcessor(reader);
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
