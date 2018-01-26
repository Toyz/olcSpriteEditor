using System.ComponentModel;
using System.Runtime.CompilerServices;
using SPE.Annotations;

namespace SPE
{
    public class WindowDataContext : INotifyPropertyChanged
    {
        private string _currentProgramStatus;

        public string CurrentProgramStatus
        {
            get => _currentProgramStatus;
            set
            {
                _currentProgramStatus = value;
                OnPropertyChanged();
            }
        }

        private string _currentSystemTool;

        public string CurrentSystemTool
        {
            get => _currentSystemTool;
            set
            {
                _currentSystemTool = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
