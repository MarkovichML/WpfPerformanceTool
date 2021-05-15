using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Core;

namespace Listener
{
    public class ProcessListener

    {
        private Task subs;
        private static DateTime currentTime;
        PerformanceCounter totalRamCounter = new PerformanceCounter("Memory", "Available MBytes");
        PerformanceCounter totalCpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        Data data;
        string line;
        int processCpu;
        int processMemory;
        int totalCpu;
        int totalMemory;
        double currentSize;
        double previousSize;
        double sizeIncrease;
        

        private static Dictionary<Process, CancellationTokenSource> ListenedProcesses { get; } = new Dictionary<Process, CancellationTokenSource>();

        public void Subscribe(IDataInteraction<Data> dataWriter, DirectoryInfo directory, Process process, bool cpuBoxStatus, bool memoryBoxStatus)
        {
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken cancelationToken = cts.Token;
                ListenedProcesses.Add(process, cts);

                Action subscribe = () =>
                {
                    PerformanceCounter ramCounter = new PerformanceCounter("Process", "Working set", process.ProcessName);
                    PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
                    List<Data> dataSet = new List<Data>();
                    while (!cancelationToken.IsCancellationRequested)
                    {
                        currentTime = DateTime.Now;
                        if (cpuBoxStatus)
                        {
                            processCpu = (int)cpuCounter.NextValue();
                            totalCpu = (int)totalCpuCounter.NextValue();
                        }
                        else
                        {
                            processCpu = (int)cpuCounter.NextValue();
                            totalCpu = (int)totalCpuCounter.NextValue();
                        }
                        if (memoryBoxStatus)
                        {
                            processMemory = (int)(ramCounter.NextValue() / 1000000);
                            totalMemory = (int)totalRamCounter.NextValue();
                        }
                        else
                        {
                            processMemory = 0;
                            totalMemory = 0;
                        }
                        string time = currentTime.ToString("MM/dd/yyyy HH:mm:ss");
                        bool notRespond = false;
                        if (!process.Responding)
                        {
                            notRespond = true;
                        }
                        currentSize = directory.EnumerateFiles().Sum(file => file.Length);
                        sizeIncrease = currentSize - previousSize;
                        line = $"{time},{processCpu},{totalCpu},{processMemory},{totalMemory},{sizeIncrease},{notRespond}";
                        data = new Data(line);
                       
                        dataSet.Add(data);
                        dataWriter.WriteData(dataSet); ;
                        previousSize = currentSize;
                        Thread.Sleep(1000);

                    }
                };
                subs = Task.Factory.StartNew(subscribe, cancelationToken);
            };
        }

        public void Unsubscribe(Process process)
        {
            CancellationTokenSource token;
            if (ListenedProcesses.TryGetValue(process, out token))
            {
                token.Cancel();
                MessageBox.Show("Listen is stopped!");
            }

        }
    }
}

