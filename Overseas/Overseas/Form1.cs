using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Overseas
{
    public partial class Form1 : Form
    {
        private string selectedFile = "";
        private List<JsonResponse> jsonResponsesList = new List<JsonResponse>();
        List<Thread> downloadThreads = new List<Thread>();

        public Form1()
        {
            InitializeComponent();
        }

        private void ChooseFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "G:\\0 MOJE PUZZLE\\OVERSEAS\\excel";
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

            // drugi način sa vlastitim threadovima:
            /*
            foreach (DataRow row in dataTable.Rows)
            {
                // definira se stupac za broj paketa i dodaje u listu
                string brojPosiljke = row["Broj paketa"].ToString();
                if (row["Broj paketa"].ToString() == "") continue;

                string tmp = brojPosiljke;

                Thread thread = new Thread(() => this.downloadDataAndAddToList(tmp));
                downloadThreads.Add(thread);
                Console.WriteLine(brojPosiljke);
                thread.Start();

                //downloadDataAndAddToList(brojPosiljke);

            }

            */
           
        }

        public void downloadDataAndAddToList(string brojPosiljke)
        {
            DataDownloader dataDownloader = new DataDownloader(brojPosiljke);
            JsonResponse jsonResponse = dataDownloader.getData();
            jsonResponsesList.Add(jsonResponse);
        }

        public void ispis()
        {
            Console.WriteLine(jsonResponsesList.Count);
            addDataToGrid();

            /*
            foreach(Thread thread in downloadThreads)
            {
                if (thread.IsAlive)
                {
                    Alert.showAlert("Greška", "Podatci se još preuzimaju!");
                    return;
                }
            }

            foreach (JsonResponse jsonResponse in statusiPosiljaka)
            {
                IList<Colly> collyList;
                collyList = jsonResponse.Collies;

                IList<Trace> traceList;

                foreach (Colly colly in collyList)
                {
                    traceList = colly.Traces;
                    foreach (Trace trace in traceList)
                    {
                        Console.WriteLine(trace.ScanDateString + " " + trace.ScanTimeString + ": " + trace.StatusDescription);
                    }
                }

                Console.WriteLine(Environment.NewLine);
            }
            */
        }



        private void addDataToGrid()
        {
            mainDataGrid.DataSource = jsonResponsesList;
            
            // prolaz kroz sve zapise
            for(int i = 0; i <= 40; i++)
            {
                if (i == 2 || i == 4 || i == 11) continue;
                mainDataGrid.Columns[i].Visible = false;
            }
            mainDataGrid.Columns[2].HeaderText = "Broj pošiljke";
            mainDataGrid.Columns[4].HeaderText = "Datum slanja";
            mainDataGrid.Columns[11].HeaderText = "Primatelj";

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ispis();
        }

        private void MainDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRow = e.RowIndex;
            if (selectedRow < 0) return;

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
    }
}
