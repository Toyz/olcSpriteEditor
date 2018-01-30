using System.Windows.Controls;
using System.Windows.Input;

namespace SPE
{
    public static class WindowCommands
    {
        public static readonly RoutedCommand NewCommand = new RoutedUICommand("New", "NewSprite", typeof(MenuItem), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.N, ModifierKeys.Control)
        }));

        public static readonly RoutedCommand OpenCommand = new RoutedUICommand("Open", "OpenSprite", typeof(MenuItem), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.O, ModifierKeys.Control)
        }));

        public static readonly RoutedCommand ExportCommand = new RoutedUICommand("Export", "ExportSprite", typeof(MenuItem), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Alt)
        }));

        public static readonly RoutedCommand SaveCommand = new RoutedUICommand("Save", "SaveSprite", typeof(MenuItem), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.S, ModifierKeys.Control)
        }));

        public static readonly RoutedCommand SaveAsCommand = new RoutedUICommand("Save As", "SaveAsSprite", typeof(MenuItem), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift)
        }));

        public static readonly RoutedCommand ToggleGridCommand = new RoutedUICommand("Toggle Canvas Grid", "ToggleGridView", typeof(MenuItem), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.G, ModifierKeys.Control | ModifierKeys.Alt)
        }));

        public static readonly RoutedCommand ShowAllColours = new RoutedUICommand("Show All Colours", "ShowAllColours", typeof(MenuItem), null);

        public static readonly RoutedCommand ShowSystemColours = new RoutedUICommand("Show System Colours", "ShowSystemColours", typeof(MenuItem), null);

        public static readonly RoutedCommand ResetSystemSettings = new RoutedUICommand("Reset Settings", "ResetSystemSettings", typeof(MenuItem), null);
    }
}
