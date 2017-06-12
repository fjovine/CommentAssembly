
namespace CommentAssembly
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public abstract class AbstractInfoProcessor : IInfoProcessor
    {
        /// <summary>
        /// Prefix string for the first line of a new to-do in the AssemblyInfo.cs file
        /// </summary>
        protected static readonly string StartOfTodoContent = "// TODO [";

        /// <summary>
        /// Prefix string for the following lines of the already started to-do's in the AssemblyInfo.cs file
        /// </summary>
        protected static readonly string StartOfTodoFollowing = "// TODO";

        /// <summary>
        /// Starting string that encodes a program property
        /// </summary>
        protected static readonly string TodoParam = "// TODO PARAM";

        /// <summary>
        /// Ending string for the to-do zone in the AssemblyInfo.cs file
        /// </summary>
        protected static readonly string EndOfTodos = "// ENDTODO";

        /// <summary>
        /// Number of lines containing the latest compilation comments to be loaded from <c>AssemblyInfo.cs</c>
        /// file.
        /// </summary>
        protected static readonly int MaxComments = 50;

        /// <summary>
        /// Backup field of the LastComments property.
        /// </summary>
        protected readonly List<string> lastComments = new List<string>();

        /// <summary>
        /// Gets or sets the current version of the assembly.
        /// </summary>
        public AssemblyVersion CurrentVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets an enumeration of the last comments.
        /// </summary>
        public IEnumerable<string> LastComments
        {
            get
            {
                return this.lastComments;
            }
        }


        /// <summary>
        /// Gets or sets the name of the project
        /// </summary>
        public string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the relative file path with respect to the folder whence the program is launched
        /// </summary>
        public virtual string RelativeInfoFilePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Must be called before starting loading the LoadAssembly info.
        /// </summary>
        public abstract void InitLoading();

        /// <summary>
        /// Loads the info of the assembly from the AssemblyInfo.cs file.
        /// </summary>
        /// <param name="reader">TextReader of the <c>AssemblyInfo.cs</c> formatted file.</param>
        /// <param name="todoList">Class responsible for to-do storage</param>
        /// <returns>The <see cref="IInfoProcessor"/> instance where the loaded information is stored.</returns>
        public abstract IInfoProcessor LoadAssemblyInfo(TextReader reader, ITodoList todoList);

        /// <summary>
        /// Processes each single line in the AssemblyInfo source code.
        /// </summary>
        /// <param name="writer">The writer where the new info file must be stored.</param>
        /// <param name="line">The line to be processed.</param>
        /// <param name="version">The new version number.</param>
        /// <param name="comment">The comment to add.</param>
        public abstract void ProcessLine(TextWriter writer, string line, AssemblyVersion version, string comment);
    }
}
