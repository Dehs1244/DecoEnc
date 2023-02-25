using DecoEnc.Objects;
using Microsoft.Win32;
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
using DecoEnc.Objects;
using DecoEnc.Utils;
using System.IO;

namespace DecoEnc.MVM.View
{
    /// <summary>
    /// Логика взаимодействия для DecryptFileView.xaml
    /// </summary>
    public partial class DecryptFileView : UserControl
    {
        private CryptSettings _settings;
        private KeyCryptor _key;
        private DecryptFile _decryptor;
        private DecryptFileResult _result;

        public DecryptFileView()
        {
            InitializeComponent();
        }

        private void PackingFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.FileName = "Folder selection.";
            var chooseResult = dialog.ShowDialog() == true && !File.Exists(dialog.FileName);
            if (chooseResult)
            {
                _result.Write(dialog.FileName);
            }
            else
            {
                MessageBox.Show("Выберите каталог, куда будут сохранены дешифрованные файлы", "Каталог не выбран", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Decrypt(object sender, RoutedEventArgs e)
        {
            _result = _decryptor.Decrypt();
            PackageDecryptFileButton.IsEnabled = true;
        }

        private void LoadKeys(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Config files(*.ini)|*.ini";
            if (dialog.ShowDialog() == true)
            {
                IniConvert ini = new IniConvert(dialog.FileName);
                _key = new KeyCryptor(ini.Read("key", "CryptoKeys"));
                _settings = new CryptSettings() { Salt = ini.Read("salt", "CryptoSettings") };
                LoadFilesButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Выберите файл с ключом (все ключи находятся в папке \"keys\")", "Ключ не был выбран", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChooseFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.FileName = "Folder or file selection.";

            bool? resultShow = dialog.ShowDialog();
            if (resultShow == true)
            {
                _decryptor = new DecryptFile(_key, dialog.FileName) { Settings = _settings };
                DecryptButton.IsEnabled = true;
                MessageBox.Show("Файлы готовы к шифрованию", "Файлы получены", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else MessageBox.Show("Вы не выбрали файлы для шифрования", "Файлы для шифрования", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
