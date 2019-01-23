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
using System.Collections.ObjectModel;

namespace Proiect_Baze
{


    /// <summary>
    /// Interaction logic for Invatator.xaml
    /// </summary>
    public partial class Invatator : Window
    {
        #region Variabile si proprietati
        public struct X
        {
            public string a;
            public string b;
            public string c;
        }
        public struct Obj
        {
            public int Id;
            public string Nume;
        }

        public List<Obj> Pereche = new List<Obj>();
        public List<Obj> PerecheDisciplina = new List<Obj>();
        public List<Obj> PerecheAbs = new List<Obj>();
        public List<Obj> PerecheDisciplinaAbs = new List<Obj>();
        public List<Obj> MediiDiscipline = new List<Obj>();
        public List<Obj> MediiElevi = new List<Obj>();
        public Invatator()
        {

            InitializeComponent();
            DataContext = this;
        }
        public int IID = 0;
        public int NrElevi;
        public string WelcomeMes { get; set; }
        public string CondicaData { get; set; }
        public string CondicaDisciplina { get; set; }
        public string CondicaTitlu { get; set; }
        public string DataNota { get; set; }
        public string NotaNume { get; set; }
        public string NotaDisciplina { get; set; }
        public string NotaNota { get; set; }
        public string NumeAbsenta { get; set; }
        public string DataAbsenta { get; set; }
        public string DisciplinaAbsenta1 { get; set; }
        public string MustrareNume { get; set; }
        public string MustrareText1 { get; set; }
        public string DataMustrare { get; set; }
        public string DisciplinaMustrare1 { get; set; }
        public struct MedieNote
        {
         public    int NrNote;
         public   int Suma;
        }
        MedieNote[,] A= new MedieNote[100, 100 ];

