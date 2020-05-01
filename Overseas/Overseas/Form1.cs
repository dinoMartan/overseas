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
                Console.WriteLine("File pronaden");
                jsonResponsesList = Load<JsonResponse>(SAVED_LIST);
                addDataToGrid();
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

            // praznjenje liste

            mainDataGrid.DataSource = null;
            jsonResponsesList.Clear();
            mainDataGrid.Rows.Clear();

            // procitaj excel datoteku
            FileReader fileReader = new FileReader(selectedFile);
            DataTable dataTable = fileReader.readFile();

            Console.WriteLine("Preuzimanje...");
            // iteracija po redovima excel datoteke - PREUZIMANJE SVIH STATUSA POŠILJKI

            LoadingForm loadingForm = new LoadingForm();
            loadingForm.Show();
            Application.DoEvents();
            this.Enabled = false;

            

            Parallel.ForEach(dataTable.AsEnumerable(), row =>
                {
                    Console.WriteLine("ja sam novi thread");

                    // definira se stupac za broj paketa i dodaje u listu
                    string brojPosiljke = row["Broj paketa"].ToString();
                    if (row["Broj paketa"].ToString() == "") return;

                    downloadDataAndAddToList(brojPosiljke);
                }
            );

            loadingForm.Close();

            this.Enabled = true;
            if (File.Exists(SAVED_LIST))
            {
                File.Delete(SAVED_LIST);
            }
            Save(SAVED_LIST, jsonResponsesList);
            addDataToGrid();
            dateTimeFileCreatedLabel.Visible = false;
        }

        public void downloadDataAndAddToList(string brojPosiljke)
        {
            DataDownloader dataDownloader = new DataDownloader(brojPosiljke);
            JsonResponse jsonResponse = dataDownloader.getData();
            jsonResponsesList.Add(jsonResponse);
        }


        private void addDataToGrid()
        {
            mainDataGrid.DataSource = null;
            mainDataGrid.AutoGenerateColumns = true;
            mainDataGrid.Columns.Clear();

            mainDataGrid.DataSource = jsonResponsesList;

            rowColorSet();
            // prolaz kroz sve zapise
            for (int i = 0; i <= 40; i++)
            {
                if (i == 2 || i == 4 || i == 11) continue;
                mainDataGrid.Columns[i].Visible = false;
            }
            mainDataGrid.Columns[2].HeaderText = "Broj pošiljke";
            mainDataGrid.Columns[4].HeaderText = "Datum slanja";
            mainDataGrid.Columns[11].HeaderText = "Primatelj";

            mainDataGrid.Columns[2].Width = 150;
            mainDataGrid.Columns[4].Width = 150;
            mainDataGrid.Columns[11].Width = 150;
        }

        private void MainDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRow = e.RowIndex;
            if (selectedRow < 0) return;

            // prikazi oznake
            if(!statusDescriptionLabel.Visible) showAllLabels();

            countryLabel.Text = jsonResponsesList[selectedRow].Consignee.Country.ToString();
            postalCodeLabel.Text = jsonResponsesList[selectedRow].Consignee.PostalCode.ToString();
            cityLabel.Text = jsonResponsesList[selectedRow].Consignee.City.ToString();

            statusDescriptionLabel.Text = jsonResponsesList[selectedRow].StatusDescription.ToString();

            scanDateStringLabel.Text = jsonResponsesList[selectedRow].LastShipmentTrace.ScanDateString.ToString();
            ScanTimeStringLabel.Text = jsonResponsesList[selectedRow].LastShipmentTrace.ScanTimeString.ToString();
            statusNumberLabel.Text = jsonResponsesList[selectedRow].LastShipmentTrace.StatusNumber.ToString();

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

        private void rowColorSet()
        {
            foreach(DataGridViewRow row in mainDataGrid.Rows){
                int index = row.Index;
                JsonResponse jsonResponse = jsonResponsesList[index];
                if (jsonResponse.LastShipmentTrace.StatusNumber == 41) row.DefaultCellStyle.BackColor = Color.Green; // naplaćeno
                else if (jsonResponse.LastShipmentTrace.StatusNumber == 40) row.DefaultCellStyle.BackColor = Color.PaleGreen; // isporučeno
                else if (jsonResponse.LastShipmentTrace.StatusNumber == 260) row.DefaultCellStyle.BackColor = Color.Red; // povrat
                else row.DefaultCellStyle.BackColor = Color.Yellow;
            }
        }

        private void SetColorsButton_Click(object sender, EventArgs e)
        {
            rowColorSet();
        }
    }
}
