using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Data
    {
        public Data(string line)
        {
            string[] l = line.Split(',');
            Time = l[0];
            ProcessCpu = int.Parse(l[1]);
            TotalCpu = int.Parse(l[2]);
            ProcessMemory = int.Parse(l[3]);
            TotalMemory = int.Parse(l[4]);
            VolumeIncrease = double.Parse(l[5]);
            NotResponding = bool.Parse(l[6]);
            
        }

        public Data() { }
        #region Public Properties
        public int ProcessCpu { get; }
        public int TotalCpu { get; }
        public int ProcessMemory { get; }
        public int TotalMemory { get; }
        public string Time { get; }
        public double VolumeIncrease { get; }
        public bool NotResponding { get; }
        #endregion
        #region Public Methods

        public override string ToString()
        {
            return $"{Time},{ProcessCpu},{TotalCpu},{ProcessMemory},{TotalMemory},{VolumeIncrease},{NotResponding}";
        }

        #endregion
    }
}
