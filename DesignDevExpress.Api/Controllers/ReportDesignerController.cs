using System.Collections.Generic;
using System.Threading.Tasks;
using DesignDevExpress.Api.Service;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.Web.ReportDesigner;
using Microsoft.AspNetCore.Mvc;

namespace DesignDevExpress.Api.Controllers
{
    [Route("api/[controller]")]
    public class DesignerController :  ReportDesignerController
    {
        private IReportDesignerClientSideModelGenerator reportDesignerClientSideModelGenerator;
        public DesignerController(
            IReportDesignerMvcControllerService controllerService,
            IReportDesignerClientSideModelGenerator reportDesignerClientSideModelGenerator
        ) : base(controllerService)
        {
            this.reportDesignerClientSideModelGenerator = reportDesignerClientSideModelGenerator;
        }
        [Route("")]
        [ResponseCache(Duration = 2147483647)]
        [HttpPost]
        [HttpGet]
        [IgnoreAntiforgeryToken]
        public override Task<IActionResult> Invoke()
        {
            var result = base.Invoke();
            return result;
        }
        [Route("GetReportDesignerModel")]
        [HttpPost]
        [HttpGet]
        [IgnoreAntiforgeryToken]
        public IActionResult GetReportDesignerModel(string reportUrl)
        {
            string modelJsonScript =
                reportDesignerClientSideModelGenerator
                    .GetJsonModelScript(
                        reportUrl,                 // The URL of a report that is opened in the Report Designer when the application starts.
                        GetAvailableDataSources(), // Available data sources in the Report Designer that can be added to reports.
                        "api/Designer",   // The URI path of the controller action that processes requests from the Report Designer.
                        DevExpress.AspNetCore.Reporting.WebDocumentViewer.WebDocumentViewerController.DefaultUri,
                        // The URI path of the default controller
                        // that processes requests from the Query Builder.
                        DevExpress.AspNetCore.Reporting.QueryBuilder.QueryBuilderController.DefaultUri
                    );
            return Content(modelJsonScript, "application/json");
        }

        Dictionary<string, object> GetAvailableDataSources()
        {
            var dataSources = new Dictionary<string, object>();
            
            var paramsConnection = new CustomStringConnectionParameters(ConnectionService.GetConnectionString());
            SqlDataSource ds = new SqlDataSource(paramsConnection);
           /*
            var query = SelectQueryFluentBuilder.AddTable("dbo.CATEGORIA").SelectAllColumns().Build("CATEGORIA");
            ds.Queries.Add(query);
            ds.RebuildResultSchema();
            */
            dataSources.Add("SqlDataSource", ds);
            
            var jsonDs = new JsonDataSource();
            var jsonString = System.IO.File.ReadAllText("data/Pessoas.json");
            jsonDs.JsonSource = new CustomJsonSource(jsonString);
           
            jsonDs.RootElement = "Pessoas";
            jsonDs.Fill();
            
            dataSources.Add("JsonDataSource", jsonDs);
            
            return dataSources;
        }

        
    }
    
}