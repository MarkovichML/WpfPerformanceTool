using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Threading;
using Core;

namespace Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainMonitorWindow : Window
    {
        private Task drawing;
        LogFile logFile;
        bool totalMemoryCheckBox;
        bool processCpuCheckBox;
        bool processMemoryCheckBox;
        bool totalCpuCheckBox;
        List<Data> data = new List<Data>();

        public MainMonitorWindow(LogFile log,bool totmem,bool totcpu,bool procmem,bool proccpu)
        {
            logFile = log;
            totalMemoryCheckBox = totmem;
            totalCpuCheckBox = totcpu;
            processMemoryCheckBox = procmem;
            processCpuCheckBox = proccpu;
            InitializeComponent();
            TotalChart.ChartAreas[0].AxisX.LabelStyle.Format = "MM/dd/yyyy HH:mm:ss";
            TotalChart.Series[0].XValueType = ChartValueType.DateTime;
            ProcessChart.ChartAreas[0].AxisX.LabelStyle.Format = "MM/dd/yyyy HH:mm:ss";
            ProcessChart.Series[0].XValueType = ChartValueType.DateTime;
            ProcessChart.Series["Process Cpu"].Color = Color.Blue;
            ProcessChart.Series["Process Memory"].Color = Color.Yellow;
            TotalChart.Series["Total Memory"].Color = Color.Yellow;
            TotalChart.Series["Total Cpu"].Color = Color.Blue;
        }

        private void Drawing(IDataInteraction<Data> reader)
        {
            Action _drawing = () =>
            {
                while (true)
                {
                    data = reader.ReadData();
                    UpdateForm();
                    Thread.Sleep(1000);
                }
            };
            drawing = Task.Factory.StartNew(_drawing);
        }
        private void DrawingOnce(IDataInteraction<Data> reader)
        {
            Action _drawing = () =>
            {
                    data = reader.ReadData();
                    UpdateFormOnce();
                    Thread.Sleep(1000);
            };
            drawing = Task.Factory.StartNew(_drawing);
        }

        private void UpdateForm()
        {
            Action update = () =>
            {
                if (TimePicker.IsChecked == false)
                {
                    if (processCpuCheckBox)
                    {
                        ProcessChart.Series["Process Cpu"].Points.AddXY(data[data.Count - 1].Time, data[data.Count - 1].ProcessCpu);
                        int count = ProcessChart.Series["Process Cpu"].Points.Count;
                        if (data[data.Count - 1].ProcessCpu > int.Parse(LimitCPUBox.Text))
                        {
                            ProcessChart.Series["Process Cpu"].Points[count-1].Color = Color.DarkBlue;
                        }
                        else if (data[data.Count - 1].NotResponding)
                        {
                            ProcessChart.Series["Process Cpu"].Points[count-1].Color = Color.Black;
                        }
                    }
                    else
                        ProcessChart.Series["Process Cpu"].Enabled = false;

                    if (processMemoryCheckBox)
                    {
                        ProcessChart.Series["Process Memory"].Points.AddXY(data[data.Count - 1].Time, data[data.Count - 1].ProcessMemory);
                        int count = ProcessChart.Series["Process Memory"].Points.Count;

                        if (data[data.Count - 1].ProcessMemory > int.Parse(LimitMemoryBox.Text))
                            {
                                ProcessChart.Series["Process Memory"].Points[count - 1].Color = Color.Red;
                            }
                        else if (data[data.Count-1].NotResponding)
                            {
                                ProcessChart.Series["Process Memory"].Points[count - 1].Color = Color.DarkRed;
                            }
                    }
                    else
                        ProcessChart.Series["Process Memory"].Enabled = false;

                    if (totalCpuCheckBox)
                    {
                        TotalChart.Series["Total Cpu"].Points.AddXY(data[data.Count - 1].Time, data[data.Count - 1].TotalCpu);
                    }
                    else
                        TotalChart.Series["Total Cpu"].Enabled = false;



                    if (totalMemoryCheckBox)
                    {
                        TotalChart.Series["Total Memory"].Points.AddXY(data[data.Count - 1].Time, data[data.Count - 1].TotalMemory);
                    }
                    else
                        TotalChart.Series["Total Memory"].Enabled = false;
                    IncreasingChart.Series["Increase"].Points.AddXY(data[data.Count-1].Time, data[data.Count-1].VolumeIncrease);
                }
            };

            Dispatcher.Invoke(update);
        }
        private void UpdateFormOnce()
        {
            Action update = () =>
            {
                foreach (Data dat in data)
                {
                    if (DateTime.Parse(dat.Time) > StartTimePicker.Value && DateTime.Parse(dat.Time) < EndTimePicker.Value)
                    {

                        if (processCpuCheckBox)
                        {
                            ProcessChart.Series["Process Cpu"].Points.AddXY(dat.Time, dat.ProcessCpu);
                            int count = ProcessChart.Series["Process Cpu"].Points.Count;
                            if (dat.ProcessCpu > int.Parse(LimitCPUBox.Text))
                            {
                                ProcessChart.Series["Process Cpu"].Points[count - 1].Color = Color.DarkBlue;
                            }
                            else if (data[data.Count - 1].NotResponding)
                            {
                                ProcessChart.Series["Process Cpu"].Points[count - 1].Color = Color.Black;
                            }
                        }
                        else
                            ProcessChart.Series["Process Cpu"].Enabled = false;

                        if (processMemoryCheckBox)
                        {
                            ProcessChart.Series["Process Memory"].Points.AddXY(dat.Time, dat.ProcessMemory);
                            int count = ProcessChart.Series["Process Memory"].Points.Count;

                            if (dat.ProcessMemory > int.Parse(LimitMemoryBox.Text))
                            {
                                ProcessChart.Series["Process Memory"].Points[count - 1].Color = Color.Red;
                            }
                            else if (data[data.Count - 1].NotResponding)
                            {
                                ProcessChart.Series["Process Memory"].Points[count - 1].Color = Color.DarkRed;
                            }
                        }
                        else
                            ProcessChart.Series["Process Memory"].Enabled = false;

                        if (totalCpuCheckBox)
                        {
                            TotalChart.Series["Total Cpu"].Points.AddXY(dat.Time, dat.TotalCpu);
                        }
                        else
                            TotalChart.Series["Total Cpu"].Enabled = false;

                        if (totalMemoryCheckBox)
                        {
                            TotalChart.Series["Total Memory"].Points.AddXY(dat.Time, dat.TotalMemory);
                        }
                        else
                            TotalChart.Series["Total Memory"].Enabled = false;
                        IncreasingChart.Series["Increase"].Points.AddXY(dat.Time, dat.VolumeIncrease);
                    }
                }
            };
            Dispatcher.Invoke(update);
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimePicker.IsChecked == false)
            {
                Drawing(logFile);
            }
            else
            {
                DrawingOnce(logFile);
            }
        }
    }
}

