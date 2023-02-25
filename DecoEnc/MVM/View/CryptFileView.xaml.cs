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
using DecoEnc.Utils;
using Microsoft.Win32;
using System.IO;

namespace DecoEnc.MVM.View
{
    /// <summary>
    /// Логика взаимодействия для CryptFileView.xaml
    /// </summary>
    public partial class CryptFileView : UserControl
    {
        private CryptFile _cryptor;
        private KeyCryptor _keyCrypt;
        private CryptFileResult _resultCrypt;

        public CryptFileView()
        {
            InitializeComponent();
        }

        private void ChooseFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            TextBox cryptFieldKey = CryptFieldKey.FindStashDecoTextBox();

            if (string.IsNullOrEmpty(cryptFieldKey.Text))
            {
                MessageBox.Show("Поле с ключом пустое, ключ не может быть пустым или включите в настройках автогенерацию ключа", "Поле с ключом пустое", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.FileName = "Folder or file selection.";

            bool? resultShow = dialog.ShowDialog();
            if(resultShow == true)
            {
                _keyCrypt = new KeyCryptor(cryptFieldKey.Text);
                ButtonDownloadKey.IsEnabled = true;
                _cryptor  = new CryptFile(cryptFieldKey.Text, dialog.FileName);
                CryptButton.IsEnabled = true;
                TextFileCrypt.Foreground = new SolidColorBrush(Colors.Blue);
                TextCompleteCrypt.Visibility = Visibility.Hidden;
                TextCompleteFileWrite.Visibility = Visibility.Hidden;
                TextFileCrypt.Text = $"Файлов к шифрованию: {_cryptor.Count}";
                MessageBox.Show("Файлы готовы к шифрованию", "Файлы получены", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else MessageBox.Show("Вы не выбрали файлы для шифрования", "Файлы для шифрования", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CryptFiles(object sender, RoutedEventArgs e)
        {
            TextBox cryptFieldKey = CryptFieldKey.FindStashDecoTextBox();
            _resultCrypt = _cryptor.Crypt();
            _keyCrypt = new KeyCryptor(cryptFieldKey.Text);
            ButtonDownloadKey.IsEnabled = true;
            TextCompleteCrypt.Visibility = Visibility.Visible;
            PackageCryptFileButton.IsEnabled = true;
            MessageBox.Show("Файлы зашифрованы и готовы к упаковке в контейнер", "Файлы зашифрованы", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DownloadKey(object sender, RoutedEventArgs e)
        {
            ButtonDownloadKey.IsEnabled = false;
            _keyCrypt.Zip();
            _keyCrypt = new KeyCryptor();
            MessageBox.Show("Архив с ключом создан в папке keys", "Ключ успешно упакован", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PackageCryptFiles(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("containers")) Directory.CreateDirectory("containers");
            var path = $"containers/container_{DateTime.Now.ToString().Replace(":", ".")}";
            Directory.CreateDirectory(path);
            foreach(var file in _resultCrypt.Results)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(file.Result.CryptedValue);
                using (var writeStream = File.Create(path + $"/{file.FileName}.decenc"))
                {
                    writeStream.Write(buffer, 0, buffer.Length);
                    writeStream.Close();
                }
            }
            TextCompleteFileWrite.Text = $"Файлы записаны в контейнер: {path}";
            TextCompleteFileWrite.Visibility = Visibility.Visible;
            MessageBox.Show("Архив с ключом создан в папке keys", "Ключ успешно упакован", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
