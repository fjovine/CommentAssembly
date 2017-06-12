//-----------------------------------------------------------------------
// <copyright file="IInfoProcessor.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Every source language decoder must implement this interface.
    /// </summary>
    public interface IInfoProcessor
    {
        /// <summary>
        /// Gets or sets the current version of the assembly.
        /// </summary>
        AssemblyVersion CurrentVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the project
        /// </summary>
        string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets an enumeration of the last comments.
        /// </summary>
        IEnumerable<string> LastComments
        {
            get;
        }

        /// <summary>
        /// Gets the relative file path with respect to the folder whence the program is launched
        /// </summary>
        string RelativeInfoFilePath
        {
            get;
        }

        /// <summary>
        /// Must be called before starting loading the LoadAssembly info.
        /// </summary>
        void InitLoading();

        /// <summary>
        /// Processes each single line in the AssemblyInfo source code.
        /// </summary>
        /// <param name="writer">The writer where the new info file must be stored.</param>
        /// <param name="line">The line to be processed.</param>
        /// <param name="version">The new version number.</param>
        /// <param name="comment">The comment to add.</param>
        void ProcessLine(TextWriter writer, string line, AssemblyVersion version, string comment);

        /// <summary>
        /// Loads the info of the assembly from the AssemblyInfo.cs file.
        /// </summary>
        /// <param name="reader">TextReader of the <c>AssemblyInfo.cs</c> formatted file.</param>
        /// <param name="todoList">Class responsible for to-do storage</param>
        /// <returns>The <see cref="IInfoProcessor"/> instance where the loaded information is stored.</returns>
        IInfoProcessor LoadAssemblyInfo(TextReader reader, ITodoList todoList);
    }
}
