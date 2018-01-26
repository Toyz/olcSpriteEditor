using System.Windows;
using System.Windows.Controls;
using SPE.Engine;

namespace SPE.Dialogs
{
    /// <summary>
    /// Interaction logic for NewSpriteDialog.xaml
    /// </summary>
    public partial class NewSpriteDialog : Window
    {
        private readonly MainWindow _mainWindow;

        public NewSpriteDialog(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();

            DataContext = mainWindow.WindowDataContext;

            NumberPickerHeight.Value = Properties.Settings.Default.Create_H;
            NumberPickerWidth.Value = Properties.Settings.Default.Create_W;
        }

        private void HandleButton_Clicked(object sender, RoutedEventArgs e)
        {
            var name = ((Button) sender).Name;

            switch (name)
            {
                case "CreateSprite":
                    _mainWindow.LoadedSprite = new Sprite(NumberPickerWidth.Value, NumberPickerHeight.Value, _mainWindow);
                    Properties.Settings.Default.Create_H = _mainWindow.LoadedSprite.Height;
                    Properties.Settings.Default.Create_W = _mainWindow.LoadedSprite.Width;

                    _mainWindow.WindowDataContext.CurrentProgramStatus = $"Created {_mainWindow.LoadedSprite.Width}x{_mainWindow.LoadedSprite.Height} Sprite";

                    Properties.Settings.Default.Save();
                    DialogResult = true;
                    break;
                case "CancelSprite":
                    DialogResult = false;
                    break;
            }

            Close();
        }
    }
}
