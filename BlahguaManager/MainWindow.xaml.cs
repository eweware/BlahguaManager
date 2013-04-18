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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Data;
using System.Net;
using System.Data.OleDb;
using System.IO;
using System.Collections.Specialized;

namespace BlahguaManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void DoSelectFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV files (.csv)|*.csv";

            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                FileNameBox.Text = filename;
                DoImportCSV(filename);
            }
        }

        public void DoImportCSV(string fileName) {           
           // DataTable newTable = Convert(fileName, ",");
            DataTable newTable = GetDataTableFromCsv(fileName, true);
            newTable.Columns.Add("Status");
            BlahDataTable.DataContext = newTable;
            BlahDataTable.ItemsSource = newTable.DefaultView;

        }

        static DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader)
        {
            string header = isFirstRowHeader ? "Yes" : "No";

            string pathOnly = System.IO.Path.GetDirectoryName(path);
            string fileName = System.IO.Path.GetFileName(path);

            string sql = @"SELECT * FROM [" + fileName + "]";

            using (OleDbConnection connection = new OleDbConnection(
                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                      ";Extended Properties=\"Text;HDR=" + header + "\""))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                dataTable.Locale = System.Globalization.CultureInfo.CurrentCulture;
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public static DataGridRow GetRow(DataGrid grid, int index)
        {
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

       

        private void DoImportBlahs(object sender, RoutedEventArgs e)
        {
            DataTable theTable = (DataTable)BlahDataTable.DataContext;

            if (theTable != null) 
            {
                for (int i = 0; i < theTable.Rows.Count; i++)
                {
                    DataRow curRow = theTable.Rows[i];
                    BlahImportItem curItem = new BlahImportItem(curRow);
                    string resultStr = curItem.ImportBlah();
                    if (resultStr == "ok")
                    {
                        curRow["status"] = resultStr;
                        GetRow(BlahDataTable, i).Background = new SolidColorBrush(Colors.Green);

                    }
                    else
                    {
                        curRow["status"] = resultStr;
                        GetRow(BlahDataTable, i).Background = new SolidColorBrush(Colors.Red);
                    }

                }

            }

        }

       

        

        

        
    }
}
