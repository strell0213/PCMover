using PCMover.Models;
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
    public enum ActivePage { ExportTab, ImportTab }
    public partial class MainWindow : Window
    {
        public ActivePage _activaPage; //энам - на какой вкладке находится пользователь
        public readonly IEService _IEService; //логика импорта и экспорта
        public AnimationClass _animationClass; //класс для анимации
        public MainWindow()
        {
            InitializeComponent();

            _IEService = new IEService();
            _activaPage = ActivePage.ExportTab;
            _animationClass = new AnimationClass();
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

        private void GExportTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_activaPage == ActivePage.ExportTab) return;

            UpdateTabOn(LExportTab, LineExportTab, GExport, GExportTab);
            UpdateTabOff(LImportTab, LineImportTab, GImport, GImportTab);

            _activaPage = ActivePage.ExportTab;
        }

        private void GImportTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_activaPage == ActivePage.ImportTab) return;

            UpdateTabOn(LImportTab, LineImportTab, GImport, GImportTab);
            UpdateTabOff(LExportTab, LineExportTab, GExport, GExportTab);

            _activaPage = ActivePage.ImportTab;
        }

        public void UpdateTabOn(Label label, Line line, Grid grid, Grid headerGrid)
        {
            label.Foreground = ColorFields.ColorActiveTab;
            line.Fill = ColorFields.ColorActiveTab;
            line.X2 = grid.ActualWidth;
            grid.Visibility = Visibility.Visible;

            _animationClass.AnimationLineActive(line, headerGrid);
        }

        public void UpdateTabOff(Label label, Line line, Grid grid, Grid headerGrid)
        {
            label.Foreground = ColorFields.ColorInactiveTab;
            line.Fill = ColorFields.ColorInactiveTab;
            line.X2 = 0;
            grid.Visibility = Visibility.Hidden;

            _animationClass.AnimationLineInactive(line, headerGrid);
        }
    }
}
