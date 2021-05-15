using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Core;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Listener
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ListenerWindow : Window
    {
        ProcessListener listener = new ProcessListener();
        LogFile logFile;
        Process p;




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
                ProcessBox.Items.Add(p.ProcessName + " " + p.Id);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] process = ProcessBox.SelectedItem.ToString().Split(' ').ToArray();
            p = Process.GetProcessById(int.Parse(process[1]));
            logFile = OpenFile(logFile);
            //listener.Subscribe(logFile, p, (bool)cpuCheckBox.IsChecked, (bool)memoryCheckBox.IsChecked);
            DataGridProcesses.Items.Add(new { ProcessName = p.ProcessName, ProcessID = p.Id, ProcessStatus = "In progress", ProcessStopper = "StopClick"});
            ProcessBox.Items.Remove(ProcessBox.SelectedItem);

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
        private void DataGridCell_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            listener.Unsubscribe(p);
            string process  = DataGridProcesses.SelectedItem.ToString();
            string[] arrDetailOfProc = process.Split(' ').ToArray();
            ProcessBox.Items.Add($"{arrDetailOfProc[0]} {arrDetailOfProc[1]}");
            DataGridProcesses.Items.Remove(DataGridProcesses.SelectedItem);

        }
    }
}
