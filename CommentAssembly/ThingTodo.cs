//-----------------------------------------------------------------------
// <copyright file="ThingToDo.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Descriptor of the thing to be done.
    /// </summary>
    public class ThingTodo
    {
        /// <summary>
        /// Backing field of the <see cref="IsDone"/> property.
        /// </summary>
        private bool isDone;

        /// <summary>
        /// Initializes static members of the <see cref="ThingTodo" /> class.
        /// </summary>
        static ThingTodo()
        {
            TheThingsToDo = new List<ThingTodo>();
            ThingsDoneDuringThisSession = new HashSet<ThingTodo>();
        }

        /// <summary>
        /// Gets or sets the set of to-do's that have been implemented
        /// </summary>
        public static HashSet<ThingTodo> ThingsDoneDuringThisSession
        {
            get;
            set;
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
        /// Gets a unique identifier of the to-do.
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the to-do has been implemented
        /// </summary>
        public bool IsDone
        {
            get
            {
                return this.isDone;
            }

            set
            {
                this.isDone = value;
                if (this.isDone)
                {
                    ThingsDoneDuringThisSession.Add(this);
                }
                else
                {
                    if (ThingsDoneDuringThisSession.Contains(this))
                    {
                        ThingsDoneDuringThisSession.Remove(this);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of things to be done 
        /// </summary>
        private static IList<ThingTodo> TheThingsToDo
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a to-do that can be done or not
        /// </summary>
        /// <param name="isDone">True if the to-do is done</param>
        /// <param name="description">First line describing the to-do</param>
        public static void Add(bool isDone, string description)
        {
            int maxId = 0;
            if (TheThingsToDo.Count > 0)
            {
                maxId = TheThingsToDo.Max<ThingTodo>((t) => { return t.Id; });
            }

            ThingTodo.TheThingsToDo.Add(new ThingTodo() { Id = maxId + 1, IsDone = isDone, Description = description });
        }

        /// <summary>
        /// Appends the line to the last to-do, adding a new line in between
        /// </summary>
        /// <param name="line">Line to be added to the last to-do</param>
        public static void AppendLine(string line)
        {
            int lastIndex = ThingTodo.TheThingsToDo.Count - 1;
            if (lastIndex < 0)
            {
                throw new FormatException("Impossible to append lines here");
            }

            ThingTodo.TheThingsToDo[lastIndex].Description += Environment.NewLine + line;
        }

        /// <summary>
        /// Deletes the to-do having the passed id
        /// </summary>
        /// <param name="id">Id of the to-do to be deleted</param>
        public static void Delete(int id)
        {
            int indexToDelete = FindIndex(id);
            if (indexToDelete >= 0)
            {
                TheThingsToDo.RemoveAt(indexToDelete);
            }
        }

        /// <summary>
        /// Iterates the passed Action as visitor to every to-do in the list
        /// </summary>
        /// <param name="visitor">An action delegate accepting the current to-do as parameter</param>
        public static void ForEach(Action<ThingTodo> visitor)
        {
            foreach (var element in TheThingsToDo)
            {
                if (visitor != null)
                {
                    visitor(element);
                }
            }
        }

        /// <summary>
        /// Computes a reference to the to-do having the passed identifier
        /// </summary>
        /// <param name="id">The unique identifier of the ThingToDo to be found.</param>
        /// <returns>A reference to the found ThingToDo, null if not found</returns>
        public static ThingTodo HavingId(int id)
        {
            int index = FindIndex(id);
            if (index >= 0)
            {
                return ThingTodo.TheThingsToDo[index];
            }

            return null;
        }

        /// <summary>
        /// Changes the description string of the to-do having the passed id
        /// </summary>
        /// <param name="id">Id of the to-do to be changed</param>
        /// <param name="newContent">New description of the to-do</param>
        public static void Modify(int id, string newContent)
        {
            int index = FindIndex(id);
            if (index >= 0)
            {
                ThingTodo.TheThingsToDo[index].Description = newContent;
            }
        }

        /// <summary>
        /// Linear search of the to-do having the specified id
        /// </summary>
        /// <param name="id">Unique identifier of the to-do to be searched</param>
        /// <returns>The index of the found to-do or -1 if the specified Id does not exist.
        /// </returns>
        private static int FindIndex(int id)
        {
            for (int i = 0; i < TheThingsToDo.Count; i++)
            {
                if (TheThingsToDo[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
