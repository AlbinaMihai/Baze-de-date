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
using System.Configuration;
using System.Data.Common;
namespace Proiect_Baze
{
    /// <summary>
    /// Interaction logic for ParentUpdateData.xaml
    /// </summary>
   
    public partial class ParentUpdateData : Window
    {
        #region String uri
        public string Nume { get; set; }
        public string NrTel { get; set; }
        public string Adresa { get; set; }
        public string Ocupatie { get; set; }
        public string Studii { get; set; }
        #endregion
        public ParentUpdateData()
        {
            InitializeComponent();            
            DataContext = this;

        }
        public int PID;
      public void InitializeString()
        {
            var SBD = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SBD.ConnectionString);
            connection.Open();
            string SelectText = "SELECT Parinti.Nume, Parinti.NrTelefon, Parinti.Studii, Parinti.Ocupatie, Parinti.Adresa FROM Parinti WHERE Id_Parinte = @IdParinte";
            SqlCommand SelectCMD= new SqlCommand(SelectText, connection);
            SelectCMD.Parameters.AddWithValue("@IdParinte", PID);
            DbDataReader Reader = SelectCMD.ExecuteReader();
            Reader.Read();
            if(Nume=="")
            Nume = (string)Reader["Nume"];
            if(NrTel=="")
            NrTel = (string)Reader["NrTelefon"];
            if(Adresa=="")
            Adresa = (string)Reader["Adresa"];
            if(Studii=="")
            Studii = (string)Reader["Studii"];
            connection.Close();


        }

      
        public void GetPid(int X)
        {
            PID = X;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitializeString();

            var SBD = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SBD.ConnectionString);
            connection.Open();
            string UpdateString = "UPDATE Parinti SET Nume = @NumeParinte, Ocupatie = @Ocupatie, NrTelefon = @NrTel, Adresa = @Adresa, Studii = @Studii  WHERE Id_Parinte = @IdParinte";
            SqlCommand UpdateCMD = new SqlCommand(UpdateString, connection);
            UpdateCMD.Parameters.AddWithValue("@NumeParinte", Nume);
            UpdateCMD.Parameters.AddWithValue("@Ocupatie", Ocupatie);
            UpdateCMD.Parameters.AddWithValue("@NrTel", NrTel);
            UpdateCMD.Parameters.AddWithValue("@Adresa", Adresa);
            UpdateCMD.Parameters.AddWithValue("@Studii", Studii);
            UpdateCMD.Parameters.AddWithValue("@IdParinte", PID);
            int Rez = UpdateCMD.ExecuteNonQuery();
            if (Rez == 1)
                MessageBox.Show("Schimbarea datelor a fost realizata cu succes!");
            connection.Close();
        }
    }
}
