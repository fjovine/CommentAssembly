﻿//-----------------------------------------------------------------------
// <copyright file="ToDoListUserControl.xaml.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
// <creation>2016.04.13</creation>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for <c>ToDoList.xaml</c>
    /// </summary>
    public partial class ToDoList : UserControl
    {
        /// <summary>
        /// This predicate is true when all the to-do's (deleted and not deleted) are to be shown
        /// </summary>
        private static bool showAll = false;

        /// <summary>
        ///  Id of the content being modified
        /// </summary>
        private int idToModify;

        /// <summary>
        /// This predicate is true when the to-do content is being modified
        /// </summary>
        private bool isModifyMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoList" /> class.
        /// </summary>
        public ToDoList()
        {
            this.InitializeComponent();
            this.TodoList.ItemsSource = ListToBeShown;
        }

        /// <summary>
        /// Gets the list of to-do to be shown, dependent on the <see cref="showAll"/> predicate.
        /// </summary>
        private static IList<ThingTodo> ListToBeShown
        {
            get
            {
                IList<ThingTodo> result = new List<ThingTodo>();
                ThingTodo.ForEach((todo) =>
                {
                    if (showAll)
                    {
                        result.Add(todo);
                    }
                    else
                    {
                        if (!todo.IsDone)
                        {
                            result.Add(todo);
                        }
                    }
                });
                return result;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user control is in modify mode.
        /// </summary>
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
                this.EditButton.IsEnabled = this.TodoToAdd.Text.Length > 0;
                this.DelButton.IsEnabled = this.isModifyMode;
            }
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
                ThingTodo.Modify(this.idToModify, this.TodoToAdd.Text);
            }
            else
            {
                ThingTodo.Add(false, this.TodoToAdd.Text);
            }

            this.TodoList.ItemsSource = ListToBeShown;
            this.TodoToAdd.Clear();
            this.IsModifyMode = false;
        }

        /// <summary>
        /// Handles the event of click on the Delete button
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.IsModifyMode)
            {
                this.TodoList.ItemsSource = null;
                ThingTodo.Delete(this.idToModify);
                this.TodoList.ItemsSource = ListToBeShown;
                this.TodoToAdd.Clear();
                this.IsModifyMode = false;
            }
        }

        /// <summary>
        /// Handles the event sent when the show deleted checkbox is checked.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ShowDeleted_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.TodoList.ItemsSource = null;
            showAll = this.ShowDeleted.IsChecked == true;
            this.TodoList.ItemsSource = ListToBeShown;
        }

        /// <summary>
        /// Handles the even generated by selection of a new row in the <c>datagrid</c> of to-dos.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void TodoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected = this.TodoList.SelectedIndex;
            if (selected < 0)
            {
                return;
            }

            if (this.IsModifyMode)
            {
                // In modify mode, it inhibits to change the selected cells
                this.TodoList.SelectedCells.Clear();
                return;
            } 

            IList<DataGridCellInfo> selectedRow = this.TodoList.SelectedCells;
            this.idToModify = ((ThingTodo)selectedRow[0].Item).Id;

            ThingTodo selectedTodo = ThingTodo.HavingId(this.idToModify);
            if (selectedTodo != null)
            {
                string todo = selectedTodo.Description;
                this.TodoToAdd.Text = todo;
                this.IsModifyMode = true;
            }
        }

        private void TodoToAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.EditButton.IsEnabled = this.TodoToAdd.Text.Length > 0;
        }
    }
}
