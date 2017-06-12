//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for <c>App.xaml</c>
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Handler that manages the application startup event. It checks the presence of the project
        /// path as the only command line parameter. If this is not the case, a message box is shown.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (! CommentAssembly.MainWindow.ProcessCommandLine())
            {
                MessageBox.Show("Error : the project path must be provided on the command line");
                Environment.Exit(1);
            }
        }
    }
}
