using System.Diagnostics;
using Listener;
using Core;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StressTest
{
    public class ProcessTest
    {
        ProcessListener listener = new ProcessListener();
        LogFile logFileSmallSize = new LogFile(@"D:\logFiles\testSmall.txt");
        LogFile logFileBigSize = new LogFile(@"D:\logFiles\testBig.txt");
        LogFile logFileVideo = new LogFile(@"D:\logFiles\testVideo.txt");
        DirectoryInfo directory;
        Process process = Process.GetProcessById(6376);

        [TestCase]
        public void SmallSizePhotoFileTest()
        {
            
            directory = new DirectoryInfo(@"D:\destinationSmallSize");
            listener.Subscribe(logFileSmallSize,directory, process, true, true);
            ProcessStartInfo processToRunInfo = new ProcessStartInfo();
            processToRunInfo.Arguments = @"/C xcopy /b D:\sourceSmallSize D:\destinationSmallSize /I /Y";
            processToRunInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processToRunInfo.FileName = "cmd.exe";
            Process proc = new Process();
            proc.StartInfo = processToRunInfo;
            proc.Start();
            proc.WaitForExit();
           
        }
        [TestCase]
        public void BigSizePhotoFileTest()
        {
            directory = new DirectoryInfo(@"D:\destinationBigSize");
            listener.Subscribe(logFileBigSize, directory, process, true, true);
            ProcessStartInfo processToRunInfo = new ProcessStartInfo();
            processToRunInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processToRunInfo.FileName = "cmd.exe";
            processToRunInfo.Arguments = @"/C xcopy /b D:\sourceBigSize D:\destinationBigSize /I /Y ";
            Process proc = new Process();
            proc.StartInfo = processToRunInfo;
            proc.Start();
            proc.WaitForExit();
        }
        [TestCase]
        public void VideoFileTest()
        {
            directory = new DirectoryInfo(@"D:\destinationVideo");
            listener.Subscribe(logFileVideo,directory, process, true, true);
            ProcessStartInfo processToRunInfo = new ProcessStartInfo();
            processToRunInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processToRunInfo.FileName = "cmd.exe";
            processToRunInfo.Arguments = @"/C xcopy /b D:\sourceVideo D:\destinationVideo /I /Y ";
            Process proc = new Process();
            proc.StartInfo = processToRunInfo;
            proc.Start();
            proc.WaitForExit();
        }
    }
}
