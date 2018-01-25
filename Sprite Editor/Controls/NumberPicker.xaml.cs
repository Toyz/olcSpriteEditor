using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SPE.Controls
{
    /// <summary>
    /// Interaction logic for NumberPicker.xaml
    /// </summary>
    public partial class NumberPicker
    {
        public int Minvalue { get; set; } = 1;
        public int Maxvalue { get; set; } = 100;
        public int Startvalue { get; set; } = 10;

        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                _value = value.Clamp(Minvalue, Maxvalue);

                NUDTextBox.Text = _value.ToString();
            }
        }

        public NumberPicker()
        {
            InitializeComponent();
            Value = Startvalue;
            NUDTextBox.Text = Startvalue.ToString();
        }

        private void NUDButtonUP_Click(object sender, RoutedEventArgs e)
        {
            var number = NUDTextBox.Text != "" ? Convert.ToInt32(NUDTextBox.Text) : 0;
            if (number < Maxvalue)
                NUDTextBox.Text = Convert.ToString(number + 1);
        }

        private void NUDButtonDown_Click(object sender, RoutedEventArgs e)
        {
            var number = NUDTextBox.Text != "" ? Convert.ToInt32(NUDTextBox.Text) : 0;
            if (number > Minvalue)
                NUDTextBox.Text = Convert.ToString(number - 1);
        }

        private void NUDTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    NUDButtonUP.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(NUDButtonUP, new object[] { true });
                    break;
                case Key.Down:
                    NUDButtonDown.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(NUDButtonDown, new object[] { true });
                    break;
            }
        }

        private void NUDTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(NUDButtonUP, new object[] { false });
                    break;
                case Key.Down:
                    typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(NUDButtonDown, new object[] { false });
                    break;
            }
        }

        private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var number = 0;
            if (NUDTextBox.Text != "")
                if (!int.TryParse(NUDTextBox.Text, out number)) NUDTextBox.Text = Startvalue.ToString();
            if (number > Maxvalue) NUDTextBox.Text = Maxvalue.ToString();
            if (number < Minvalue) NUDTextBox.Text = Minvalue.ToString();
            NUDTextBox.SelectionStart = NUDTextBox.Text.Length;

            Value = int.Parse(NUDTextBox.Text);
        }
    }
}
