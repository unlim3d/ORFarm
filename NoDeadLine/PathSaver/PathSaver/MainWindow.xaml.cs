using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;
using Newtonsoft.Json;

namespace PathSaver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "My Title";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;
            dlg.IsFolderPicker = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folder = dlg.FileName;
                // Do something with selected folder string
                UserPath path = new UserPath();
                path.Path = folder;
                string json = JsonConvert.SerializeObject(path);
                System.IO.File.WriteAllText(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\path.json", json);
                MessageBoxResult result = MessageBox.Show(folder, "Your choice");
                if (result == MessageBoxResult.OK || result == MessageBoxResult.Cancel)
                {
                    System.Windows.Application.Current.Shutdown();
                }
            }
        }
    }

    public class UserPath
    {
        public string Path { get; set; }
    }
}

