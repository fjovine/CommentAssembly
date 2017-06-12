//-----------------------------------------------------------------------
// <copyright file="PascalInfoProcessor.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace CommentAssembly
{
    public class PascalInfoProcessor : IInfoProcessor
    {
        /// <summary>
        /// Backup field of the LastComments property.
        /// </summary>
        private readonly List<string> lastComments = new List<string>();

        /// <summary>
        /// Gets or sets the current version of the assembly.
        /// </summary>
        public AssemblyVersion CurrentVersion
        {
            get;
            set;
        }



        public IEnumerable<string> LastComments
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string ProjectName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string RelativeInfoFilePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void InitLoading()
        {
            throw new NotImplementedException();
        }

        public IInfoProcessor LoadAssemblyInfo(TextReader reader, ITodoList todoList)
        {
            throw new NotImplementedException();
        }

        public void ProcessLine(TextWriter writer, string line, AssemblyVersion version, string comment)
        {
            throw new NotImplementedException();
        }
    }
}