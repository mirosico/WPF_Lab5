using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Lab5_Mysko.Models
{
    class ProcessesInfoModel
    {
        public void UpdateProcesses(ObservableCollection<ReadableProcess> processes)
        {
            for (var i = 0; i < processes.Count; i++)
            {
                if (processes[i].MainProcess != null)
                    processes[i].Update();
            }
        }

        public void RebuildProcesses(ObservableCollection<ReadableProcess> processes)
        {
            var running = new Dictionary<int, Process>();
            foreach (var process in Process.GetProcesses())
            {
                running.Add(process.Id, process);
            }

            for (var i = 0; i < processes.Count; i++)
            {
                var cur = processes[i];
                if (!running.ContainsKey(cur.Id))
                {
                    processes.RemoveAt(i);
                }
                else
                {
                    running.Remove(cur.Id);
                    cur.Update();
                }
            }

            foreach (var process in running.Values)
            {
                try
                {
                    processes.Add(new ReadableProcess(process));
                }
                catch (Win32Exception)
                {
                }
            }
        }
    }
}