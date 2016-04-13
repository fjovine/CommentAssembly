//-----------------------------------------------------------------------
// <copyright file="TodoManager.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
// <creation>2016.04.13</creation>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    /// <summary>
    /// Manager of the to-do that implements the <see cref="ITodoLst"/> interface storing them into the to-do list
    /// </summary>
    public class TodoManager : ITodoList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoManager" /> class.
        /// </summary>
        public TodoManager()
        {
        }

        /// <summary>
        /// Adds a to-do the the <see cref="ToDoList"/>  object.
        /// </summary>
        /// <param name="done">True if the to-do has already been implemented, false otherwise</param>
        /// <param name="content">Description of what must be done.</param>
        public void AddTodo(bool done, string content)
        {
            ToDoList.AddTodo(done, content);
        }

        /// <summary>
        /// Appends a line to the last to-do of the list
        /// </summary>
        /// <param name="content">Line to be added to the last to-do</param>
        public void AppendLineTodo(string content)
        {
            ToDoList.AppendLineTodo(content);
        }
    }
}
