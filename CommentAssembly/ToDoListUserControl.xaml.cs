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
    using System.Linq;

    /// <summary>
    /// Interaction logic for <c>ToDoList.xaml</c>
    /// </summary>
    public partial class ToDoList : UserControl
    {

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
                //this.TodoList.IsEnabled = !this.isModifyMode;
            }
        }

        private int idToModify;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoList" /> class.
        /// </summary>
        public ToDoList()
        {
            this.InitializeComponent();
            this.TodoList.ItemsSource = listToBeShown;
        }

        private static bool showAll = false;

        private static IList<ThingTodo> listToBeShown
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
                        if (! todo.IsDone)
                        {
                            result.Add(todo);
                        }
                    }
                });
                return result;
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
            this.TodoList.ItemsSource = listToBeShown;
            this.TodoToAdd.Clear();
            this.IsModifyMode = false;
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.IsModifyMode)
            {
                this.TodoList.ItemsSource = null;
                ThingTodo.Delete(this.idToModify);
                this.TodoList.ItemsSource = listToBeShown;
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

        private void ShowDeleted_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.TodoList.ItemsSource = null;
            showAll = this.ShowDeleted.IsChecked == true;
            this.TodoList.ItemsSource = listToBeShown;
        }
    }
}
