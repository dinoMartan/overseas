using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Overseas
{
    public partial class Form1 : Form
    {
        private string selectedFile = "";
        private List<JsonResponse> statusiPosiljaka = new List<JsonResponse>();

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
            foreach (DataRow row in dataTable.Rows)
            {
                // definira se stupac za broj paketa i dodaje u listu
                string brojPosiljke = row["Broj paketa"].ToString();
                if (row["Broj paketa"].ToString() == "") continue;



                DataDownloader dataDownloader = new DataDownloader(brojPosiljke);
                JsonResponse jsonResponse = dataDownloader.getData();

                // dodavanje dobivenog statusa u listu svih statusa
                statusiPosiljaka.Add(jsonResponse);
            }

            ispis();




        }

        public void ispis()
        {
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
        }
    }
}
