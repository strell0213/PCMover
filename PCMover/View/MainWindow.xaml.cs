using PCMover.Services;
using PCMover.SimpleLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PCMover
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly IEService _IEService;
        public MainWindow()
        {
            InitializeComponent();

            _IEService = new IEService();
        }

        private void BExportFile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                _IEService.ExportData();
            }
            catch (Exception ex)
            {
                Logger log = new Logger("ExportErrorButton", ex.Message, (logLevel)3);
                log.Write();
            }
        }
    }
}
