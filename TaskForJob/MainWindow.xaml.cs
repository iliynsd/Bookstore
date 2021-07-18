using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml;

namespace TaskForJob
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public XmlDocument xml;

        public ObservableCollection<Book> bookList;

        public string path = string.Empty;

        int indexOfRow = -1;

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


        //функция разбирает xml документ на составляющие, записывает их в экземпляр книги и добавляет в  список 
        public void XmlParse(string path, ObservableCollection<Book> bookList, XmlDocument xml)
        {
            xml.Load(path); //инициализация документа

            var xmlRoot = xml.DocumentElement; //получение корня
            if (xmlRoot != null)
            {
                if (xmlRoot.HasChildNodes)
                {
                    var xmlNodes = xmlRoot.ChildNodes; //получаем список дочерних узлов корня

                    foreach (XmlNode xmlNode in xmlNodes) //обходим все дочерние узлы корня
                    {
                        var title = string.Empty;
                        var authors = string.Empty;
                        var category = string.Empty;
                        var price = string.Empty;
                        var year = string.Empty;
                        if (xmlNode.Attributes != null)
                        {
                            category = xmlNode.Attributes.GetNamedItem("category")?.Value; //получаем значение атрибута
                        }

                        if (xmlNode.HasChildNodes)
                        {
                            var xmlElements = xmlNode.ChildNodes; //получаем список элементов в узле
                            foreach (XmlElement xmlElement in xmlElements) //обходим все элементы, копируя значения
                            {
                                if (xmlElement.Name == "title")
                                {
                                    title = xmlElement.InnerText;
                                }

                                if (xmlElement.Name == "year")
                                {
                                    year = xmlElement.InnerText;
                                }

                                if (xmlElement.Name == "author")
                                {
                                    authors += (xmlElement.InnerText);
                                    authors += ';';
                                    authors += '\n';
                                }

                                if (xmlElement.Name == "price")
                                {
                                    price = xmlElement.InnerText;
                                }
                            }
                        }

                        //Создаем экземпляр класса и инициализируем его полученными данными
                        Book book = new Book()
                        {
                            Title = title,
                            Authors = authors,
                            Category = category,
                            Year = year,
                            Price = price
                        };

                        bookList.Add(book); //добавляем экземпляр класса в список
                    }
                }
            }
        }

        //функция добавляет узел(book) в корень xml документа
        public void XmlAdd(string path, Book book, XmlDocument xml)
        {
            int numOfAuth = 0;
            for (int i = 0; i < book.Authors.Length; i++) //подсчет количества авторов
            {
                if (book.Authors[i] == ';')
                {
                    numOfAuth++;
                }
            }

            xml.Load(path); //инициализируем документ
            XmlElement xRoot = xml.DocumentElement; //получаем корень


            //создаем новый элемент book
            var bookElem = xml.CreateElement("book");
            //создаем новый атрибут category
            var categoryAttr = xml.CreateAttribute("category");
            //создаем новый элемент title
            var titleElem = xml.CreateElement("title");
            //создаем новый атрибут lang
            var langAttr = xml.CreateAttribute("lang");
            //создаем новые элементы year и price
            var yearElem = xml.CreateElement("year");
            var priceElem = xml.CreateElement("price");

            //создаём текстовые значения для элементов и атрибутов
            var categoryText = xml.CreateTextNode(book.Category);
            var titleText = xml.CreateTextNode(book.Title);
            var langText = xml.CreateTextNode("en");
            var yearText = xml.CreateTextNode(book.Year);
            var priceText = xml.CreateTextNode(book.Price);
            var authorsText = new List<XmlText>();

            string auth = string.Empty;
            for (int j = 0; j < book.Authors.Length; j++)
            {
                if (book.Authors[j] != ';')
                {
                    auth += book.Authors[j];
                }
                else
                {
                    j++;
                    authorsText.Add(xml.CreateTextNode(auth));
                    auth = String.Empty;
                }
            }

            //добавляем узлы
            categoryAttr.AppendChild(categoryText);
            titleElem.AppendChild(titleText);
            langAttr.AppendChild(langText);
            yearElem.AppendChild(yearText);
            priceElem.AppendChild(priceText);

            bookElem.Attributes.Append(categoryAttr);
            titleElem.Attributes.Append(langAttr);
            bookElem.AppendChild(titleElem);

            for (int i = 0; i < numOfAuth; i++)
            {
                var authElem = xml.CreateElement("author");
                authElem.AppendChild(authorsText[i]);
                bookElem.AppendChild(authElem);
            }

            bookElem.AppendChild(yearElem);
            bookElem.AppendChild(priceElem);
            xRoot.AppendChild(bookElem);
            xml.Save(path);
        }


        //функция показа popup
        public void ShowError()
        {
            Info.Text = "Сначала откройте файл!";
            Info.Foreground = Brushes.Red;
            ShowInfo.IsOpen = true;
        }

        //обработчик события нажатия на кнопку button_open
        public void button_open_Click(object sender, RoutedEventArgs e)
        {
            path = string.Empty;
            xml = new XmlDocument(); //создаем новый экземпляр xml документа
            bookList = new ObservableCollection<Book>(); //создаём список книг
            grid.ItemsSource = null; //нужно чтобы при выборе другого xml старый список отвязался
            path = GetPath(); //получаем путь к xml документу
            XmlParse(path, bookList, xml); //инициализируем список
            grid.ItemsSource = bookList; //связываем datagrid со списком
        }

        //обработчик события нажатия на кнопку button_save
        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            if (path != string.Empty) //проверка проинизиализирован ли путь
            {
                XmlElement xRoot = xml.DocumentElement; //получение корня
                xRoot.RemoveAll(); //удаление дочерних узлов
                xml.Save(path); //сохранение документа

                foreach (var book in bookList) //обход все элементов списка и добавление их в xml
                {
                    XmlAdd(path, book, xml);
                }

                xml.Save(path); //сохранение документа


                Info.Text = "Файл сохранён!";
                Info.Foreground = Brushes.Green;
                ShowInfo.IsOpen = true;
            }
            else
            {
                ShowError();
            }
        }


        //обработчик события нажатия мышкой два раза по строчке в datagrid 
        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //записываем индекс строчки
            indexOfRow = grid.SelectedIndex;
        }

        //функция удаления элемента из списка
        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            if (path != string.Empty) //проверка выбран ли xml документ
            {
                if (indexOfRow == -1) //проверка был ли выбор строчки 
                {
                    Info.Text = "Выберите строчку!";
                    Info.Foreground = Brushes.Red;
                    ShowInfo.IsOpen = true;
                }
                else if (indexOfRow <= bookList.Count)
                {
                    //удаление элемента с нужным индексом из списка
                    bookList.Remove(bookList.ElementAt(indexOfRow));

                    Info.Text = "Удаление прошло успешно!";
                    Info.Foreground = Brushes.Green;
                    ShowInfo.IsOpen = true;
                }
            }
            else
            {
                ShowError();
            }
        }

        //функция добавления элемента в список 
        private void button_add_Click(object sender, RoutedEventArgs e)
        {
            if (path != string.Empty)
            {
                //вызов окна добавления
                AddWindow addWindow = new AddWindow();
                addWindow.ShowDialog();
                addWindow.Owner = this;


                if (
                    addWindow.Record() !=
                    null) // если все строчки в диалоговом окне были заполнены, то создается экземпляр класса книги и добавляется в список
                {
                    Book book = new Book();
                    book = addWindow.Record();
                    bookList.Add(book);
                    Info.Text = "Запись  добавлена!";
                    Info.Foreground = Brushes.Green;
                    ShowInfo.IsOpen = true;
                }
            }
            else
            {
                ShowError();
            }
        }


        //функция получения отчёта в html
        private void button_report_Click(object sender, RoutedEventArgs e)
        {
            //вызов окна получения отчёта
            ReportWindow reportWindow = new ReportWindow();
            reportWindow.ShowDialog();
            reportWindow.Owner = this;
        }
    }
}