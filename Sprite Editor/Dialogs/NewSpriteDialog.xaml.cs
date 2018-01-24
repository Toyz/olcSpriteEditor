﻿using System.Windows;
using System.Windows.Controls;

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
            NumberPickerWidth.Maxvalue = 256;
            NumberPickerHeight.Maxvalue = 256;
        }

        private void HandleButton_Clicked(object sender, RoutedEventArgs e)
        {
            var name = ((Button) sender).Name;

            switch (name)
            {
                case "CreateSprite":
                    _mainWindow.LoadedSprite = new Sprite(NumberPickerWidth.Value, NumberPickerHeight.Value);
                    _mainWindow.WindowDataContext.CurrentProgramStatus = $"Created {_mainWindow.LoadedSprite.Width}x{_mainWindow.LoadedSprite.Height} Sprite";
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
