using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DecoEnc.Utils
{
    static class TextBlockExtensions
    {
        public static string GetBackgroundText(DependencyObject obj)
        {
            return (string)obj.GetValue(BackgroundTextProperty);
        }

        public static void SetBackgroundText(DependencyObject obj, string value)
        {
            obj.SetValue(BackgroundTextProperty, value);
        }

        public static readonly DependencyProperty BackgroundTextProperty =
            DependencyProperty.RegisterAttached(
                "BackgroundText", typeof(string), typeof(TextBlockExtensions));

        public static TextBox FindStashDecoTextBox(this TextBox styledBox) => (TextBox)((ControlTemplate)((Setter)styledBox.Style.Setters[0]).Value).FindName("DecoTextBox", styledBox);
    }
}
