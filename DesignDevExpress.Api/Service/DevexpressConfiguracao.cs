using System.IO;
using System.Threading.Tasks;
using DevExpress.XtraReports.UI;

namespace DesignDevExpress.Api.Service
{
    public class DevexpressConfiguracao
    {
        public static async Task<XtraReport> GetXtraReport(string idArtefato)
        {
            var report = GetXtraReportFromStream(default);

            if (report == default)
            {
                report = new XtraReport();
            }
            
          //  AddReportServices(report);
            
            report.Tag = "TesteReport";
            
            return report;
        }

        private static XtraReport GetXtraReportFromStream(byte[] metadata = default)
        {
            XtraReport report = default;

            if (metadata != default)
            {
                using (var stream = new MemoryStream(metadata))
                    report = XtraReport.FromStream(stream);
            }

            return report;
        }
    }
}


