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

        // This is the default block size for the sprite
        private CellSize _spriteBlockSize;
        private int _selectedSpriteIndex;

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

        public CellSize SpriteBlockSize
        {
            get => _spriteBlockSize;
            set
            {
                _spriteBlockSize = value;
                OnPropertyChanged();
            }
        }

        public CellSize[] AllowedSpriteSizes => new[]
        {
            new CellSize("8 x 8", 8), new CellSize("16 x 16", 16), new CellSize("32 x 32", 32), new CellSize("64 x 64", 64),  
        };

        public int SelectedSpriteIndex
        {
            get => _selectedSpriteIndex;
            set
            {
                _selectedSpriteIndex = value;
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
