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
        private List<JsonResponse> statusiPosiljaka = new List<JsonResponse>();
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

            /*
            Parallel.ForEach(dataTable.AsEnumerable(), row =>
                {
                    Console.WriteLine("ja sam novi thread");

                    // definira se stupac za broj paketa i dodaje u listu
                    string brojPosiljke = row["Broj paketa"].ToString();
                    if (row["Broj paketa"].ToString() == "") return;

                    downloadDataAndAddToList(brojPosiljke);
                    ispis();
                }
            );
            */
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
           
        }

        public void downloadDataAndAddToList(string brojPosiljke)
        {
            DataDownloader dataDownloader = new DataDownloader(brojPosiljke);
            JsonResponse jsonResponse = dataDownloader.getData();
            statusiPosiljaka.Add(jsonResponse);
        }

        public void ispis()
        {
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
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ispis();
        }
    }
}
