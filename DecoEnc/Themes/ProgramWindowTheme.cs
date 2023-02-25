using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DecoEnc.Themes
{
    partial class ProgramWindowTheme
    {
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as System.Windows.Window).StateChanged += Window_StateChanged;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            System.Windows.Window me = (sender as System.Windows.Window);
            Button maximizeCaptionButton = me.Template.FindName("MaxRestoreButton", me) as Button;
            if (maximizeCaptionButton is not null)
            {
                maximizeCaptionButton.Content = me.WindowState == WindowState.Maximized ? "2" : "1";
            }
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            ((sender as FrameworkElement).TemplatedParent as System.Windows.Window).Close();
        }

        private void MaxRestoreClick(object sender, RoutedEventArgs e)
        {
            ((sender as FrameworkElement).TemplatedParent as System.Windows.Window)
                .WindowState = (((sender as FrameworkElement).TemplatedParent as System.Windows.Window)
                .WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            ((sender as FrameworkElement).TemplatedParent as System.Windows.Window)
                .WindowState = WindowState.Minimized;
        }
    }
}
