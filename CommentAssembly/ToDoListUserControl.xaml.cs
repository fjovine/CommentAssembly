//-----------------------------------------------------------------------
// <copyright file="ToDoListUserControl.xaml.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
// <creation>2016.04.13</creation>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for <c>ToDoList.xaml</c>
    /// </summary>
    public partial class ToDoList : UserControl
    {
        /// <summary>
        /// Initializes static members of the <see cref="ToDoList" /> class.
        /// </summary>
        static ToDoList()
        {
            TheToDoList = new List<ToDoList>();
        }

        private bool isModifyMode;

        private bool IsModifyMode
        {
            get
            {
                return this.isModifyMode; 
            }
            set
            {
                this.isModifyMode = value;
                this.EditButton.Content = this.isModifyMode ? "Mod" : "Add";
                this.DelButton.IsEnabled = this.isModifyMode;
            }
        }

        private int indexToModify;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoList" /> class.
        /// </summary>
        public ToDoList()
        {
            this.InitializeComponent();
            this.TodoList.ItemsSource = TheToDoList;
        }

        /// <summary>
        /// Gets the list of things to be done 
        /// </summary>
        public static IList<ToDoList> TheToDoList
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the description of what should be done
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the to-do has been implemented
        /// </summary>
        public bool IsDone
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a to-do that can be done or not
        /// </summary>
        /// <param name="isDone">True if the to-do is done</param>
        /// <param name="description">First line describing the to-do</param>
        public static void AddTodo(bool isDone, string description)
        {
            ToDoList.TheToDoList.Add(new ToDoList() { IsDone = isDone, Description = description });
        }

        public static void ModifyTodo(int index, string newContent)
        {
            ToDoList.TheToDoList[index].Description = newContent;
        }

        /// <summary>
        /// Appends the line to the last to-do, adding a new line in between
        /// </summary>
        /// <param name="line">Line to be added to the last to-do</param>
        public static void AppendLineTodo(string line)
        {
            int lastIndex = ToDoList.TheToDoList.Count - 1;
            if (lastIndex < 0)
            {
                throw new FormatException("Impossible to append lines here");
            }

            ToDoList.TheToDoList[lastIndex].Description += Environment.NewLine + line;
        }

        /// <summary>
        /// Handles the event when the button is clicked on
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used</param>
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.TodoList.ItemsSource = null;
            if (this.IsModifyMode)
            {
                ModifyTodo(this.indexToModify, this.TodoToAdd.Text);
            }
            else
            {
                AddTodo(false, this.TodoToAdd.Text);
            }
            this.TodoList.ItemsSource = TheToDoList;
            this.TodoToAdd.Clear();
            this.IsModifyMode = false;
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.IsModifyMode)
            {
                this.TodoList.ItemsSource = null;
                TheToDoList.RemoveAt(this.indexToModify);
                this.TodoList.ItemsSource = TheToDoList;
                this.TodoToAdd.Clear();
                this.IsModifyMode = false;
            }
        }

        private void TodoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected = this.TodoList.SelectedIndex;
            if (selected < 0)
            {
                return;
            }
            string todo = ToDoList.TheToDoList[selected].Description;
            this.TodoToAdd.Text = todo;
            this.IsModifyMode = true;
            this.indexToModify = selected;
        }
    }
}
