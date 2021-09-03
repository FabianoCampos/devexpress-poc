using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesignDevExpress.Api.Service;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Hosting;

namespace DesignDevExpress.Api.StorageWeb
{
    public class CustomReportStorageWebExtension: DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        readonly string ReportDirectory;
        const string FileExtension = ".repx";
        public CustomReportStorageWebExtension (IWebHostEnvironment env)
        {
            ReportDirectory = Path.Combine(env.ContentRootPath, "Reports");
            if (!Directory.Exists(ReportDirectory))
            {
                Directory.CreateDirectory(ReportDirectory);
            }
        }

        public override bool CanSetData(string url) { return true; }
        public override bool IsValidUrl(string url) { return true; }
        public override byte[] GetData(string url)
        {
            try
            {
                var task = DevexpressConfiguracao.GetXtraReport(url);
                task?.Wait();
                var report = task?.Result;
                using (var stream = new MemoryStream())
                {
                    report?.SaveLayoutToXml(stream);
                    return stream.ToArray();
                }
            }
            catch (Exception)
            {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException(
                    string.Format("Could not find report '{0}'.", url));
            }
        }

        public override Dictionary<string, string> GetUrls()
        {
            return Directory.GetFiles(ReportDirectory, "*" + FileExtension)
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .ToDictionary<string, string>(x => x);
        }

        public override void SetData(XtraReport report, string url)
        {
            report.SaveLayoutToXml(Path.Combine(ReportDirectory, url + FileExtension));
        }

        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            SetData(report, defaultUrl);
            return defaultUrl;
        }
    }
}