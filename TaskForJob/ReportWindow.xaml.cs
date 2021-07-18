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
using System.Windows.Shapes;
using System.Xml.Xsl;

namespace TaskForJob
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
            
        }

        //функция, которая трансформирует xml документ в html с помощью xslt и открывает в веб-браузере полученный html файл
        public void ShowReport(string path)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(@"D:\Programming\TaskForJob\TaskForJob\books.xslt");
            xslt.Transform(path, @"D:\Programming\TaskForJob\TaskForJob\books.html");
        
            wb.Navigate(@"D:\Programming\TestXslt\TestXslt\books.html");
        }
        //функция, которая открывает проводник и возвращает путь к выбранному файлу  
        public string GetPath()
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return null;
        }

        private void button_open_Click(object sender, RoutedEventArgs e)
        {
           string path =  GetPath();
            if(path!= null)
            {
                ShowReport(path);
            }
            else
            {
                show_info.IsOpen = true;
                info.Foreground = Brushes.Red;
                info.Text = "Откройте файл!";
            }

        }
    }
}
