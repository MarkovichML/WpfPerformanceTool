using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Core;
using System.Threading;
using System.Windows.Forms;

namespace Monitor
{
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public LogFile logFile;
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logFile = OpenFile(logFile);
            MainMonitorWindow mw = new MainMonitorWindow(logFile, totalCpuBox.IsEnabled, totalMemoryBox.IsEnabled, processCpuBox.IsEnabled, processMemoryBox.IsEnabled);
            mw.Show();
            
        }
        public LogFile OpenFile(LogFile logFile)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    logFile = new LogFile(openFileDialog.InitialDirectory + openFileDialog.FileName);
                }
            }
            return logFile;
        }

    }
}
