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
using System.Windows.Threading;

namespace BlahguaManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string CurrentFileName;
        DispatcherTimer dispatcherTimer;
        int curBlah;
        int curPerson;
        bool isImportingBlahs;

        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void DoSelectFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "Excel files (.xlsx)|*.xlsx";

            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                FileNameBox.Text = filename;
                //DoImportCSV(filename);
                DoImportExcel(filename);
            }
        }

        public void DoImportCSV(string fileName) {           
           // DataTable newTable = Convert(fileName, ",");
            DataTable newTable = GetDataTableFromCsv(fileName, true);
            newTable.Columns.Add("Status");
            BlahDataTable.DataContext = newTable;
            BlahDataTable.ItemsSource = newTable.DefaultView;
        }

        public void DoImportExcel(string fileName)
        {
            CurrentFileName = fileName;
            GetDataTableFromExcel(fileName);
            
        }

        public void GetDataTableFromExcel(string path)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(path);
            string fileName = System.IO.Path.GetFileName(path);

            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", path);

            var adapter = new OleDbDataAdapter("SELECT * FROM [Blahs$] WHERE CHANNEL IS NOT NULL", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds, "blahTable");

            DataTable newTable = ds.Tables["blahTable"];
            newTable.Columns.Add("Status");
            BlahDataTable.DataContext = newTable;
            BlahDataTable.ItemsSource = newTable.DefaultView;
            adapter.Dispose();

            var personaAdapter = new OleDbDataAdapter("SELECT * FROM [Personas$] Where USERNAME IS NOT NULL", connectionString);

            var personads = new DataSet();

            personaAdapter.Fill(personads, "personaTable");

            DataTable personTable = personads.Tables["personaTable"];
            personTable.Columns.Add("Status");
            PersonaDataTable.DataContext = personTable;
            PersonaDataTable.ItemsSource = personTable.DefaultView;
            personaAdapter.Dispose();
        }

        static DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader)
        {
            string header = isFirstRowHeader ? "Yes" : "No";

            string pathOnly = System.IO.Path.GetDirectoryName(path);
            string fileName = System.IO.Path.GetFileName(path);

            string sql = @"SELECT * FROM [" + fileName + "] ";

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

       delegate void ImportBlahsDelegate(string someItem);

       private void DoImportBlahs(object sender, RoutedEventArgs e)
       {
           isImportingBlahs = true;
           curBlah = 0;
           ImportProgress.Maximum = ((DataTable)BlahDataTable.DataContext).Rows.Count;
           App.Blahgua.StartLogFile();
           dispatcherTimer.Start();
       }

       private void DoImportPersonas(object sender, RoutedEventArgs e)
       {
           isImportingBlahs = false;
           curPerson = 0;
           ImportProgress.Maximum = ((DataTable)PersonaDataTable.DataContext).Rows.Count;
           App.Blahgua.StartLogFile();
           dispatcherTimer.Start();
       }

       private void dispatcherTimer_Tick(object sender, EventArgs e)
       {
           if (isImportingBlahs)
               HandleBlahTick();
           else
               HandlePersonaTick();

        }

       private void HandleBlahTick()
       {
           DataTable theTable = (DataTable)BlahDataTable.DataContext;
           dispatcherTimer.Stop();

           if (theTable != null)
           {

               string resultStr;

               DataRow curRow = theTable.Rows[curBlah];
               BlahImportItem curItem = new BlahImportItem(curRow);

               resultStr = curItem.ImportBlah();

               curBlah++;
               curRow["status"] = resultStr;
               ImportProgress.Value = curBlah;
               if (curBlah >= theTable.Rows.Count)
               {
                   dispatcherTimer.Stop();
                   App.Blahgua.StopLogFile();
               }
               else
                   dispatcherTimer.Start();

           }

       }

       private void HandlePersonaTick()
       {
           DataTable theTable = (DataTable)PersonaDataTable.DataContext;
           dispatcherTimer.Stop();

           if (theTable != null)
           {

               string resultStr;

               DataRow curRow = theTable.Rows[curPerson];
               ProfileImportItem curItem = new ProfileImportItem(curRow);

               resultStr = curItem.ImportProfile();


               curPerson++;
               curRow["status"] = resultStr;
               ImportProgress.Value = curPerson;
               if (curPerson >= theTable.Rows.Count)
               {
                   dispatcherTimer.Stop();
                   App.Blahgua.StopLogFile();
               }
               else
                   dispatcherTimer.Start();

           }

       }

       

        

        

        
    }
}
