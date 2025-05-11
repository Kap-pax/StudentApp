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

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtPesel.Text))
            {
                txtPesel.Background = Brushes.Red;
                return false;
                
            }
            else if (!(ValidatePesel(txtPesel.Text)))
            {
                return false;
            }




             if(string.IsNullOrEmpty(txtName.Text) && !(int.TryParse(txtName.Text, out _)))
            {
                txtName.Background = Brushes.Red;
                return false;
                
            }


             if (string.IsNullOrEmpty(txtSurname.Text) && !(int.TryParse(txtSurname.Text, out _)))
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
        private bool ValidatePesel(string pesel)
        {
            int[] w = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int s = 0;
            if (pesel.Length == 11) 
            {
                if (int.TryParse(pesel , out _))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        s += pesel[i] * w[i];
                    }
                    int o1 = s % 10;
                    int o2 = 10 - o1;
                    o1 = o2 % 10;
                    if (o1 == pesel[-1])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
