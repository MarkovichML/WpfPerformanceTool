using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Core
{
    public class LogFile : IDataInteraction
    {
        #region Public Constructors

        public LogFile(string path)
        {
            Path = path;
            Locker = new object();
        }
        public LogFile() { }


        #endregion
        #region Public Properties

        public string Path { get; }

        public object Locker { get; }

        #endregion
        #region Public Methods
        public void WriteData(LogFile logFile, List<string> time, List<double> memoryCounters, List<double> cpuCounters, List<int> totalCpu, List<int> totalMemory)
        {
            lock (logFile.Locker)
            {
                using (FileStream bw = new FileStream(logFile.Path, FileMode.OpenOrCreate))
                {
                    for (int i = 0; i < memoryCounters.Count; i++)
                    {
                        byte[] timeBytes = Encoding.ASCII.GetBytes($"{time[i]},");
                        bw.Write(timeBytes, 0, timeBytes.Length);
                        byte[] cpuBytes = Encoding.ASCII.GetBytes($"{cpuCounters[i]},");
                        bw.Write(cpuBytes, 0, cpuBytes.Length);
                        byte[] memoryBytes = Encoding.ASCII.GetBytes($"{memoryCounters[i]},");
                        bw.Write(memoryBytes, 0, memoryBytes.Length);
                        byte[] totalCpuBytes = Encoding.ASCII.GetBytes($"{totalCpu[i]},");
                        bw.Write(totalCpuBytes, 0, totalCpuBytes.Length);
                        byte[] totalMemoryBytes = Encoding.ASCII.GetBytes($"{totalMemory[i]}\n");
                        bw.Write(totalMemoryBytes, 0, totalMemoryBytes.Length);
                    }
                }
            }
        }
        public void WriteData(LogFile logFile, List<string> time, List<double> processCounters, List<int> totalCounter)
        {
            lock (logFile.Locker)
            {
                using (FileStream bw = new FileStream(logFile.Path, FileMode.OpenOrCreate))
                {
                    for (int i = 0; i < processCounters.Count; i++)
                    {
                        byte[] timeBytes = Encoding.ASCII.GetBytes($"{time[i]},");
                        bw.Write(timeBytes, 0, timeBytes.Length);
                        byte[] cpuBytes = Encoding.ASCII.GetBytes($"{processCounters[i]},");
                        bw.Write(cpuBytes, 0, cpuBytes.Length);
                        byte[] totalCpuBytes = Encoding.ASCII.GetBytes($"{totalCounter[i]},");
                        bw.Write(totalCpuBytes, 0, totalCpuBytes.Length);
                    }
                }
            }
        }
        public List<DateTime> ReadTime(LogFile logFile)
        {
            string line;
            List<string[]> lines = new List<string[]>();
            List<DateTime> tim = new List<DateTime>();
            char[] delimiter = new char[] { ',' };
            lock (logFile.Locker)
            {
                using (StreamReader sr = new StreamReader(logFile.Path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {

                        var l = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                        lines.Add(l);

                    }
                    foreach (string[] lin in lines)
                    {
                        tim.Add(DateTime.ParseExact(lin[0], "MM/dd/yyyy HH:mm:ss", null));
                    }
                }
            }
            return tim;
        }
        public List<double> ReadCpu(LogFile logFile)
        {
            string line;
            List<string[]> lines = new List<string[]>();
            List<double> nums = new List<double>();
            char[] delimiter = new char[] { ',' };
            lock (logFile.Locker)
            {
                using (StreamReader sr = new StreamReader(logFile.Path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {

                        var l = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                        lines.Add(l);

                    }
                    foreach (string[] lin in lines)
                    {
                        nums.Add(double.Parse(lin[1]));
                    }
                }
            }
            return nums;
        }
        public List<double> ReadMemory(LogFile logFile)
        {
            string line;
            List<string[]> lines = new List<string[]>();
            List<double> nums = new List<double>();
            char[] delimiter = new char[] { ',' };
            lock (logFile.Locker)
            {
                using (StreamReader sr = new StreamReader(logFile.Path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {

                        var l = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                        lines.Add(l);

                    }
                    foreach (string[] lin in lines)
                    {
                        nums.Add(double.Parse(lin[2]));
                    }
                }
            }
            return nums;
        }
        public List<int> ReadTotalCpu(LogFile logFile)
        {
            string line;
            List<string[]> lines = new List<string[]>();
            List<int> nums = new List<int>();
            char[] delimiter = new char[] { ',' };
            lock (logFile.Locker)
            {
                using (StreamReader sr = new StreamReader(logFile.Path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {

                        var l = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                        lines.Add(l);

                    }
                    foreach (string[] lin in lines)
                    {
                        nums.Add(int.Parse(lin[3]));
                    }
                }
            }
            return nums;
        }
        public List<int> ReadTotalMemory(LogFile logFile)
        {
            string line;
            List<string[]> lines = new List<string[]>();
            List<int> nums = new List<int>();
            char[] delimiter = new char[] { ',' };
            lock (logFile.Locker)
            {
                using (StreamReader sr = new StreamReader(logFile.Path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {

                        var l = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                        lines.Add(l);

                    }
                    foreach (string[] lin in lines)
                    {
                        nums.Add(int.Parse(lin[4]));
                    }
                }
            }
            return nums;
        }
        
        #endregion
    }

}
