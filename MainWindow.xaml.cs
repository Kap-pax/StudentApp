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
            if (AddStudent.ShowDialog()==true)
            {
                Person a_Student = AddStudent.Student;
                mainList.Items.Add(a_Student);
                
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            
            if(mainList.SelectedItems != null)
            {
                for (int i = mainList.SelectedItems.Count - 1; i >= 0; i--)
                {
                    mainList.Items.Remove(mainList.SelectedItems[i]);
                }
            }
            else
            {
                MessageBox.Show("Wybierz pola do usunięcia.");
            }
        }
        private void Edit_click(object sender, RoutedEventArgs e)
        {
            if(mainList.SelectedItem is Person selected)
            {
                Window1 e_window = new Window1();
                e_window.LoadData(selected);
                if (e_window.ShowDialog() == true)
                {
                    int i = mainList.Items.IndexOf(selected);
                    mainList.Items[i]= e_window.Student;
                }
            }
                ///https://learn.microsoft.com/pl-pl/dotnet/csharp/language-reference/operators/type-testing-and-cast    is uzwywam jako sprawdzenie czy jest to typu 
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
                        m_Surname = parts.ElementAtOrDefault(3),
                        m_DateOfBirth = parts.ElementAtOrDefault(4),
                        m_PhoneNumber = parts.ElementAtOrDefault(5),
                        m_Location = parts.ElementAtOrDefault(6),
                        m_City = parts.ElementAtOrDefault(7),
                        m_Code = parts.ElementAtOrDefault(8)
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