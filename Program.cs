using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ////create a csv file

            //comvert vsc to datatable
            var table = ConvertCSVtoDataTable("Exam2.csv");

            //get value of special column
            string[] columnName = { "Fail_Stage", "Location","1Test_Condition_ST1_Status"};

            for (int i = table.Columns.Count - 1; i >= 0; i--)
            {
                DataColumn dc = table.Columns[i];
                if (!columnName.Contains(dc.ColumnName))
                {
                    table.Columns.Remove(dc);
                }
            }

            WriteDataToFile(table, "datatable.csv");
        }
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static void WriteDataToFile(DataTable submittedDataTable, string submittedFilePath)
        {
            int i = 0;
            StreamWriter sw = null;

            sw = new StreamWriter(submittedFilePath, false);

            for (i = 0; i < submittedDataTable.Columns.Count - 1; i++)
            {

                sw.Write(submittedDataTable.Columns[i].ColumnName + ",");

            }
            sw.Write(submittedDataTable.Columns[i].ColumnName);
            sw.WriteLine();

            foreach (DataRow row in submittedDataTable.Rows)
            {
                object[] array = row.ItemArray;

                for (i = 0; i < array.Length - 1; i++)
                {
                    sw.Write(array[i].ToString() + ",");
                }
                sw.Write(array[i].ToString());
                sw.WriteLine();

            }

            sw.Close();
        }
    }
}
