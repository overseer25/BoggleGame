﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BoggleServer;

namespace Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] arguments = new String[2];
            arguments[0] = "" + 80;
            arguments[1] = @"..\..\..\dictionary.txt";
            ThreadPool.QueueUserWorkItem((x) => BoggleServer.BoggleServer.Main(arguments));

            System.Diagnostics.Process p1 = new System.Diagnostics.Process();
            p1.StartInfo.FileName = @"..\..\..\BoggleClient\bin\Debug\BoggleClient.exe";
            new Thread(() => p1.Start()).Start();

            System.Diagnostics.Process p2 = new System.Diagnostics.Process();
            p2.StartInfo.FileName = @"..\..\..\BoggleClient\bin\Debug\BoggleClient.exe";
            new Thread(() => p2.Start()).Start();

            Console.Read();
        }
    }
}
