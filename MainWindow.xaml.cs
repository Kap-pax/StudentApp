using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentApp
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
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void New_Click(object sender, RoutedEventArgs e)
        {
            Window1 AddStudent = new Window1();
            AddStudent.ShowDialog();
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki CSV z separatorem (,) |*.csv|Pliki CSV z separatorem (;) |*.csv";
            openFileDialog.Title = "Otwórz plik CSV";

            if (openFileDialog.ShowDialog() == true)
            {
                mainList.Items.Clear();
                string delimiter = openFileDialog.FilterIndex == 1 ? "," : ";";
                var lines = File.ReadAllLines(openFileDialog.FileName, Encoding.UTF8);

                foreach (var line in lines)
                {
                    string[] parts = line.Split(delimiter);
                    Person Student = new()
                    {
                        m_PESEL = parts.ElementAtOrDefault(0),
                        m_Name = parts.ElementAtOrDefault(1),
                        m_SecName = parts.ElementAtOrDefault(2),
                        m_Surname = parts.ElementAtOrDefault(3)
                    };
                    mainList.Items.Add(Student);
                }
            }
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Pliki CSV z separatorem (,) |*.csv|Pliki CSV z separatorem (;) |*.csv";
            saveFileDialog.Title = "Zapisz jako plik CSV";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                string delimiter = ";";
                if (saveFileDialog.FilterIndex == 1)
                {
                    delimiter = ",";
                }

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (Person item in mainList.Items)
                    {
                        var row = $"{item.m_PESEL}{delimiter}{item.m_Name}" +
                            $"{delimiter}{item.m_SecName}{delimiter}{item.m_Surname}";
                        writer.WriteLine(row);
                    }
                }
            }
        }
        public class Person
        {
            public string? m_PESEL { get; set; }
            public string? m_Name { get; set; }
            public string? m_SecName { get; set; }
            public string? m_Surname { get; set; }
            public string? m_DateOfBirth { get; set; }
            public string? m_PhoneNumber { get; set; }
            public string? m_Location { get; set; }
            public string? m_City { get; set; }
            public string? m_Code { get; set; }
        }
    }
}