using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Listener
{
    class MonitoringClass
    {
        PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        public int getCurrentCpuUsage()
        {
            return (int)cpuCounter.NextValue();
        }
        public int getAvailableRAM()
        {
            return (int)ramCounter.NextValue();
        }
        
    }
}
