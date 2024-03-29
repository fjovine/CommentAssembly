﻿//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Manages the main window of the application <para />
    /// This window is composed by an pane with the comments to be added and a lower pane with the list of the
    /// most recent compilation comments.
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Dictionary<string, Type> Registry = new Dictionary<string, Type>()
        {
            { "CLANGUAGE", typeof(CLanguageInfoProcessor) },
            { "CSHARP", typeof(CSharpInfoProcessor) },
            { "PASCAL", typeof(PascalInfoProcessor) },
            { "RC", typeof(RCInfoProcessor) }
        };

        private static string languageType = "CSHARP";

        /// <summary>
        /// True indicates that an interaction happened with the window.
        /// </summary>
        private bool interactionHappened = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            IInfoProcessor iinfoProcessor = (IInfoProcessor)Activator.CreateInstance(Registry[languageType]);

            this.TheAssemblyInfo = AssemblyInfoProcessor.LoadAssemblyInfo(ProjectFolder, iinfoProcessor);
            if (this.TheAssemblyInfo == null)
            {
                MessageBox.Show("The assembly in " + ProjectFolder + " cannot be loaded");
                Environment.Exit(1);
            }

            Rect rc = ProgramProperty.WinLocation;
            this.Left = rc.X;
            this.Top = rc.Y;
            this.Width = rc.Width;
            this.Height = rc.Height;

            Rect virtualScreen = new Rect(SystemParameters.VirtualScreenLeft, SystemParameters.VirtualScreenTop, SystemParameters.VirtualScreenWidth, SystemParameters.VirtualScreenHeight);
            if (!virtualScreen.Contains(rc))
            {
                // The screen containing all the screens does not contain the stored coordinates. We move the rectangle inside the screen.
                if (this.Width < virtualScreen.Width)
                {
                    double horizontalDiff = virtualScreen.Width - this.Width;
                    this.Left = horizontalDiff / 2;
                }
                else
                {
                    this.Left = 10;
                    this.Width = virtualScreen.Width - 20;
                }

                if (this.Height < virtualScreen.Height)
                {
                    double verticalDiff = virtualScreen.Height - this.Height;
                    this.Top = verticalDiff / 2;
                }
                else
                {
                    this.Top = 10;
                    this.Height = virtualScreen.Height - 20;
                }
            }

            this.InitializeComponent();
            this.TimeoutProgress.Visibility = ProgramProperty.CloseWinAutomatically ? Visibility.Visible : Visibility.Collapsed;
            this.Topmost = ProgramProperty.KeepOnTop;
            this.Title = "Project [" + this.TheAssemblyInfo.ProjectName + "] Current Version :" + this.TheAssemblyInfo;
            this.Release.Text = "CommentAssembly rel. " + Assembly.GetExecutingAssembly().GetName().Version;
            foreach (string line in this.TheAssemblyInfo.LastComments)
            {
                this.History.AppendText(line);
                this.History.AppendText(Environment.NewLine);
            }
        }

        /// <summary>
        /// Gets or sets the object processing the info attached to the current assembly being processed.
        /// </summary>
        public IInfoProcessor TheAssemblyInfo
        {
            get;
            set;
        }

        private static string projectFolder;

        /// <summary>
        /// Gets the project folder passed to the application as a command line parameter.
        /// </summary>
        private static string ProjectFolder
        {
            get
            {
                return projectFolder;
            }

            set
            {
                if (value.EndsWith("\""))
                {
                    projectFolder = value.Substring(0, value.Length - 1);
                }
                else
                {
                    projectFolder = value;
                }
            }
        }

        /// <summary>
        /// Processes the command line. If the first parameter found is among the supported languages, then the second parameter is the position of the version file
        /// otherwise it is the first parameter.
        /// </summary>
        public static bool ProcessCommandLine()
        {
            bool result = false;
            string[] commandLineParameters = Environment.GetCommandLineArgs();
            if (commandLineParameters.Length > 2)
            {
                Type languageDecoder;
                if (Registry.TryGetValue(commandLineParameters[1].ToUpperInvariant(), out languageDecoder))
                {
                    // Format <language> <folder> in this case the number of parameters is 3
                    if (commandLineParameters.Length == 3)
                    {
                        languageType = commandLineParameters[1].ToUpperInvariant();
                        ProjectFolder = commandLineParameters[2];
                        result = true;
                    }
                }
                else
                {
                    // Format <language> <folder> in this case the number of parameters is 2 (defaults to csharp)
                    if (commandLineParameters.Length == 2)
                    {
                        ProjectFolder = commandLineParameters[1];
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Handler called when the main windows is closing that stores the additional comments
        /// as well as the updated version number into the target AssemblyInfo.cs file
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IInfoProcessor iinfoProcessor = (IInfoProcessor)Activator.CreateInstance(Registry[languageType]);

            if (ThingTodo.ThingsDoneDuringThisSession.Count > 0)
            {
                foreach (var todo in ThingTodo.ThingsDoneDuringThisSession)
                {
                    if (this.Comment.Text.Length > 0)
                    {
                        this.Comment.AppendText("\n");
                    }

                    this.Comment.AppendText("Done : " + todo.Description);
                }
            }

            AssemblyInfoProcessor.UpdateAssemblyInfo(
                ProjectFolder,
                this.TheAssemblyInfo.CurrentVersion.Next(),
                this.Comment.Text,
                iinfoProcessor);
        }

        /// <summary>
        /// Handles the DeactivatedEvent to put the window on top of the others.
        /// </summary>
        /// <param name="sender">Window that created the event.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = ProgramProperty.KeepOnTop;
        }

        /// <summary>
        /// Handles the KeyDown event to stop the countdown.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!this.interactionHappened && e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
            }

            this.interactionHappened = true;
        }

        /// <summary>
        /// Handles the Loaded event to start a timer that keeps track of the timeout.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer closeIfNoKeyPressed = new DispatcherTimer();
            closeIfNoKeyPressed.Interval = new TimeSpan(0, 0, 0, 1, 0);
            this.TimeoutProgress.Maximum = ProgramProperty.ClosingTime;
            this.TimeoutProgress.Minimum = 0;
            this.TimeoutProgress.Value = 0;
            int sec = 0;
            closeIfNoKeyPressed.Tick += (s, a) =>
            {
                if (sec >= ProgramProperty.ClosingTime && !interactionHappened && ProgramProperty.CloseWinAutomatically)
                {
                    this.Close();
                }

                if (interactionHappened)
                {
                    this.TimeoutProgress.Visibility = Visibility.Collapsed;
                }

                if (this.TimeoutProgress.Visibility == Visibility.Visible)
                {
                    this.TimeoutProgress.Value = sec++;
                }
            };

            closeIfNoKeyPressed.Start();
        }

        /// <summary>
        /// Handles the MouseDown event to stop the countdown.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.interactionHappened = true;
        }

        /// <summary>
        /// Handler of the event signaling that the properties checkbox is active
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void ButtonProperties_Click(object sender, RoutedEventArgs e)
        {
            this.PropertiesPanel.Visibility = this.PropertiesActivator.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Handler of the event happening when the window is dragged. Stores the windows location in the parameter.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            ProgramProperty.WinLocation = new Rect(this.Left, this.Top, this.Width, this.Height);
        }

        /// <summary>
        /// Handler of the event happening when the window changes its size. Stores the windows location in the parameter.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                // The new size is stored only if the window is in Normal mode
                ProgramProperty.WinLocation = new Rect(this.Left, this.Top, this.ActualWidth, this.ActualHeight);
            }
        }
    }
}
