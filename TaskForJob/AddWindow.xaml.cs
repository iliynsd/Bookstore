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

namespace TaskForJob
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }
        
        //Функция которая возвращает экземпляр книги(считывая значения текстовых полей), если хотя бы одно текстовое поле  пустое, то возвращает null     
        public Book Record()
        {
            if (Title.Text != string.Empty && Authors.Text !=string.Empty  && Year.Text != string.Empty && Price.Text != string.Empty && Category.Text != string.Empty)
            {
                Book book = new Book
                {
                    Title = Title.Text,
                    Authors = Authors.Text,
                    Price = Price.Text,
                    Category = Category.Text,
                    Year = Year.Text
                };
                return book;
            }
            else return null;

        }
        private void   Button_add_book_Click(object sender, RoutedEventArgs e)
        {
            
            
            if(Record() == null)
            {
                Info.Text = "Запись не добавлена!";
                Info.Foreground = Brushes.Red;
                ShowInfo.IsOpen = true;
            }
            else
            {
                Record();
                System.Threading.Thread.Sleep(100);
                this.Close();
            }

            

        }
    }
}
