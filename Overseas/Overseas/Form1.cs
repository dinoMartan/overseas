using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Overseas
{
    public partial class Form1 : Form
    {
        private const string SAVED_LIST = "SavedList.dat";
        private string selectedFile = "";
        private List<JsonResponse> jsonResponsesList = new List<JsonResponse>();


        public Form1()
        {
            InitializeComponent();
            hideAllLabels();
            if (File.Exists(SAVED_LIST))
            {
                // ako postoji datoteka, ucitaj je u jsonResponsesList i dodaj u main grid
                jsonResponsesList = Load<JsonResponse>(SAVED_LIST);
                addDataToGrid(this.jsonResponsesList);
                // stavi oznaku kada je kreirana trenutno ucitana datoteka
                DateTime dateTimeFileCreated = File.GetLastWriteTime(SAVED_LIST);
                dateTimeFileCreatedLabel.Text = "Popis ažuriran: " + dateTimeFileCreated.ToString();
            }
        }

        private void hideAllLabels()
        {
            statusDescriptionLabel.Visible = false;

            scanDateStringLabel.Visible = false;
            ScanTimeStringLabel.Visible = false;
            statusNumberLabel.Visible = false;

            countryLabel.Visible = false;
            postalCodeLabel.Visible = false;
            cityLabel.Visible = false;
        }

        private void showAllLabels()
        {
            statusDescriptionLabel.Visible = true;

            scanDateStringLabel.Visible = true;
            ScanTimeStringLabel.Visible = true;
            statusNumberLabel.Visible = true;

            countryLabel.Visible = true;
            postalCodeLabel.Visible = true;
            cityLabel.Visible = true;
        }

        // odaberi datoteku i njenu putanju spremi u selectedFile
        private void ChooseFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c\\";
            openFileDialog.Filter = "Excel files (*.xls, *.xlsx)|*.xls;*.xlsx";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFile = openFileDialog.FileName;
                chosenFileLable.Text = selectedFile;
            }
        }

        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            if (selectedFile == "")
            {
                Alert.showAlert("Greška", "Nije odabrana datoteka");
                return;
            }

            // praznjenje liste i datagrida
            mainDataGrid.DataSource = null;
            jsonResponsesList.Clear();
            mainDataGrid.Rows.Clear();

            // procitaj excel datoteku
            FileReader fileReader = new FileReader(selectedFile);
            DataTable dataTable = fileReader.readFile();
            
            // prikazi loading formu
            LoadingForm loadingForm = new LoadingForm();
            loadingForm.Show();
            Application.DoEvents();
            this.Enabled = false;

            // iteracija po redovima excel datoteke - PREUZIMANJE SVIH STATUSA POŠILJKI
            Parallel.ForEach(dataTable.AsEnumerable(), row =>
                {
                    if (row == null || row["Zadnji status"].ToString() == "") { return; }
                    // definira se stupac za broj paketa i dodaje u listu
                    string brojPosiljke = row["Broj paketa"].ToString();

                    if (row["Broj paketa"].ToString() == "" || row == null) return;

                    // preuzimanje podataka za broj posiljke
                    JsonResponse jsonResponse = downloadData(brojPosiljke);
                    jsonResponsesList.Add(jsonResponse);
                }
            );

            // uklanjanje loading forme
            loadingForm.Close();
            this.Enabled = true;

            // zapisivanje u datoteku
            if (File.Exists(SAVED_LIST))
            {
                File.Delete(SAVED_LIST);
            }
            Save(SAVED_LIST, jsonResponsesList);

            // dodaj podatke na main grid
            addDataToGrid(this.jsonResponsesList);

            // sakrij oznaku vremena kreiranja datoteke jer su preuzeti novi podatci
            dateTimeFileCreatedLabel.Visible = false;
        }

        // izdvojena funkcija radi mogucnosti pokretanja u vise threadova
        public JsonResponse downloadData(string brojPosiljke)
        {
            // preuzmi podatke o posiljci na temelju broja posiljke i vrati JsonResponse objekt
            DataDownloader dataDownloader = new DataDownloader(brojPosiljke);
            JsonResponse jsonResponse = dataDownloader.getData();
            // vrati preuzeti objekt
            return jsonResponse;
        }

        // postavi jsonResponseList kao izvod podataka
        private void addDataToGrid(List<JsonResponse> jsonResponsesList)
        {
            // ocisti main grid
            mainDataGrid.DataSource = null;
            mainDataGrid.AutoGenerateColumns = true;
            mainDataGrid.Columns.Clear();

            jsonResponsesList.Sort(
                delegate (JsonResponse p2, JsonResponse p1)
                {
                    return p1.SentOn.CompareTo(p2.SentOn);
                }
            );

            mainDataGrid.DataSource = jsonResponsesList;

            // oboji redove
            rowColorSet();

            // sakrij sve stupce osim 2(broj posiljke), 4(datum slanja), 11(primatelj)
            for (int i = 0; i <= 40; i++)
            {
                if (i == 2 || i == 4 || i == 11) continue;
                mainDataGrid.Columns[i].Visible = false;
            }

            // uredi main data grid
            mainDataGrid.Columns[2].HeaderText = "Broj pošiljke";
            mainDataGrid.Columns[4].HeaderText = "Datum slanja";
            mainDataGrid.Columns[11].HeaderText = "Primatelj";

            mainDataGrid.Columns[2].Width = 150;
            mainDataGrid.Columns[4].Width = 150;
            mainDataGrid.Columns[11].Width = 150;
        }

        private void MainDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // dohvati broj kliknutog retka
            int selectedRow = e.RowIndex;
            if (selectedRow < 0) return;

            // prikazi oznake
            if(!statusDescriptionLabel.Visible) showAllLabels();

            // prikazi podatke (labele)
              // sekcija 1
            countryLabel.Text = jsonResponsesList[selectedRow].Consignee.Country.ToString();
            postalCodeLabel.Text = jsonResponsesList[selectedRow].Consignee.PostalCode.ToString();
            cityLabel.Text = jsonResponsesList[selectedRow].Consignee.City.ToString();
              // sekcija 2
            statusDescriptionLabel.Text = jsonResponsesList[selectedRow].StatusDescription.ToString();
              // sekcija 3
            scanDateStringLabel.Text = jsonResponsesList[selectedRow].LastShipmentTrace.ScanDateString.ToString();
            ScanTimeStringLabel.Text = jsonResponsesList[selectedRow].LastShipmentTrace.ScanTimeString.ToString();
            statusNumberLabel.Text = jsonResponsesList[selectedRow].LastShipmentTrace.StatusNumber.ToString();

            // postavi detalje u detaljni grid i uredi detaljni grid
            detailsGrid.DataSource = jsonResponsesList[selectedRow].Collies[0].Traces;
            detailsGrid.Columns[0].Visible = false;
            detailsGrid.Columns[2].Visible = false;
            detailsGrid.Columns[3].Visible = false;
            detailsGrid.Columns[4].Visible = false;
            detailsGrid.Columns[8].Visible = false;

            detailsGrid.Columns[1].HeaderText = "Datum";
            detailsGrid.Columns[5].HeaderText = "Opis statusa";
            detailsGrid.Columns[6].HeaderText = "Naziv centra";
            detailsGrid.Columns[7].HeaderText = "Napomena";

            detailsGrid.Columns[1].Width = 100;
            detailsGrid.Columns[5].Width = 300;
            detailsGrid.Columns[6].Width = 150;
            detailsGrid.Columns[7].Width = 200;
        }

        // genericka funkcija za spremanje liste tipa T
        public static void Save<T>(string fileName, List<T> list)
        {
            // Gain code access to the file that we are going
            // to write to
            try
            {
                // Create a FileStream that will write data to file.
                using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, list);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //genericka funkcija za ucitavanje liste tipa T
        public static List<T> Load<T>(string fileName)
        {
            var list = new List<T>();
            // Check if we had previously Save information of our friends
            // previously
            if (File.Exists(fileName))
            {

                try
                {
                    // Create a FileStream will gain read access to the
                    // data file.
                    using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        var formatter = new BinaryFormatter();
                        list = (List<T>)
                            formatter.Deserialize(stream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return list;
        }

        // oboji redove u main gridu prema statusu posiljke
        private void rowColorSet()
        {
            foreach(DataGridViewRow row in mainDataGrid.Rows){
                int index = row.Index;
                JsonResponse jsonResponse = jsonResponsesList[index];
                if(jsonResponse == null) { continue; }
                if (jsonResponse.LastShipmentTrace.StatusNumber == 41) row.DefaultCellStyle.BackColor = Color.Green; // naplaćeno
                else if (jsonResponse.LastShipmentTrace.StatusNumber == 40) row.DefaultCellStyle.BackColor = Color.PaleGreen; // isporučeno
                else if (jsonResponse.LastShipmentTrace.StatusNumber == 260) row.DefaultCellStyle.BackColor = Color.Red; // povrat
                else if (jsonResponse.LastShipmentTrace.StatusNumber == 134)
                {
                    row.DefaultCellStyle.BackColor = Color.Black; // vraćeno
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (jsonResponse.LastShipmentTrace.StatusNumber == 8) row.DefaultCellStyle.BackColor = Color.White; // zaprimljena posiljka

                else row.DefaultCellStyle.BackColor = Color.Yellow;
            }
        }

        private void SetColorsButton_Click(object sender, EventArgs e)
        {
            rowColorSet();
        }

        private void GetAllShippmentsApiButton_Click(object sender, EventArgs e)
        {
            // otvori prozor i prikupi korisnicko ime i lozinku
            Validation validationForm = new Validation();
            validationForm.ShowDialog();

            string username = validationForm.username;
            string password = validationForm.password;

            // provjeri jesu li podatci upisani
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return;

            // dataDownloader ce pozvati ApiKeyFactory koji uz korisnicko ime i lozinku vraca api kljuc
            // funkcija getAllShipmentsAPI DataDownloadera od HttpRequesta koristi sendPostRequest()
            // s kojim se vraca JObject pa DataDownloader pretvara u listu <JsonResponse> i vraca tu listu
            DataDownloader dataDownloader = new DataDownloader(username, password);
            List<JsonResponse> allShippmentsApiList = dataDownloader.getAllShippmentsAPI();

            if(allShippmentsApiList == null)
            {
                Alert.showAlert("Greška", "SumTingWrung");
                return;
            }

            // ako su podatci stigli, ubaci ih u main grid
            this.jsonResponsesList = allShippmentsApiList;
            addDataToGrid(this.jsonResponsesList);

            // spremi novu listu
            if (File.Exists(SAVED_LIST))
            {
                File.Delete(SAVED_LIST);
            }
            Save(SAVED_LIST, this.jsonResponsesList);

        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            // praznjenje liste i datagrida
            // mainDataGrid.DataSource = null;
            // mainDataGrid.Rows.Clear();

            // prikazi loading formu
            LoadingForm loadingForm = new LoadingForm();
            loadingForm.Show();
            Application.DoEvents();
            this.Enabled = false;

            List<JsonResponse> updatedJsonResponseList = new List<JsonResponse>();
            // iteracija po redovima excel datoteke - PREUZIMANJE SVIH STATUSA POŠILJKI
            
            Parallel.ForEach(jsonResponsesList, jsonResponse =>
            {
                if (jsonResponse == null) { return; }
                // provjeri je li posiljka isporucena - ako je, preskoci dohvacanje
                if (jsonResponse.LastShipmentTrace.StatusNumber == 41 || jsonResponse.LastShipmentTrace.StatusNumber == 40) {
                    updatedJsonResponseList.Add(jsonResponse);
                    return;
                }

                // definira se stupac za broj paketa i dodaje u listu
                string brojPosiljke = jsonResponse.ShipmentNumber;
                // preuzimanje podataka za broj posiljke
                JsonResponse updatedJsonResponse = downloadData(brojPosiljke);
                updatedJsonResponseList.Add(updatedJsonResponse);
            }
            );

            jsonResponsesList = null;
            jsonResponsesList = updatedJsonResponseList;

            addDataToGrid(jsonResponsesList);

            // uklanjanje loading forme
            loadingForm.Close();
            this.Enabled = true;

            // zapisivanje u datoteku
            if (File.Exists(SAVED_LIST))
            {
                File.Delete(SAVED_LIST);
            }
            Save(SAVED_LIST, updatedJsonResponseList);

            // napisi u oznaku da je azurirano
            dateTimeFileCreatedLabel.Visible = true;
            dateTimeFileCreatedLabel.Text = "Ažurirano!";

        }

        private void TestButton_Click(object sender, EventArgs e)
        {

        }
    }
}
