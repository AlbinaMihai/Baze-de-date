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
using System.Data;
using System.Data.SqlClient;

namespace Proiect_Baze
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window 
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            var connection = new SqlConnection();
            connection.ConnectionString = "Server=.;Database=SchoolDataBase;Trusted_Connection=true";

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                string SelectQuery = "SELECT COUNT(1) FROM Utilizatori WHERE @CodUser=Cod";
                SqlCommand SelectCommand = new SqlCommand(SelectQuery, connection);
               SelectCommand.Parameters.AddWithValue("@CodUser",GetId.Password);

                int Count = Convert.ToInt32(SelectCommand.ExecuteScalar());

                if (Count != 1)
                    MessageBox.Show("Ati introdus GRESIT codul!");
                else
                    if (Count == 1)
                    {
                       connection.Close();

                      if (GetId.Password[0] == '0' && GetId.Password[1] == '1')   
                        {
                            Parinti NewParentWindow = new Parinti();
                            NewParentWindow.GetId(GetId.Password);
                            NewParentWindow.Show();
                        }
                    else
                      
                        if(GetId.Password[0] == '0' && GetId.Password[1] == '2')
                            {

                        Invatator NewInvWindow = new Invatator();

                        NewInvWindow.GetIID(GetId.Password);
                        NewInvWindow.Show();

                            }      
                    else
                        if(GetId.Password[0] == '0' && GetId.Password[1] == '3')
                        {

                        }
                    else
                        if (GetId.Password[0] == '0' && GetId.Password[1] == '4')
                        {

                        }
                    else
                        if (GetId.Password[0] == '1' && GetId.Password[1] == '3')
                        {

                        }
                    else
                        if (GetId.Password[0] == '1' && GetId.Password[1] == '4')
                        {

                        }
                    else
                        if (GetId.Password[0] == '2' && GetId.Password[1] == '4')
                    {

                    }



                }
                  
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            finally
            {
                connection.Close();
            }

            

        }
    }
}