        #endregion
        public void GetIID(string Id)
        {
            IID = (Id[2] - '0') * 100 + (Id[3] - '0') * 10 + (Id[4] - '0');
            BuildMesString();
            CreateCatalog();
            AbsenteCatalog();
            VeziCondica();
        }
        public void BuildMesString()
        {
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var SelectCMD = connection.CreateCommand();
            SelectCMD.CommandType = CommandType.Text;
            SelectCMD.CommandText = "SELECT Nume,Prenume FROM Profesori where @IdInv = Id_Prof";
            SelectCMD.Parameters.AddWithValue("@IdInv", IID);
            DbDataReader Reader = SelectCMD.ExecuteReader();
            Reader.Read();
            WelcomeMes = "Bine ai venit, " + (string)Reader["Nume"] + " " + Reader["Prenume"] + "!";
            connection.Close();
        }
        private string GetCalificativ(int Nota)
        {
            string Calificativ;
            if (Nota == 10 || Nota == 9)
                Calificativ = "Fb";
            else
                if (Nota == 8 || Nota == 7)
                Calificativ = "B";
            else
                if (Nota == 6 || Nota == 5)
                Calificativ = "S";
            else
                if (Nota == 4 || Nota == 3)
                Calificativ = "I";
            else
                Calificativ = "Eroare";

            return Calificativ;
        }
        public void CreateCatalog()
        {
            #region MakeTable
            DataTable dt = new DataTable();
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var SelectCMD = connection.CreateCommand();
            SelectCMD.CommandType = CommandType.Text;
            SelectCMD.CommandText = "SELECT Discipline.Nume FROM ( Profesori INNER JOIN Clase ON Profesori.Id_Prof = Clase.Id_Prof INNER JOIN Discipline ON Clase.Clasa = Discipline.An ) WHERE Profesori.Id_Prof = @IdProf";
            SelectCMD.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderDiscipline = SelectCMD.ExecuteReader();
            dt.Columns.Add("Elev", typeof(string));
            while (ReaderDiscipline.Read())
            {
                dt.Columns.Add((string)ReaderDiscipline["Nume"], typeof(string));

            }
            connection.Close();

            connection.Open();
            var SelectCMD1 = connection.CreateCommand();
            SelectCMD1.CommandText = "SELECT Elevi.Nume,Elevi.Prenume FROM ( Profesori INNER JOIN Clase ON Clase.Id_Prof = Profesori.Id_Prof INNER JOIN Elevi ON Elevi.Id_Clasa = Clase.Crt_Nr ) WHERE Profesori.Id_Prof = @IdProf ORDER BY Elevi.Nume,Elevi.Prenume";

            SelectCMD1.Parameters.AddWithValue("@IdProf", IID);

            DbDataReader ReaderElevi = SelectCMD1.ExecuteReader();



            NrElevi = 0;
            while (ReaderElevi.Read())
            {


                DataRow dr = dt.NewRow();
                dr[0] = (string)ReaderElevi["Nume"] + " " + (string)ReaderElevi["Prenume"];
                dt.Rows.Add(dr);
                NrElevi++;


            }


            connection.Close();


            var SDB1 = ConfigurationManager.ConnectionStrings["SDB"];
            var con = new SqlConnection(SDB1.ConnectionString);
            con.Open();
            var SelectMarks = con.CreateCommand();
            SelectMarks.CommandType = CommandType.Text;
            SelectMarks.CommandText = "SELECT Note.Nota, Note.Data_Nota, Elevi.Nume, Elevi.Prenume, Discipline.Nume as Disciplina FROM ( Profesori INNER JOIN Note ON Note.Id_Prof = Profesori.Id_Prof INNER JOIN Elevi ON Elevi.Id_Elev = Note.Id_Copil INNER JOIN Discipline ON Discipline.Id_Disciplina = Note.Id_Disciplina ) WHERE Profesori.Id_Prof = @IdProf ORDER BY Elevi.Nume,Elevi.Prenume";
            SelectMarks.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderMarks = SelectMarks.ExecuteReader();
            #endregion
            int i = 0, j = 1;// i pt linie j pt coloana 
                             // pe coloana 0 sunt numele elevilor 
            while (ReaderMarks.Read())
            {
                string nm = (string)ReaderMarks["Nume"] + " " + (string)ReaderMarks["Prenume"];
                DataRow MarkRow = dt.NewRow();
                while ((dt.Rows[i][0].ToString() != (string)ReaderMarks["Nume"] + " " + (string)ReaderMarks["Prenume"]) && i < NrElevi)
                {

                    i++;
                }

                if ((dt.Rows[i][0].ToString() == (string)ReaderMarks["Nume"] + " " + (string)ReaderMarks["Prenume"]) && i < NrElevi)
                {
                    DateTime aux1 = (DateTime)ReaderMarks["Data_Nota"];
                    var aux = aux1.ToString("dd/MM");

                    dt.Rows[i][(string)ReaderMarks["Disciplina"]] = dt.Rows[i][(string)ReaderMarks["Disciplina"]] + "\n" + GetCalificativ((int)ReaderMarks["Nota"]) + " " + aux.ToString();
                }


                i = 0;



            }

            con.Close();
            Catalog.ItemsSource = dt.DefaultView;
            Catalog.MinColumnWidth = 100;
            Catalog.MinRowHeight = 120;

        }
        public void CondicaButton_Click(object sender, RoutedEventArgs e)
        {

            if (CondicaDisciplina == "")
            {
                MessageBox.Show("Nu ati trecut disciplina!");
                return;
            }
            if (CondicaTitlu == "")
            {
                MessageBox.Show("Nu ati trecut titlul!");
                return;
            }
            if (CondicaData == "")
            {
                MessageBox.Show("Nu ati trecut data!");
                return;
            }
            var SBD = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SBD.ConnectionString);
            connection.Open();
            string InsertString = "INSERT INTO Condica (Id_Prof, Data_Scriere, Disciplina, Titlu) VALUES (@IdInv, @Data, @Disciplina, @Titlu)";
            SqlCommand InsertCMD = new SqlCommand(InsertString, connection);
            InsertCMD.Parameters.AddWithValue("@IdInv", IID);
            InsertCMD.Parameters.AddWithValue("@Data", CondicaData);
            InsertCMD.Parameters.AddWithValue("@Disciplina", CondicaDisciplina);
            InsertCMD.Parameters.AddWithValue("@Titlu", CondicaTitlu);

