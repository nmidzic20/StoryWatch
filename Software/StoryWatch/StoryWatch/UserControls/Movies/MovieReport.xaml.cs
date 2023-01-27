using BoldReports.UI.Xaml;
using BusinessLayer;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StoryWatch.UserControls.Movies
{
    /// <summary>
    /// Interaction logic for MovieReport.xaml
    /// </summary>
    public partial class MovieReport : Window
    {
        public MovieReport()
        {
            InitializeComponent();

            reportViewer.Load += ReportViewer_Load;

        }
        private bool _isReportViewerLoaded;

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                ReportDataSource reportDataSource = new ReportDataSource();
                var movieServices = new MovieServices();
                var dataset = movieServices.GetAllMovies();

                reportViewer.LocalReport.DataSources.Clear();
                var DataSource = new ReportDataSource() { Name = "DataSet1", Value = dataset };
                reportViewer.LocalReport.DataSources.Add(DataSource);
                string Path = "UserControls/Movies/Reports/Report1.rdlc";
                reportViewer.LocalReport.ReportPath = Path;
                reportViewer.Refresh();
                reportViewer.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }
    }
    
}
