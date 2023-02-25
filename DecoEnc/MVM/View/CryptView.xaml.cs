using DecoEnc.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DecoEnc.Utils.Extensions;
using DecoEnc.Utils;

namespace DecoEnc.MVM.View
{
    /// <summary>
    /// Логика взаимодействия для CryptView.xaml
    /// </summary>
    public partial class CryptView : UserControl
    {
        public KeyCryptor KeyCrypt;

        public CryptView()
        {
            InitializeComponent();
        }

        private void Crypt(object sender, RoutedEventArgs e)
        {
            var keyTextBox = CryptFieldKey.FindStashDecoTextBox();
            var messageTextBox = CryptFieldMessage.FindStashDecoTextBox();
            var outputTextBox = CryptFieldOuput.FindStashDecoTextBox();

           if (string.IsNullOrEmpty(keyTextBox.Text))
           {
               MessageBox.Show("Поле с ключом пустое, ключ не может быть пустым или включите в настройках автогенерацию ключа", "Поле с ключом пустое", MessageBoxButton.OK, MessageBoxImage.Error);
               return;
           }
           if (string.IsNullOrEmpty(messageTextBox.Text))
           {
               MessageBox.Show("Введите сообщение для шифрования", "Поле с сообщением", MessageBoxButton.OK, MessageBoxImage.Error);
               return;
           }
            Cryptor cryptor = new Cryptor(keyTextBox.Text);
            var result = cryptor.Crypt(messageTextBox.Text);
            outputTextBox.Text = result.CryptedValue;
            KeyCrypt = new KeyCryptor(keyTextBox.Text);
            ButtonDownloadKey.IsEnabled = true;
            //MessageBox.Show(result.CryptedValue);
        }

        private void DownloadKey(object sender, RoutedEventArgs e)
        {
            ButtonDownloadKey.IsEnabled = false;
            KeyCrypt.Zip();
            MessageBox.Show("Архив с ключом создан в папке keys", "Ключ успешно упакован", MessageBoxButton.OK, MessageBoxImage.Information);
            KeyCrypt = new KeyCryptor();
        }
    }
}