            int Rez = InsertCMD.ExecuteNonQuery();
            if (Rez == 1)
                MessageBox.Show("Condica a fost scrisa!");
            else
                MessageBox.Show("Datele nu au fost incarcate!");
            connection.Close();


        }
        private void NumeElev_Loaded(object sender, RoutedEventArgs e)
        {

            List<string> NumeP = new List<string>();
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var SelectCMD1 = connection.CreateCommand();
            SelectCMD1.CommandText = "SELECT Elevi.Nume,Elevi.Prenume, Elevi.Id_Elev FROM ( Profesori INNER JOIN Clase ON Clase.Id_Prof = Profesori.Id_Prof INNER JOIN Elevi ON Elevi.Id_Clasa = Clase.Crt_Nr ) WHERE Profesori.Id_Prof = @IdProf ORDER BY Elevi.Nume,Elevi.Prenume";
            SelectCMD1.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderNames = SelectCMD1.ExecuteReader();
            Obj aux;
            while (ReaderNames.Read())
            {
                NumeP.Add((string)ReaderNames["Nume"] + " " + (string)ReaderNames["Prenume"]);
                aux.Id = (int)ReaderNames["Id_Elev"];
                aux.Nume = (string)ReaderNames["Nume"] + " " + (string)ReaderNames["Prenume"];
                Pereche.Add(aux);
            }
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = NumeP;
            comboBox.SelectedIndex = 0;
            connection.Close();
        }
        private void Nota_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> Note = new List<string>();
            Note.Add("Fb");
            Note.Add("B");
            Note.Add("S");
            Note.Add("I");
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = Note;
            comboBox.SelectedIndex = 0;

        }
        private void Disciplina_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> Discipline = new List<string>();
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var SelectCMD1 = connection.CreateCommand();
            SelectCMD1.CommandText = "SELECT Discipline.Nume, Discipline.Id_Disciplina FROM (Profesori INNER JOIN Clase ON Clase.Id_Prof = Profesori.Id_Prof INNER JOIN Discipline ON Discipline.An = Clase.Clasa ) WHERE Profesori.Id_Prof = @IdProf";
            SelectCMD1.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderDisc = SelectCMD1.ExecuteReader();
            Obj aux;
            while (ReaderDisc.Read())
            {

                Discipline.Add((string)ReaderDisc["Nume"]);
                aux.Id = (int)ReaderDisc["Id_Disciplina"];
                aux.Nume = (string)ReaderDisc["Nume"];
                PerecheDisciplina.Add(aux);
            }
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = Discipline;
            comboBox.SelectedIndex = 0;
        }
        private void WriteMark_Click(object sender, RoutedEventArgs e)
        {
            int NumarNota = 0;
            if (NotaNota == "Fb")
                NumarNota = 10;
            else
                if (NotaNota == "B")
                NumarNota = 8;
            else
                if (NotaNota == "S")
                NumarNota = 6;
            else
                if (NotaNota == "I")
                NumarNota = 4;
            else
                NumarNota = 1;
            Obj rezDis = PerecheDisciplina.Find(x => x.Nume == NotaDisciplina);
            Obj rezNume = Pereche.Find(x => x.Nume == NotaNume);
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var InsertCMD = connection.CreateCommand();
            InsertCMD.CommandText = "INSERT INTO Note (Id_Copil, Id_Prof, Data_Nota, Nota, Id_Disciplina) VALUES (@IdElev, @IdProf, @DataNota, @Nota, @IdDisciplina)";
            InsertCMD.Parameters.AddWithValue("@IdElev", rezNume.Id);
            InsertCMD.Parameters.AddWithValue("@IdProf", IID);
            InsertCMD.Parameters.AddWithValue("@DataNota", DataNota);
            InsertCMD.Parameters.AddWithValue("@Nota", NumarNota);
            InsertCMD.Parameters.AddWithValue("@IdDisciplina", rezDis.Id);
            int Sol = InsertCMD.ExecuteNonQuery();
            if (Sol == 1)
                MessageBox.Show("Nota a fost scrisa cu succes!");
            connection.Close();

        }
        public void AbsenteCatalog()
        {
            DataTable dt = new DataTable();
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var SelectCMD = connection.CreateCommand();
            SelectCMD.CommandType = CommandType.Text;
            SelectCMD.CommandText = "SELECT Discipline.Nume FROM ( Profesori INNER JOIN Clase ON Profesori.Id_Prof = Clase.Id_Prof INNER JOIN Discipline ON Clase.Clasa = Discipline.An ) WHERE Profesori.Id_Prof = @IdProf";
            SelectCMD.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderDiscipline = SelectCMD.ExecuteReader();
            dt.Columns.Add("Elev", typeof(string));
            while (ReaderDiscipline.Read())
            {
                dt.Columns.Add((string)ReaderDiscipline["Nume"], typeof(string));

            }
            connection.Close();

            connection.Open();
            var SelectCMD1 = connection.CreateCommand();
            SelectCMD1.CommandText = "SELECT Elevi.Nume,Elevi.Prenume, ELevi.Id_Elev FROM ( Profesori INNER JOIN Clase ON Clase.Id_Prof = Profesori.Id_Prof INNER JOIN Elevi ON Elevi.Id_Clasa = Clase.Crt_Nr ) WHERE Profesori.Id_Prof = @IdProf ORDER BY Elevi.Nume,Elevi.Prenume";

            SelectCMD1.Parameters.AddWithValue("@IdProf", IID);

            DbDataReader ReaderElevi = SelectCMD1.ExecuteReader();



            NrElevi = 0;
            while (ReaderElevi.Read())
            {


                DataRow dr = dt.NewRow();
                dr[0] = (string)ReaderElevi["Nume"] + " " + (string)ReaderElevi["Prenume"];
                dt.Rows.Add(dr);
                NrElevi++;


            }


            connection.Close();


            var SDB1 = ConfigurationManager.ConnectionStrings["SDB"];
            var con = new SqlConnection(SDB1.ConnectionString);
            con.Open();
            var SelectAbs = con.CreateCommand();
            SelectAbs.CommandType = CommandType.Text;
            SelectAbs.CommandText = " SELECT Absente.Data_Absenta, Discipline.Nume as Disciplina,Elevi.Nume as NumeElevi,Elevi.Prenume FROM (Profesori INNER JOIN Absente ON Profesori.Id_Prof = Absente.ID_Prof INNER JOIN Discipline ON Absente.Id_Disciplina = Discipline.Id_Disciplina INNER JOIN Elevi ON Absente.Id_Elev = Elevi.Id_Elev )WHERE Profesori.Id_Prof = @IdProf";
            SelectAbs.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderMarks = SelectAbs.ExecuteReader();

            int i = 0, j = 1;// i pt linie j pt coloana 
                             // pe coloana 0 sunt numele elevilor 
            while (ReaderMarks.Read())
            {
                
                DataRow MarkRow = dt.NewRow();
                while ((dt.Rows[i][0].ToString() != (string)ReaderMarks["NumeElevi"] + " " + (string)ReaderMarks["Prenume"]) && i < NrElevi)
                {

                    i++;
                }
                 
                if ((dt.Rows[i][0].ToString() == (string)ReaderMarks["NumeElevi"] + " " + (string)ReaderMarks["Prenume"]) && i < NrElevi)
                {
                    DateTime aux1 = (DateTime)ReaderMarks["Data_Absenta"];
                    var aux = aux1.ToString("dd/MM");

                    dt.Rows[i][(string)ReaderMarks["Disciplina"]] = dt.Rows[i][(string)ReaderMarks["Disciplina"]] + "\n" + aux.ToString();
                }


                i = 0;



            }

            con.Close();
            Absente.ItemsSource = dt.DefaultView;
            Absente.MinColumnWidth = 100;
            Absente.MinRowHeight = 120;
        }
        private void NumeElevAbs_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> NumeP = new List<string>();
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var SelectCMD1 = connection.CreateCommand();
            SelectCMD1.CommandText = "SELECT Elevi.Nume,Elevi.Prenume, Elevi.Id_Elev FROM ( Profesori INNER JOIN Clase ON Clase.Id_Prof = Profesori.Id_Prof INNER JOIN Elevi ON Elevi.Id_Clasa = Clase.Crt_Nr ) WHERE Profesori.Id_Prof = @IdProf ORDER BY Elevi.Nume,Elevi.Prenume";
            SelectCMD1.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderNames = SelectCMD1.ExecuteReader();
            Obj aux;
            while (ReaderNames.Read())
            {
                NumeP.Add((string)ReaderNames["Nume"] + " " + (string)ReaderNames["Prenume"]);
                aux.Id = (int)ReaderNames["Id_Elev"];
                aux.Nume = (string)ReaderNames["Nume"] + " " + (string)ReaderNames["Prenume"];
                PerecheAbs.Add(aux);
            }
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = NumeP;
            comboBox.SelectedIndex = 0;
            connection.Close();
        }
        private void DisciplinaAbsenta_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> Discipline = new List<string>();
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var SelectCMD1 = connection.CreateCommand();
            SelectCMD1.CommandText = "SELECT Discipline.Nume, Discipline.Id_Disciplina FROM (Profesori INNER JOIN Clase ON Clase.Id_Prof = Profesori.Id_Prof INNER JOIN Discipline ON Discipline.An = Clase.Clasa ) WHERE Profesori.Id_Prof = @IdProf";
            SelectCMD1.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderDisc = SelectCMD1.ExecuteReader();
            Obj aux;
            while (ReaderDisc.Read())
            {

                Discipline.Add((string)ReaderDisc["Nume"]);
                aux.Id = (int)ReaderDisc["Id_Disciplina"];
                aux.Nume = (string)ReaderDisc["Nume"];
                PerecheDisciplinaAbs.Add(aux);
            }
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = Discipline;
            comboBox.SelectedIndex = 0;
        }
        private void WriteAbs_Click(object sender, RoutedEventArgs e)
        {
            Obj rezDis = PerecheDisciplinaAbs.Find(x => x.Nume == DisciplinaAbsenta1);
            Obj rezNume = PerecheAbs.Find(x => x.Nume == NumeAbsenta);
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var InsertCMD = connection.CreateCommand();
            InsertCMD.CommandText = "INSERT INTO Absente (Id_Elev, ID_Prof, Data_Absenta, Id_Disciplina) VALUES (@IdElev, @IdProf, @DataAbs, @IdDisciplina)";
            InsertCMD.Parameters.AddWithValue("@IdElev", rezNume.Id);
            InsertCMD.Parameters.AddWithValue("@IdProf", IID);
            InsertCMD.Parameters.AddWithValue("@DataAbs", DataAbsenta);
            
            InsertCMD.Parameters.AddWithValue("@IdDisciplina", rezDis.Id);
            int Sol = InsertCMD.ExecuteNonQuery();
            if (Sol == 1)
                MessageBox.Show("Absenta a fost scrisa cu succes!");
            connection.Close();
        }
        public void  VeziCondica()
        {
            DataTable dt = new DataTable();
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            var SelectCMD = connection.CreateCommand();
            SelectCMD.CommandType = CommandType.Text;
            SelectCMD.CommandText = "SELECT Condica.Disciplina, Condica.Titlu, Condica.Data_Scriere FROM ( Profesori INNER JOIN Condica ON Condica.Id_Prof = Profesori.Id_Prof ) WHERE Profesori.Id_Prof = @IdProf";
            SelectCMD.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderCondica = SelectCMD.ExecuteReader();
            dt.Columns.Add("Disciplina", typeof(string));
            dt.Columns.Add("Titlu", typeof(string));
            dt.Columns.Add("Data", typeof(string));
            while (ReaderCondica.Read())
            {

                DataRow dr = dt.NewRow();
                dr[0] = (string)ReaderCondica["Disciplina"];
                dr[1] = (string)ReaderCondica["Titlu"];              
                dr[2] = (string)ReaderCondica["Data_Scriere"];
                dt.Rows.Add(dr);
                
            }
           Condica.ItemsSource = dt.DefaultView;
           Condica.MinColumnWidth = 100;
            Condica.MinRowHeight = 120;
            connection.Close();


            
        }
        private void WriteMustrare_Click(object sender, RoutedEventArgs e)
        {
            Obj rezDis = PerecheDisciplinaAbs.Find(x => x.Nume == DisciplinaMustrare1);
            Obj rezNume = Pereche.Find(x => x.Nume == MustrareNume);
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            
            var SelectCMD = connection.CreateCommand();
            var SelectIdMustrare = connection.CreateCommand();
            SelectIdMustrare.CommandText = "SELECT TOP 1 PERCENT Mustrari.Id_Mustrare FROM Mustrari ORDER BY Mustrari.Id_Mustrare DESC ";
            int IdMustrare = (int)SelectIdMustrare.ExecuteScalar();         
            int IdElev = rezNume.Id;


            var InsertCMD = connection.CreateCommand();
            InsertCMD.CommandText = "INSERT INTO Mustrari (Id_Mustrare, Id_Elev, Id_Disciplina, Id_Prof, Text, Data_Mustrare) VALUES (@IdMustrare, @IdElev, @IdDisciplina, @IdProf, @Text, @DataMustrare)";         
            InsertCMD.Parameters.AddWithValue("@IdElev", IdElev);
            InsertCMD.Parameters.AddWithValue("@IdProf", IID);
            InsertCMD.Parameters.AddWithValue("@DataMustrare", DataMustrare);
            InsertCMD.Parameters.AddWithValue("@IdMustrare", IdMustrare+1);
            InsertCMD.Parameters.AddWithValue("@Text",MustrareText1);
            InsertCMD.Parameters.AddWithValue("@IdDisciplina", rezDis.Id);
            int Sol = InsertCMD.ExecuteNonQuery();
            if (Sol == 1)
                MessageBox.Show("Mustrarea a fost scrisa cu succes!");
            connection.Close();
        }
    
        public void Medii()
        {
            DataTable dt = new DataTable();
            var SDB = ConfigurationManager.ConnectionStrings["SDB"];
            var connection = new SqlConnection(SDB.ConnectionString);
            connection.Open();
            #region Selectare disciplina

            var SelectCMD = connection.CreateCommand();
            SelectCMD.CommandType = CommandType.Text;
            SelectCMD.CommandText = "SELECT Discipline.Nume, Discipline.Id_Disciplina FROM ( Profesori INNER JOIN Clase ON Profesori.Id_Prof = Clase.Id_Prof INNER JOIN Discipline ON Clase.Clasa = Discipline.An ) WHERE Profesori.Id_Prof = @IdProf";
            SelectCMD.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderDiscipline = SelectCMD.ExecuteReader();
            dt.Columns.Add("Elev", typeof(string));
            while (ReaderDiscipline.Read())
            {
                dt.Columns.Add((string)ReaderDiscipline["Nume"], typeof(string));
                Obj aux;
                aux.Id = (int)ReaderDiscipline["Id_Disciplina"];
                aux.Nume = (string)ReaderDiscipline["Nume"];
                MediiDiscipline.Add(aux);

            }
            #endregion
            #region Selectare Elevi
            var SelectCMD1 = connection.CreateCommand();
            SelectCMD1.CommandText = "SELECT Elevi.Id_Elev, Elevi.Nume,Elevi.Prenume" +
                                     " FROM ( Profesori INNER JOIN Clase ON Clase.Id_Prof = Profesori.Id_Prof " +
                                     "INNER JOIN Elevi ON Elevi.Id_Clasa = Clase.Crt_Nr ) WHERE Profesori.Id_Prof = @IdProf ORDER BY Elevi.Nume,Elevi.Prenume";

            SelectCMD1.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderElevi = SelectCMD1.ExecuteReader();



            NrElevi = 0;
            while (ReaderElevi.Read())
            {
                Obj aux;
                aux.Id = (int)ReaderDiscipline["Id_Elev"];
                aux.Nume = (string)ReaderDiscipline["Nume"] +" "+ (string)ReaderDiscipline["Prenume"];
                MediiElevi.Add(aux);
                DataRow dr = dt.NewRow();
                dr[0] = (string)ReaderElevi["Nume"] + " " + (string)ReaderElevi["Prenume"];
                dt.Rows.Add(dr);
                NrElevi++;


            }

            #endregion
            connection.Close();


            var SDB1 = ConfigurationManager.ConnectionStrings["SDB"];
            var con = new SqlConnection(SDB1.ConnectionString);
            con.Open();
            var SelectMarks = con.CreateCommand();
            SelectMarks.CommandType = CommandType.Text;
            SelectMarks.CommandText = "SELECT Note.Nota,Elevi.Id_Elev, Discipline.Id_Disciplina FROM ( Profesori INNER JOIN Note ON Note.Id_Prof = Profesori.Id_Prof INNER JOIN Elevi ON Elevi.Id_Elev = Note.Id_Copil INNER JOIN Discipline ON Discipline.Id_Disciplina = Note.Id_Disciplina ) WHERE Profesori.Id_Prof = @IdProf ORDER BY Elevi.Nume,Elevi.Prenume";
            SelectMarks.Parameters.AddWithValue("@IdProf", IID);
            DbDataReader ReaderMarks = SelectMarks.ExecuteReader();

          while(ReaderMarks.Read())
            {

                A[(int)ReaderMarks["Id_Elev"], (int)ReaderMarks["Id_Disciplina"]].NrNote++;
                A[(int)ReaderMarks["Id_Elev"], (int)ReaderMarks["Id_Disciplina"]].Suma += (int)ReaderMarks["Nota"];

            }


            con.Close();
            Catalog.ItemsSource = dt.DefaultView;
            Catalog.MinColumnWidth = 100;
            Catalog.MinRowHeight = 120;
        }
    }
}
