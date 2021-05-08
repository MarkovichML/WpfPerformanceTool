using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Windows.Forms;
using Core;

namespace Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainMonitorWindow : Window
    {
        List<double> cpu;
        List<double> memory;
        List<int> totalCpu;
        List<int> totalMemory;
        List<DateTime> time;
        private Task drawing;
        LogFile logFile;
        bool totalMemoryCheckBox;
        bool processCpuCheckBox;
        bool processMemoryCheckBox;
        bool totalCpuCheckBox;

        public MainMonitorWindow(LogFile log,bool totmem,bool totcpu,bool procmem,bool proccpu)
        {
            logFile = log;
            totalMemoryCheckBox = totmem;
            totalCpuCheckBox = totcpu;
            processMemoryCheckBox = procmem;
            processCpuCheckBox = proccpu;
            InitializeComponent();
            MemoryChart.ChartAreas[0].AxisX.LabelStyle.Format = "MM/dd/yyyy HH:mm:ss";
            MemoryChart.Series[0].XValueType = ChartValueType.DateTime;
            CPUchart.ChartAreas[0].AxisX.LabelStyle.Format = "MM/dd/yyyy HH:mm:ss";
            CPUchart.Series[0].XValueType = ChartValueType.DateTime;
            Drawing();
        }
        private void Drawing()
        {
            Action _drawing = () =>
            {
                LogFile reader = new LogFile();
                cpu = new List<double>();
                memory = new List<double>();
                totalCpu = new List<int>();
                totalMemory = new List<int>();
                time = new List<DateTime>();
                while (true)
                {
                    cpu = reader.ReadCpu(logFile);
                    memory = reader.ReadMemory(logFile);
                    time = reader.ReadTime(logFile);
                    totalCpu = reader.ReadTotalCpu(logFile);
                    totalMemory = reader.ReadTotalMemory(logFile);
                    UpdateForm();
                    Thread.Sleep(1000);
                }
            };
            drawing = Task.Factory.StartNew(_drawing);
        }
        private void UpdateForm()
        {
            Action update = () =>
            {
                if (processCpuCheckBox == true)
                {
                    CPUchart.Series["series3"].Points.Clear();
                    CPUchart.Series["series3"].Points.DataBindXY(time, cpu);
                }
                if (totalCpuCheckBox == true)
                {
                    CPUchart.Series["series4"].Points.Clear();
                    CPUchart.Series["series4"].Points.DataBindXY(time, totalCpu);
                }
                if (processMemoryCheckBox == true)
                {
                    MemoryChart.Series["series1"].Points.Clear();
                    MemoryChart.Series["series1"].Points.DataBindXY(time, memory);
                }
                if (totalMemoryCheckBox == true)
                {
                    MemoryChart.Series["series2"].Points.Clear();
                    MemoryChart.Series["series2"].Points.DataBindXY(time, totalMemory);
                }
            };
            Dispatcher.Invoke(update);
        }
    }
}

