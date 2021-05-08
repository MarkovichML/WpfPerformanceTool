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
    class Listen
    {
        private Task subs;
        private static DateTime previousTime;
        private static TimeSpan previousTotalProcessorTime;
        private static DateTime currentTime;
        private static TimeSpan currentTotalProcessorTime;
        public void Subscribe(LogFile path, Process process, bool cpuBoxStatus, bool memoryBoxStatus)
        {
            {
                Action subscribe = () =>
                {
                    LogFile writer = new LogFile();
                    List<double> cpu = new List<double>();
                    List<double> memory = new List<double>();
                    List<string> time = new List<string>();
                    List<int> totalMemory = new List<int>();
                    List<int> totalCpu = new List<int>();
                    while (true)
                    {
                        MonitoringClass mc = new MonitoringClass();
                        Process[] processes = Process.GetProcesses();
                        if (processes.Length == 0)
                        {
                            Console.WriteLine(process + " does not exist");
                        }
                        else
                        {
                            process = processes[0];
                            if (previousTime == null || previousTime == new DateTime())
                            {
                                previousTime = DateTime.Now;
                                previousTotalProcessorTime = process.TotalProcessorTime;
                            }
                            else
                            {
                                currentTime = DateTime.Now;
                                currentTotalProcessorTime = process.TotalProcessorTime;

                                double CPUUsage = (currentTotalProcessorTime.TotalMilliseconds - previousTotalProcessorTime.TotalMilliseconds) / currentTime.Subtract(previousTime).TotalMilliseconds;
                                CPUUsage *= 100;
                                cpu.Add(CPUUsage);
                                double mbMemory = process.PagedMemorySize64 / 1049304;
                                memory.Add(Math.Round(mbMemory));
                                totalCpu.Add(mc.getCurrentCpuUsage());
                                totalMemory.Add(mc.getAvailableRAM());
                                time.Add(currentTime.ToString("MM/dd/yyyy HH:mm:ss"));

                                previousTime = currentTime;
                                previousTotalProcessorTime = currentTotalProcessorTime;
                            }
                            if (!process.Responding)
                            {
                                MessageBox.Show("Process does not respond!");
                            }
                            if(process.HasExited == true)
                            {
                                MessageBox.Show("Process has exited!");
                                ClearFile(path);
                            }

                        }
                        if (cpuBoxStatus == true && memoryBoxStatus == true)
                            writer.WriteData(path, time, cpu, memory, totalCpu, totalMemory);
                        else if (cpuBoxStatus == true && memoryBoxStatus == false)
                            writer.WriteData(path, time, cpu, totalCpu);
                        else if (cpuBoxStatus == false && memoryBoxStatus == true)
                            writer.WriteData(path, time, memory, totalMemory);
                        else MessageBox.Show("Please, choose what parametres you want to listen");
                        Thread.Sleep(1000);
                    }
                };
                subs = Task.Factory.StartNew(subscribe);
            };
        }
        private void ClearFile(LogFile logFile)
        {
            File.WriteAllText(logFile.Path, string.Empty);
        }
        public void Unsubscribe(LogFile logFile,Process process)
        {
            
        }
    }
}

