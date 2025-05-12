using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static StudentApp.MainWindow;

namespace StudentApp
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        public Person Student { get; private set; }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

            if (!(ValidateInput()))
            {
                return;
               
            }
            string p_PESEL = txtPesel.Text.Trim();
            string p_Name = CapitalizeString(txtName.Text.Trim());
            string p_SecName = CapitalizeString(txtSecName.Text.Trim());
            string p_Surname = ValidateSurName(txtSurname.Text.Trim());
            string p_DateOfBirth = txtDateOfBirth.Text.Trim();
            string? p_PhoneNumber = FixPhoneNumber(txtNumber.Text.Trim());
            string p_Code = FixCode(txtCode.Text.Trim());
            string p_Location;
            p_Location = ValidateLocation(txtLocation.Text.Trim());
            string? p_City =ValidateLocation(txtCity.Text.Trim());


            
            Student = new Person
            {
                m_PESEL = p_PESEL,
                m_Name = p_Name,
                m_SecName = p_SecName,
                m_Surname = p_Surname,
                m_DateOfBirth = p_DateOfBirth,
                m_PhoneNumber = p_PhoneNumber,
                m_Location = p_Location,
                m_City = p_City,
                m_Code = p_Code
            };

            this.DialogResult = true;
        }
        /// <summary>
        /// metoda wczytuje dane z listy do pol widocznych w tym oknie
        public void LoadData(Person student)
        {
            txtPesel.Text = student.m_PESEL;
            txtName.Text = student.m_Name;
            txtSecName.Text = student.m_SecName;
            txtSurname.Text = student.m_Surname;
            txtDateOfBirth.Text = student.m_DateOfBirth;
            txtNumber.Text = student.m_PhoneNumber;
            txtLocation.Text = student.m_Location;
            txtCity.Text = student.m_City;
            txtCode.Text = student.m_Code;
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) && string.IsNullOrEmpty(txtPesel.Text) && string.IsNullOrEmpty(txtSurname.Text) && string.IsNullOrEmpty(txtDateOfBirth.Text) && string.IsNullOrEmpty(txtLocation.Text) && string.IsNullOrEmpty(txtNumber.Text) && string.IsNullOrEmpty(txtSecName.Text) && string.IsNullOrEmpty(txtCity.Text) && string.IsNullOrEmpty(txtCode.Text))
            {
                this.DialogResult = false;
            }
            else
            {
                if (MessageBox.Show("Czy chcesz wyjsc bez dodania uzytkownika?", "Potwierdź wyjście", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.DialogResult = false;
                }
            }

        }
        private bool ValidateInput()
        {
            if (AreRequierdChecked())
            {
                if (!(ValidatePesel( txtPesel.Text.Trim(), txtDateOfBirth.SelectedDate?.ToString("dd.MM.yyyy"))))
                {
                    txtPesel.Background = Brushes.Red;
                    return false;
                }
                else
                {
                    txtPesel.ClearValue(TextBox.BackgroundProperty);
                    return true;
                }
              
            }


            return false;
        }


        /// metoda ponizej sprawdza czy pola obowiazkowe sa wypelnione ( is null or empty) i sprawdza czy np. imie, nazwisko sa bez liczb
        private bool AreRequierdChecked()
        {
            if (string.IsNullOrEmpty(txtPesel.Text))
            {
                txtPesel.Background = Brushes.Red;
                return false;

            }

            if (string.IsNullOrEmpty(txtName.Text) || (int.TryParse(txtName.Text, out _)))
            {
                txtName.Background = Brushes.Red;
                return false;

            }

            if (string.IsNullOrEmpty(txtSurname.Text) || (int.TryParse(txtSurname.Text, out _)))
            {
                txtSurname.Background = Brushes.Red;
                return false;

            }

            if (string.IsNullOrEmpty(txtDateOfBirth.Text))
            {
                txtDateOfBirth.Background = Brushes.Red;
                return false;

            }

            if (string.IsNullOrEmpty(txtLocation.Text))
            {
                txtLocation.Background = Brushes.Red;
                return false;

            }

            if (string.IsNullOrEmpty(txtCity.Text))
            {
                txtCity.Background = Brushes.Red;
                return false;

            }

            if (string.IsNullOrEmpty(txtCode.Text))
            {
                txtCode.Background = Brushes.Red;
                return false;

            }
            return true;
        }


        private string FixCode(string code)
        {
            string numbers = Regex.Replace(code, "\\D", "");
            if (numbers.Length == 5)
            {
                return $"{numbers.Substring(0,2)}-{numbers.Substring(2)}";
            }
            else return string.Empty;
        }

        private string FixPhoneNumber(string phone)
        {
            string numbers = Regex.Replace(phone, "\\D", "");

            if (numbers.Length == 11 && numbers.StartsWith("48"))
                {
                return $"+48 {numbers.Substring(2)}";
                }
            else if (numbers.Length == 9)
            {
                return $"+48 {numbers}";
            }
            return string.Empty;
        }


        /// metoda ponizej sprawdza poprawnoc numeru pesel ( daty, czy tylko liczby, czy ma 11 znakow)  i zwraca true lub false w zaleznosci
        /// 
        private static bool ValidatePesel(string pesel, string date)
        {
            /// dlaczego long? bo int nie dziala, 1.5h nad tym siedzialem i long dziala wiec nie wnikam...
            if(pesel.Length!=11 || !long.TryParse(pesel, out _))
            {
                return false;
            }
            int[] w = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int s = 0;

            for(int i=0; i <10; i++)
            {
                s += (pesel[i] - '0') * w[i];
            }

            int control = (10 - s % 10) % 10;
            if(control == (pesel[10] - '0') && ValidatePeselDate(pesel, date))
            {
                return true;
            }
            else
            {
                return false;
            }        
            
                
        
        }

        private string ValidateSurName(string surName)
        {
            if (surName.Contains('-') || surName.Contains(' '))
            {
                string p0 = surName.ToLower();
                string[] parts = p0.Split('-', ' ');
                p0 = parts[0];
                string p1 = parts[1];
                string temp0 = CapitalizeString(p0);
                string temp1 = CapitalizeString(p1);
                surName = string.Join('-', temp0, temp1);

                return surName;
            }
            else
            {
                surName = CapitalizeString(surName);
                return surName;
            }
        }


        private string ValidateLocation(string location)
        {
            if (location.Contains('-') || location.Contains(' '))
            {
                string p0 = location.ToLower();
                char[] sep= {' ','-' };    
                string[] parts = p0.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                ///StringSplitOptions.RemoveEmptyEntries usuwa puste stringi , https://learn.microsoft.com/pl-pl/dotnet/api/system.stringsplitoptions?view=net-9.0
                ///

                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i]= CapitalizeString(parts[i]);
                }

                return string.Join(" ", parts);
            }
            else
            {
                location = CapitalizeString(location);
                return location;
            }
        }
        private string CapitalizeString(string str) 
        {
            string correct = char.ToUpper(str[0]) + str.Substring(1).ToLower();
            return correct;
        }
        
        private static bool ValidatePeselDate(string pesel , string date)
        {
            int pe_Year = int.Parse(pesel.Substring(0, 2));
            int pe_Month = int.Parse(pesel.Substring(2, 2));
            int pe_Day = int.Parse(pesel.Substring(4, 2));

            string[] temp0 = date.Split('.');
            int b_Day = int.Parse(temp0[0]);
            int b_Month = int.Parse(temp0[1]);
            int b_Year = int.Parse(temp0[2]);

            int b_YearC = int.Parse(temp0[2].Substring(2, 2));

            int a_Month = b_Month;
            if (2000 <= b_Year && b_Year <= 2099)
            {
                a_Month = b_Month + 20;
            }
            else if (1900 <= b_Year && b_Year <= 1999)
            {
                a_Month = b_Month;
            }
            else if (2100 <= b_Year && b_Year <= 2199)
            {
                a_Month = b_Month + 40;
            }
            else if (2200 <= b_Year && b_Year <= 2299)
            {
                a_Month = b_Month + 60;
            }

            if (b_YearC == pe_Year && a_Month == pe_Month && b_Day == pe_Day)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
