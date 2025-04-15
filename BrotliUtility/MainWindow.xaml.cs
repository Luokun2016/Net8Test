using BrotliUtility.Utility;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace BrotliUtility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private BackgroundWorker _demoBGWorker;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
            _demoBGWorker = new BackgroundWorker();
            _demoBGWorker.DoWork += demoBGWorker_DoWork;
            _demoBGWorker.RunWorkerCompleted += demoBGWorker_RunWorkerCompleted;
        }

        private void demoBGWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            var viewModel = (ViewModel)DataContext;
            viewModel.ResultContent = e.Result as string;

            if (!string.IsNullOrEmpty(viewModel.OutPutPath))
            {
                try
                {
                    var fileName = Path.GetFileName(viewModel.InputFile);
                    Directory.CreateDirectory(viewModel.OutPutPath);
                    File.WriteAllText(Path.Combine(viewModel.OutPutPath, fileName.Replace(".br",string.Empty)),viewModel.ResultContent);
                }
                catch
                {
                }
            }
        }

        private void demoBGWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            var model = e.Argument as ViewModel;
            if (model != null && File.Exists(model.InputFile))
            {
                var result = FileUtility.DecompressBrotliFile(model.InputFile);
                e.Result = result;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 选取路径
            Btn.IsEnabled = false;
            var filePath = FileUtility.SelectFile("br(*.br)|*.br");
            if (!string.IsNullOrEmpty(filePath))
            {
                var viewModel = (ViewModel)DataContext;
                viewModel.InputFile = filePath;

                _demoBGWorker.RunWorkerAsync(viewModel);
            }

            Btn.IsEnabled = true;
        }

        private void OutBtn_Click(object sender, RoutedEventArgs e)
        {
            // 选取路径
            OutBtn.IsEnabled = false;
            var filePath = FileUtility.SelectFolder();
            if (!string.IsNullOrEmpty(filePath))
            {
                var viewModel = (ViewModel)DataContext;
                viewModel.OutPutPath = filePath;
            }

            OutBtn.IsEnabled = true;
        }
    }
}