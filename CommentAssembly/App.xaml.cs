//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    using System;
    using System.IO;
    using System.Windows;

    /// <summary>
    /// Interaction logic for <c>App.xaml</c>
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets the user name as found in {home}/.CommentAssembly.
        /// </summary>
        public static string TheUser
        {
            get;
            private set;
        }

        /// <summary>
        /// Handler that manages the application startup event. It checks the presence of the project
        /// path as the only command line parameter. If this is not the case, a message box is shown.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                // Issue#2
                // In order to differentiate different users or no user a all, in the home folder there must be a file called ".CommentAssembly" that contains only one line
                // With the username. The username is only the first word (no spaces)
                string homeDrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
                string homePath = Environment.GetEnvironmentVariable("HOMEPATH");

                if (string.IsNullOrEmpty(homeDrive) || string.IsNullOrEmpty(homePath))
                {
                    // No environment variables, no user hence exit
                    Environment.Exit(0);
                }

                string userNameFile = homeDrive + Path.Combine(homePath, ".CommentAssembly");

                if (!File.Exists(userNameFile))
                {
                    // No user filename, hence exit.
                    Environment.Exit(0);
                }

                using (StreamReader sr = new StreamReader(userNameFile))
                {
                    string userName = string.Empty;
                    try
                    {
                        userName = sr.ReadLine().Split(' ')[0];
                    }
                    catch (Exception)
                    {
                        userName = string.Empty;
                    }

                    if (string.IsNullOrEmpty(userName))
                    {
                        // No user name found, exits.
                        Environment.Exit(0);
                    }

                    App.TheUser = userName;
                }

                if (!CommentAssembly.MainWindow.ProcessCommandLine())
                {
                    MessageBox.Show("Error : the project path must be provided on the command line");
                    Environment.Exit(1);
                }
            }
            catch (Exception theException)
            {
                MessageBox.Show(string.Format("Error : {0} ", theException.Message));
                Environment.Exit(1);
            }
        }
    }
}
