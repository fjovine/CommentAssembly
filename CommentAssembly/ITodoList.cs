//-----------------------------------------------------------------------
// <copyright file="ITodoList.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
// <creation>2016.04.13</creation>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    /// <summary>
    /// Interface abstracting the operation of adding a to-do to the to-do list
    /// </summary>
    public interface ITodoList
    {
        /// <summary>
        /// Adds a to-do together with a predicate that is true if the to-do has been implemented.
        /// </summary>
        /// <param name="done">True if the to-do has been implemented</param>
        /// <param name="content">Description of what it to be done</param>
        void AddTodo(bool done, string content);

        /// <summary>
        /// Appends other lines to the last defined to-do 
        /// </summary>
        /// <param name="toAppend">line to be appended to the last to-do</param>
        void AppendLineTodo(string toAppend);
    }
}
