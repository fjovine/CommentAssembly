namespace CommentAssembly
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;

    public class AssemblyInfo
    {
        private static readonly string AssemblyFileVersionSignature = "[assembly: AssemblyFileVersion(";
        private static readonly string AssemblyInfoName = "AssemblyInfo.cs";
        private static readonly string AssemblyVersionSignature = "[assembly: AssemblyVersion(";
        private static readonly int MaxComments = 10;
        private static readonly string Properties = "Properties";
        private List<string> lastComments = new List<string>();

        public AssemblyInfo(TextReader reader)
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

        public AssemblyVersion CurrentVersion
        {
            get;
            set;
        }

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
        /// <param name="projectFolder">Folder containing the solution project .sln</param>
        /// <returns>An AssemblyInfo object with the loaded information.</returns>
        public static AssemblyInfo LoadAssemblyInfo(string projectFolder)
        {
            string filePath = Path.Combine(projectFolder, Properties, AssemblyInfoName);

            AssemblyInfo result = null;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    result = new AssemblyInfo(reader);
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the assembly info file with the new information passed, i.e. a new version number
        /// and a new comment.
        /// </summary>
        /// <param name="projectFolder">Pathname of the folder containing the project .sln file</param>
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
