//-----------------------------------------------------------------------
// <copyright file="ProgramProperty.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
// <creation>2016.04.14</creation>
//-----------------------------------------------------------------------

namespace CommentAssembly
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Manages the properties of the program, i.e. the parameters governing some UI elements
    /// </summary>
    public class ProgramProperty
    {
        /// <summary>
        /// Key of the property stating that the window is to be closed automatically
        /// </summary>
        public static readonly string CLOSEWINAUTOMATICALLY = "CloseWinAutomatically";

        /// <summary>
        /// Key of the property containing the number of seconds after which, if no interaction is see, the window is closed
        /// </summary>
        public static readonly string CLOSINGTIME = "ClosingTime";

        /// <summary>
        /// Key of the property stating if the window should be kept on top of the others.
        /// </summary>
        public static readonly string KEEPONTOP = "KeepOnTop";

        public static readonly string WINLOCATION = "WinLocation";

        /// <summary>
        /// Dictionary containing the registry where the properties are stored
        /// </summary>
        private static Dictionary<string, string> registry = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets a value indicating whether the window is to be automatically closed if no interaction is sensed.
        /// </summary>
        public static bool CloseWinAutomatically
        {
            get
            {
                return Get<bool>(CLOSEWINAUTOMATICALLY, false, b => bool.Parse(b));
            }

            set
            {
                Set<bool>(CLOSEWINAUTOMATICALLY, value, b => b.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating after how many seconds the window should be closed if no human interaction is sensed.
        /// </summary>
        public static string ClosingTime
        {
            get
            {
                return Get<string>(CLOSINGTIME, "5", i => i);
            }

            set
            {
                Set<string>(CLOSINGTIME, value, i => i);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window should be on top of the others
        /// </summary>
        public static bool KeepOnTop
        {
            get
            {
                return Get<bool>(KEEPONTOP, false, b => bool.Parse(b));
            }

            set
            {
                Set<bool>(KEEPONTOP, value, b => b.ToString());
            }
        }

        public static Rect WinLocation
        {
            get
            {
                return Get<Rect>(WINLOCATION, new Rect(10, 10, 900, 350), r =>
                   {
                       Rect result = new Rect();
                       string[] parameters = r.Split(';');
                       result.X = int.Parse(parameters[0]);
                       result.Y = int.Parse(parameters[1]);
                       result.Width = int.Parse(parameters[2]);
                       result.Height = int.Parse(parameters[3]);
                       return result;
                   });
            }

            set
            {
                Set<Rect>(WINLOCATION, value, r => r.X + ";" + r.Y + ";" + r.Width + ";" + r.Height);
            }
        }

        /// <summary>
        /// Iterator that executes the passed function to each property
        /// </summary>
        /// <param name="act">Function that receives two parameters, property name and value, and is applied to each parameter contained</param>
        public static void ForEach(Action<string, string> act)
        {
            foreach (var elem in registry)
            {
                act(elem.Key, elem.Value);
            }
        }

        /// <summary>
        /// Defines the value of the property as a string.
        /// </summary>
        /// <param name="property">Key of the property</param>
        /// <param name="value">String value of the property</param>
        public static void Set(string property, string value)
        {
            if (registry.ContainsKey(property))
            {
                registry[property] = value;
            }
            else
            {
                registry.Add(property, value);
            }
        }

        /// <summary>
        /// Gets a property out of the registry
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="property">Key of the property</param>
        /// <param name="defValue">Default name in case there is no correspondent entry of the property</param>
        /// <param name="decoder">Function mapping the string representation of the property to its value in the chosen type</param>
        /// <returns>The value of the property</returns>
        private static T Get<T>(string property, T defValue, Func<string, T> decoder)
        {
            string toDecode;
            System.Diagnostics.Debug.WriteLine("Gettin property " + property);
            if (registry.TryGetValue(property, out toDecode))
            {
                return decoder(toDecode);
            }
            else
            {
                return defValue;
            }
        }

        /// <summary>
        /// Sets a property in the registry
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="property">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <param name="encoder">Encoder function that maps the typed value to the string value in registry</param>
        private static void Set<T>(string property, T value, Func<T, string> encoder)
        {
            string encoded = encoder(value);
            System.Diagnostics.Debug.WriteLine("Property " + property + " = " + encoded);
            if (registry.ContainsKey(property))
            {
                registry[property] = encoded;
            }
            else
            {
                registry.Add(property, encoded);
            }
        }
    }
}
