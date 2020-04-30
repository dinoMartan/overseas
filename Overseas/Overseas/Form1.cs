using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Overseas
{
    public partial class Form1 : Form
    {
        private string selectedFile = "";
        private List<string> brojeviPosiljaka = new List<string>();

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
            if(selectedFile == "")
            {
                Alert.showAlert("Greška", "Nije odabrana datoteka");
                return;
            }

            FileReader fileReader = new FileReader(selectedFile);
            DataTable dataTable = fileReader.readFile();

            foreach (DataRow row in dataTable.Rows)
            {
                // definira se stupac za broj paketa i dodaje u listu
                brojeviPosiljaka.Add(row["Broj paketa"].ToString()); 
            }  

        }
    }
}
