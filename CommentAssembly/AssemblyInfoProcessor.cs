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
    public static class AssemblyInfoProcessor
    {
        /// <summary>
        /// Factory method that creates an AssemblyInfo object with the information loaded from the passed file.
        /// </summary>
        /// <param name="projectFolder">Folder containing the solution project <c>.sln</c></param>
        /// <returns>An AssemblyInfo object with the loaded information.</returns>
        public static IInfoProcessor LoadAssemblyInfo(string projectFolder, IInfoProcessor iinfoProcessor)
        {
            string filePath = Path.Combine(projectFolder, iinfoProcessor.RelativeInfoFilePath);

            IInfoProcessor result = null;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    TodoManager todoManager = new TodoManager();
                    result = iinfoProcessor.LoadAssemblyInfo(reader, todoManager);
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
        public static void UpdateAssemblyInfo(string projectFolder, AssemblyVersion version, string comment, IInfoProcessor iinfoProcessor)
        {
            string filePath = Path.Combine(projectFolder, iinfoProcessor.RelativeInfoFilePath);
            string backupPath = filePath + ".bak";
            try
            {
                File.Copy(filePath, backupPath, true);

                using (TextReader reader = new StreamReader(backupPath))
                {
                    using (TextWriter writer = new StreamWriter(filePath))
                    {
                        string line;
                        iinfoProcessor.InitLoading();
                        while ((line = reader.ReadLine()) != null)
                        {
                            iinfoProcessor.ProcessLine(writer, line, version, comment);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible to write AssemblyInfo file");
            }
        }
    }
}
