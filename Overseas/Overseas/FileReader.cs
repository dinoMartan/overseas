using System.Data;
using System.IO;
using ExcelDataReader;

namespace Overseas
{
    class FileReader
    {
        private string fileName;

        public FileReader(string fileName)
        {
            this.fileName = fileName;
        }

        // return DataTable of read file
        public DataTable readFile()
        {
            using (var stream = File.Open(this.fileName, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader reader;

                reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                //// reader.IsFirstRowAsColumnNames
                ExcelDataSetConfiguration conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };

                var dataSet = reader.AsDataSet(conf);

                // Now you can get data from each sheet by its index or its "name"
                var dataTable = dataSet.Tables[0];

                return dataTable;
            }
        }

    }
}
