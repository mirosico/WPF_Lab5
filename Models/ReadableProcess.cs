using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Management;
using System.ComponentModel;
using Lab5_Mysko.Tools;


namespace Lab5_Mysko.Models
{
    class ReadableProcess : ObservableItem
    {
        private int _id;
        private string _name;
        private DateTime _startTime;
        private float _ramUsage;
        private double _cpuPercentage;
        private bool _active;
        private int _threadsCount;
        private string _userName;
        private string _location;

        private ProcessThreadCollection _threads;
        private ProcessModuleCollection _modules;


        public ReadableProcess(Process process)
        {
            MainProcess = process;
            UpdateValues();
        }

        public Process MainProcess { get; private set; }

        public int Id
        {
            get => _id;
            private set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public bool IsActive
        {
            get => _active;
            private set
            {
                _active = value;
                OnPropertyChanged();
            }
        }
        public double CpuPercentage
        {
            get => _cpuPercentage;
            private set
            {
                _cpuPercentage = value;
                OnPropertyChanged();
            }
        }
        public DateTime StartTime
        {
            get => _startTime;
            private set
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }

        public float RamUsage
        {
            get => _ramUsage;
            private set
            {
                _ramUsage = value;
                OnPropertyChanged();
            }
        }

        public int ThreadsCount
        {
            get => _threadsCount;
            private set
            {
                _threadsCount = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get => _userName;
            private set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string FileLocation
        {
            get => _location;
            private set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        public ProcessModuleCollection Modules
        {
            get => _modules;
            private set
            {
                _modules = value;
                OnPropertyChanged();
            }
        }

        public ProcessThreadCollection Threads
        {
            get => _threads;
            private set
            {
                _threads = value;
                OnPropertyChanged();
            }
        }

        private void UpdateValues()
        {
            Id = MainProcess.Id;
            IsActive = !MainProcess.HasExited;

            if (!IsActive)
            {
                RamUsage = 0;
                ThreadsCount = 0;
                Threads = null;
                Modules = null;
            }
            else
            {
                Name = MainProcess.ProcessName;
                StartTime = MainProcess.StartTime;
                CpuPercentage = GetCpuMeasure(MainProcess);
                RamUsage = (float) MainProcess.WorkingSet64 / 1024 / 1024;
                Threads = MainProcess.Threads;
                Modules = MainProcess.Modules;
                ThreadsCount = Threads.Count;
                UserName = GetProcessUserName(MainProcess.Id);
                FileLocation = Path.GetDirectoryName(MainProcess.MainModule.FileName);
            }
        }
        private static string GetProcessUserName(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = { "", "" };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[0];
                }
            }
            return "No owner";
        }
        private static double GetCpuMeasure(Process process)
        {
            var counter = ProcessCpuCounter.GetPerfCounterForProcessId(process.Id);
            // start capturing
            counter.NextValue();
            Thread.Sleep(200);
            var cpu = Math.Round(counter.NextValue() / (float)Environment.ProcessorCount,1);
            return cpu;
        }
        public void Update()
        {
            MainProcess.Refresh();
            UpdateValues();
        }
    }
}