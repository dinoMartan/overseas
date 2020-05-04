using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
                Console.WriteLine("File pronaden");
                jsonResponsesList = Load<JsonResponse>(SAVED_LIST);
                addDataToGrid(this.jsonResponsesList);
                DateTime dateTimeFileCreated = File.GetCreationTime(SAVED_LIST);
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
                    // definira se stupac za broj paketa i dodaje u listu

                    string brojPosiljke = row["Broj paketa"].ToString();
                    if (row["Broj paketa"].ToString() == "") return;

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
                if (jsonResponse.LastShipmentTrace.StatusNumber == 41) row.DefaultCellStyle.BackColor = Color.Green; // naplaćeno
                else if (jsonResponse.LastShipmentTrace.StatusNumber == 40) row.DefaultCellStyle.BackColor = Color.PaleGreen; // isporučeno
                else if (jsonResponse.LastShipmentTrace.StatusNumber == 260) row.DefaultCellStyle.BackColor = Color.Red; // povrat
                else if (jsonResponse.LastShipmentTrace.StatusNumber == 134) row.DefaultCellStyle.BackColor = Color.Black; // vraćeno
                else if (jsonResponse.LastShipmentTrace.StatusNumber == 134) row.DefaultCellStyle.ForeColor = Color.White; // vraćeno

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

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return;

            DataDownloader dataDownloader = new DataDownloader(username, password);
            List<JsonResponse> allShippmentsApiList = dataDownloader.getAllShippmentsAPI();

            if(allShippmentsApiList == null)
            {
                return;
            }

            jsonResponsesList = allShippmentsApiList;
            addDataToGrid(this.jsonResponsesList);

        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            // praznjenje liste i datagrida
            mainDataGrid.DataSource = null;
            mainDataGrid.Rows.Clear();

            // prikazi loading formu
            LoadingForm loadingForm = new LoadingForm();
            loadingForm.Show();
            Application.DoEvents();
            this.Enabled = false;

            List<JsonResponse> updatedJsonResponseList = new List<JsonResponse>();
            // iteracija po redovima excel datoteke - PREUZIMANJE SVIH STATUSA POŠILJKI
            
            Parallel.ForEach(jsonResponsesList, jsonResponse =>
            {
                // definira se stupac za broj paketa i dodaje u listu
                string brojPosiljke = jsonResponse.ShipmentNumber;
                Console.WriteLine(brojPosiljke);
                // preuzimanje podataka za broj posiljke
                JsonResponse updatedJsonResponse = downloadData(brojPosiljke);
                updatedJsonResponseList.Add(updatedJsonResponse);
            }
            );

            Console.WriteLine("Updated list: " + updatedJsonResponseList);
            addDataToGrid(updatedJsonResponseList);

            // uklanjanje loading forme
            loadingForm.Close();
            this.Enabled = true;

            // zapisivanje u datoteku
            if (File.Exists(SAVED_LIST))
            {
                File.Delete(SAVED_LIST);
            }
            Save(SAVED_LIST, updatedJsonResponseList);

            // sakrij oznaku vremena kreiranja datoteke jer su preuzeti novi podatci
            dateTimeFileCreatedLabel.Visible = true;
            dateTimeFileCreatedLabel.Text = "Ažurirano!";

        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            // get metoda HttpRequesta
            /*
            NameValueCollection data = new NameValueCollection();
            data["username"] = "dino";
            data["password"] = "dino";
            data["getApiKey"] = "";

            HttpRequst httpRequst = new HttpRequst("getApiKey", data);
            JObject response = httpRequst.sendGetRequest();

            if (response == null)
            {
                Console.WriteLine("nema vrijednosti, vracen null");
            }

            //Console.WriteLine(response);
            */

            /*
            HttpRequst httpRequst1 = new HttpRequst(request: "getShipmentByShipmentNumber", shipmentNumber: "07074954");
            JObject response1 = httpRequst1.sendGetRequest();
            JsonResponse responseJson = response1.ToObject<JsonResponse>();
            */
        }
    }
}
