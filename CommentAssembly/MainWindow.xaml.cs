using System;
using System.IO;
using System.Windows;

namespace CommentAssembly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.TheAssemblyInfo = AssemblyInfo.LoadAssemblyInfo(this.ProjectFolder);
            this.Title = "Current Version " + TheAssemblyInfo;
            InitializeComponent();
            foreach (string line in this.TheAssemblyInfo.LastComments)
            {
                this.History.AppendText(line);
                this.History.AppendText(Environment.NewLine);
            }
        }

        public AssemblyInfo TheAssemblyInfo
        {
            get;
            set;
        }

        private string ProjectFolder
        {
            get
            {
                string result = Environment.GetCommandLineArgs()[1];
                if (result.EndsWith("\""))
                {
                    result = result.Substring(0, result.Length - 1);
                }
                return result;
            }
        }

        public string UpdateVersionWithComment(string AssemblyInfoFile, string Comment)
        {
            return string.Empty;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AssemblyInfo.UpdateAssemblyInfo(
                this.ProjectFolder,
                this.TheAssemblyInfo.CurrentVersion.Next(),
                this.Comment.Text);
        }
    }
}
