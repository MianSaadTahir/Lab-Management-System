////using CrystalDecisions.CrystalReports.Engine;
//using System;
//using System.IO;
//using System.Windows.Forms;

//namespace ProjectB
//{
//    public partial class @for : Form
//    {
//        public @for()
//        {
//            InitializeComponent();
//        }

//        private void crystalReportViewer2_Load(object sender, EventArgs e)
//        {
//            ReportDocument r = new ReportDocument();
//            string path = Application.StartupPath;
//            string reportpath = @"C:\Users\tahir\Downloads\ProjectB\ProjectB\.rpt";
//            string fpath = Path.Combine(path, reportpath);

//            r.Load(fpath);
//            crystalReportViewer1.ReportSource = r;
//        }
//    }
//}
