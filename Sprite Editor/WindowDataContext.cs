using System.ComponentModel;
using System.Runtime.CompilerServices;
using SPE.Annotations;

namespace SPE
{
    public class WindowDataContext : INotifyPropertyChanged
    {
        private string _currentProgramStatus;
        private string _currentSystemTool;
        private bool _toggleCanvasGrid;
        private bool _modeAllColours;
        private bool _modeSystemColours;

        public string CurrentSystemTool
        {
            get => _currentSystemTool;
            set
            {
                _currentSystemTool = value;
                OnPropertyChanged();
            }
        }

        public bool ToggleCanvasGrid
        {
            get => _toggleCanvasGrid;
            set
            {
                _toggleCanvasGrid = value;
                OnPropertyChanged();
            }
        }

        public bool ModeAllColours
        {
            get => _modeAllColours;
            set
            {
                _modeAllColours = value;
                OnPropertyChanged();
            }
        }

        public bool ModeSystemColours
        {
            get => _modeSystemColours;
            set
            {
                _modeSystemColours = value;
                OnPropertyChanged();
            }
        }

        public string CurrentProgramStatus
        {
            get => _currentProgramStatus;
            set
            {
                _currentProgramStatus = value;
                OnPropertyChanged();
            }
        }

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
