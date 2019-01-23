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

    /*USE[SchoolDataBase];
     GO
     SELECT
 Profesori.Nume as Nume_Prof,
 Profesori.Prenume as Prenume_Prof,
 Profesori.Functie as Functie_Prof,
 Profesori.Nr_Tel  as Tel_Prof
 FROM
 (
 Parinti
 INNER JOIN Elevi ON Parinti.Id_Parinte= Elevi.Id_Elev
 INNER JOIN Clase ON Clase.Crt_Nr= Elevi.Id_Clasa
 INNER JOIN  Profesori ON Profesori.Id_Prof= Clase.Id_Prof
 )
 */
    public partial class Parinti : Window
    {
       
        public int PID;
       
        public string AbsString { get; set; }
        public struct AbsenteElevi
        {
           public  int ID;
            public string Nume;
            public string Prenume;
            public int NrAbsente;
        }
        public string DataProf{ get; set; }
        public string WelcomeMes { get; set; }
        public Parinti()
        {
            InitializeComponent();
           

        }
        public void GetId(string Id)
        {
            PID = (Id[2] - '0') * 100 + (Id[3] - '0') * 10 + (Id[4] - '0');
            BindingTableData();
            AbsElevi();
            GetProfData();
            GetParentName();
            DataContext = this;
        }
        public void GetParentName()
        {
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var SelectCMD = connection.CreateCommand();
            SelectCMD.CommandType = CommandType.Text;
            SelectCMD.CommandText = "select Parinti.Nume AS NumeParinte, Parinti.Prenume as PrenumeParinte FROM Parinti where Id_Parinte = @IdParinte";
            SelectCMD.Parameters.AddWithValue("@IdParinte", PID);
            DbDataReader Reader = SelectCMD.ExecuteReader();
            Reader.Read();
            WelcomeMes = "Bine ai venit, " + (string)Reader["NumeParinte"] + " " + Reader["PrenumeParinte"]+"!";
            connection.Close();
        }
      
     private void BindingTableData()
        {
            var SBD = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SBD.ConnectionString);
            connection.Open();


            string SelectQuery = "SELECT Elevi.Nume, Elevi.Prenume, Note.Nota, Note.Data_Nota, Discipline.Nume as Discipline FROM (Elevi  INNER JOIN Parinti ON Parinti.Id_Parinte = Elevi.Id_Parinte  INNER JOIN Note ON  Note.Id_Copil = Elevi.Id_Elev  Inner join Discipline on Discipline.Id_Disciplina = Note.Id_Disciplina   ) WHERE Parinti.Id_Parinte = @ParentId";
            SqlCommand SelectCommand = new SqlCommand(SelectQuery, connection);
            SelectCommand.Parameters.AddWithValue("@ParentId", PID);
            SqlDataAdapter da = new SqlDataAdapter(SelectCommand);
            DataTable NoteTable = new DataTable("Note");
            da.Fill(NoteTable);
            MarkGrid.ItemsSource = NoteTable.DefaultView;
         
            SelectQuery = "SELECT  Elevi.Nume,  Elevi.Prenume, Absente.Data_Absenta, Discipline.Nume as Disciplina,  Profesori.Nume as Nume_Prof, Profesori.Prenume as Prenume_Prof FROM(Elevi INNER JOIN Parinti ON Parinti.Id_Parinte = Elevi.Id_Parinte INNER JOIN Absente ON  Absente.Id_Elev = Elevi.Id_Elev inner join Discipline ON Discipline.Id_Disciplina = Absente.Id_Disciplina Inner join Clase on    Elevi.Id_Clasa = Clase.Crt_Nr inner join Profesori  ON Profesori.Id_Prof = Clase.Id_Prof) WHERE Parinti.Id_Parinte = @ParentId";
            SqlCommand AbsenteCMD = new SqlCommand(SelectQuery, connection);
            AbsenteCMD.Parameters.AddWithValue("@ParentId", PID);
            SqlDataAdapter DataAdapter = new SqlDataAdapter(AbsenteCMD);
            DataTable AbsenteTable = new DataTable("Absente");
            DataAdapter.Fill(AbsenteTable);
            PrezentaElevi.ItemsSource = AbsenteTable.DefaultView;

            SelectQuery = "SELECT Elevi.Nume as NumeElev, Elevi.Prenume as PrenumeElev, Mustrari.Text as ContinutMustrare, Discipline.Nume as NumeDisciplina, Mustrari.Data_Mustrare as DataMustrare FROM ( Parinti INNER JOIN  Elevi ON Elevi.Id_Parinte = Parinti.Id_Parinte INNER JOIN  Mustrari ON Mustrari.Id_Elev = Elevi.Id_Elev INNER JOIN Discipline ON Discipline.Id_Disciplina = Mustrari.Id_Disciplina ) WHERE Parinti.Id_Parinte = @IdParinte";
            var MustrariCMD = new SqlCommand(SelectQuery, connection);
            MustrariCMD.Parameters.AddWithValue("@IdParinte", PID);
            var DataAdp = new SqlDataAdapter(MustrariCMD);
            var MustrariTable = new DataTable("Mustrari");
            DataAdp.Fill(MustrariTable);
            Mustrari.ItemsSource = MustrariTable.DefaultView;
            connection.Close();
        }
     public void GetProfData()
      {
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var SelectCMD = connection.CreateCommand();
            SelectCMD.CommandType = CommandType.Text;
            SelectCMD.CommandText = "SELECT Profesori.Nume as NumeProf, Profesori.Prenume as PrenProf, Profesori.Functie, Profesori.Nr_Tel as NrTel FROM ( Parinti INNER JOIN Elevi On Elevi.Id_Parinte = Parinti.Id_Parinte INNER JOIN Clase ON Elevi.Id_Clasa = Clase.Crt_Nr INNER JOIN Profesori ON Profesori.Id_Prof = Clase.Id_Prof )WHERE @IdParinte=Parinti.Id_Parinte";
            SelectCMD.Parameters.AddWithValue("@IdParinte", PID);
            DbDataReader Reader = SelectCMD.ExecuteReader();
            while(Reader.Read())
            {
                DataProf = (string)Reader["Functie"] + " " + (string)Reader["NumeProf"] + " " + (string)Reader["PrenProf"] + "\nNumar de telefon: " + (string)Reader["NrTel"] + "\n";
            }
            connection.Close();
      }
     public void AbsElevi()
        {
            AbsenteElevi[] GestiuneAbs = new AbsenteElevi[30];

            int n = 0;
            int i;
            var SBD = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SBD.ConnectionString);
            connection.Open();
            var SelectCMD = connection.CreateCommand();
            SelectCMD.CommandType = CommandType.Text;
            SelectCMD.CommandText = "SELECT  Elevi.Nume as NumeElevi, Elevi.Prenume as PrenumeElevi,  Elevi.Id_Elev as IdElev FROM( Elevi   INNER JOIN Parinti ON Parinti.Id_Parinte = Elevi.Id_Parinte    INNER JOIN Absente ON  Absente.Id_Elev = Elevi.Id_Elev    )where Parinti.Id_Parinte=@IdParinte";
            SelectCMD.Parameters.AddWithValue("@IdParinte", PID);
            DbDataReader Reader = SelectCMD.ExecuteReader();
           
            Reader.Read();
            GestiuneAbs[n].ID = (int)Reader["IdElev"];
            GestiuneAbs[n].NrAbsente = 1;
            GestiuneAbs[n].Nume = (string)Reader["NumeElevi"];
            GestiuneAbs[n].Prenume = (string)Reader["PrenumeElevi"];
            n++;
            bool verif = false;
            while (Reader.Read())
            {
                
                for(i=0;i<n;i++)
                {
                    if ((int)Reader["IdElev"] == GestiuneAbs[i].ID)
                    {
                        GestiuneAbs[i].NrAbsente++;
                        verif = true;
                        break;
                    } 
                }

                if (!verif)
                {
                    GestiuneAbs[n].ID = (int)Reader["IdElev"];
                    GestiuneAbs[n].Nume = (string)Reader["NumeElevi"];
                    GestiuneAbs[n].Prenume = (string)Reader["PrenumeElevi"];
                    n++;
                    verif = false;
                }




            }//numara absenetele fiecarui copil

            for(i=0;i<n;i++)
            {
                AbsString = AbsString+ GestiuneAbs[i].Nume + " " + GestiuneAbs[i].Prenume + " are " + GestiuneAbs[i].NrAbsente.ToString() + " absente \n";
            }

            connection.Close();
           
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
         
            ParentUpdateData NewParentUpdateData = new ParentUpdateData();
            NewParentUpdateData.GetPid(PID);           
            NewParentUpdateData.Show();
            
        }
    }
}
