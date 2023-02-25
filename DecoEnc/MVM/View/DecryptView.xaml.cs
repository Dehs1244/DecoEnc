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
using System.IO.Compression;
using DecoEnc.Utils;
using System.IO;

namespace DecoEnc.MVM.View
{
    /// <summary>
    /// Логика взаимодействия для DecryptView.xaml
    /// </summary>
    public partial class DecryptView : UserControl
    {
        private KeyCryptor _key;
        private CryptSettings _settings;

        public DecryptView()
        {
            InitializeComponent();
        }

        public void Decrypt(object sender, RoutedEventArgs e)
        {
            TextBox decryptFieldKey = DecryptFieldMessage.FindStashDecoTextBox();
            if (string.IsNullOrEmpty(decryptFieldKey.Text))
            {
                MessageBox.Show("Введите зашифрованное сообщение в поле", "Поле с сообщением пустое", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Decryptor decrypt = new Decryptor(_key) { CryptSettings = _settings };
            var result = decrypt.Decrypt(decryptFieldKey.Text);
            if (!result.Succesfull)
            {
                MessageBox.Show("Возможно вы загрузили неверный ключ или ввели неправильное зашифрованное сообщение", "Ошибка при дешифровании", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TextBox decryptFieldOuput = DecryptFieldOuput.FindStashDecoTextBox();
            decryptFieldOuput.Text = result.DecryptedValue;
        }

        public void ReadKey(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Config files(*.ini)|*.ini";
            if (dialog.ShowDialog() == true)
            {
                IniConvert ini = new IniConvert(dialog.FileName);
                _key = new KeyCryptor(ini.Read("key", "CryptoKeys"));
                _settings = new CryptSettings() { Salt = ini.Read("salt", "CryptoSettings") };
                DecryptButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Выберите файл с ключом (все ключи находятся в папке \"keys\")", "Ключ не был выбран", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
