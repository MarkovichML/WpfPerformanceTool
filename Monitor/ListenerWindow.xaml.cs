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
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Core;

namespace Listener
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ListenerWindow : Window
    {
        Listen listener = new Listen();
        LogFile logFile;
        
        public ListenerWindow()
        {
            InitializeComponent();
            ListProcessesBox();
            AddingButton.IsEnabled = false;
            
        }
        private void ListProcessesBox()
        {
            Process[] processCollection = Process.GetProcesses();
            foreach (Process p in processCollection)
            {
                ProcessBox.Items.Add(p.Id);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process p = Process.GetProcessById((int)ProcessBox.SelectedItem);
            logFile = OpenFile(logFile);
            listener.Subscribe(logFile, p,cpuCheckBox.IsEnabled,memoryCheckBox.IsEnabled);
            ListViewProcesses.Items.Add(new { ProcessName = p.ProcessName, ProcessID = p.Id, ProcessStatus = "In progress", ProcessStopper = new System.Windows.Controls.Button() }) ;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            
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

        private void memoryCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (memoryCheckBox.IsEnabled)
                AddingButton.IsEnabled = true;
        }

        private void cpuCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (cpuCheckBox.IsEnabled)
                AddingButton.IsEnabled = true;
        }
    }
}
