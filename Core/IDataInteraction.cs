using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IDataInteraction
    {
        void WriteData(LogFile logFile, List<string> time, List<double> memoryCounters, List<double> cpuCounters, List<int> totalCpu, List<int> totalMemory);
        void WriteData(LogFile logFile, List<string> time, List<double> processCounters, List<int> totalCounters);
        List<double> ReadCpu(LogFile logFile);
        List<DateTime> ReadTime(LogFile logFile);
        List<double> ReadMemory(LogFile logFile);
        List<int> ReadTotalCpu(LogFile logFile);
        List<int> ReadTotalMemory(LogFile logFile);
    }
}
