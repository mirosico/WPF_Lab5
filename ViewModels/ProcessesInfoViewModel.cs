using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;
using Lab5_Mysko.Models;
using Lab5_Mysko.Tools;

namespace Lab5_Mysko.ViewModels
{
    internal class ProcessesInfoViewModel : ObservableItem
    {
        private const int UpdateInterval = 2000;
        private const int RebuildInterval = 5000;

        private ICommand _stopProcessCommand, _openFolderCommand;
        private ObservableCollection<ReadableProcess> _processes;
        private ProcessThreadCollection _threads;
        private ProcessModuleCollection _modules;

        private ReadableProcess _selected;
        private readonly object _locker = new object();

        public Timer UpdateTimer { get; }
        public Timer RebuildTimer { get; }
        private ProcessesInfoModel Model { get; }


        public ProcessesInfoViewModel()
        {
            LinkedList<ReadableProcess> temp = new LinkedList<ReadableProcess>();
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    temp.AddLast(new ReadableProcess(process));
                }
                catch (Win32Exception)
                {
                }
            }

            _processes = new ObservableCollection<ReadableProcess>(temp);

            BindingOperations.EnableCollectionSynchronization(Processes, _locker);
            Model = new ProcessesInfoModel();

            RebuildTimer = new Timer(RebuildProcesses, null, RebuildInterval, RebuildInterval);
            UpdateTimer = new Timer(UpdateProcesses, null, UpdateInterval + 1000, UpdateInterval);
        }

        public ReadableProcess SelectedProcess
        {
            get => _selected;
            set
            {
                _selected = value;
                if (_selected != null)
                {
                    SelectedProcessThreads = SelectedProcess.Threads;
                    SelectedProcessModules = SelectedProcess.Modules;
                }

                OnPropertyChanged();
            }
        }

        public ProcessThreadCollection SelectedProcessThreads
        {
            get => _threads;
            private set
            {
                _threads = value;
                OnPropertyChanged();
            }
        }

        public ProcessModuleCollection SelectedProcessModules
        {
            get => _modules;
            private set
            {
                _modules = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ReadableProcess> Processes
        {
            get => _processes;
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }

        public ICommand StopProcessCommand
        {
            get
            {
                if (_stopProcessCommand == null)
                {
                    _stopProcessCommand = new RelayCommand(StopProcess, ProcessSelected);
                }

                return _stopProcessCommand;
            }
        }

        public ICommand OpenFolderCommand
        {
            get
            {
                if (_openFolderCommand == null)
                {
                    _openFolderCommand = new RelayCommand(OpenFolder, ProcessSelected);
                }

                return _openFolderCommand;
            }
        }

        private bool ProcessSelected(object obj) => SelectedProcess != null;

        private void OpenFolder(object obj)
        {
            Process.Start(SelectedProcess.FileLocation);
        }

        private void StopProcess(object obj)
        {
            SelectedProcess.MainProcess.Kill();
        }

        private void UpdateProcesses(object obj)
        {
            UpdateTimer.Change(Timeout.Infinite, Timeout.Infinite);
            Model.UpdateProcesses(Processes);

            if (SelectedProcess != null)
            {
                SelectedProcessThreads = SelectedProcess.Threads;
                SelectedProcessModules = SelectedProcess.Modules;
            }

            UpdateTimer.Change(0, UpdateInterval);
        }

        private void RebuildProcesses(object obj)
        {
            RebuildTimer.Change(Timeout.Infinite, Timeout.Infinite);
            Model.RebuildProcesses(Processes);
            RebuildTimer.Change(0, RebuildInterval);
        }
    }
}