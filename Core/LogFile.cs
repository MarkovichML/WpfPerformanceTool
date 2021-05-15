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
    public class LogFile : IDataInteraction<Data>
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

        public void WriteData(List<Data> dataSet)
        {
            lock (Locker)
            {
                using (FileStream bw = new FileStream(Path, FileMode.OpenOrCreate))
                {
                    byte[] dataBytes = Encoding.ASCII.GetBytes($"{string.Join("\n", dataSet)}");
                    bw.Write(dataBytes, 0, dataBytes.Length);
                }
            }
        }
        public List<Data> ReadData()
        {
            string line;
            List<Data> data = new List<Data>();
            lock (Locker)
            {
                using (StreamReader sr = new StreamReader(Path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                       Data dat = new Data(line);
                       data.Add(dat);
                    }
                }
            }
            return data;
        }
        
        #endregion
    }

}
