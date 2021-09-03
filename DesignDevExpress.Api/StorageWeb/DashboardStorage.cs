using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using DesignDevExpress.Api.Service;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;

namespace DesignDevExpress.Api.StorageWeb
{
    public class DashboardStorage : IDashboardStorage
    {
     

        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            return new List<DashboardInfo>()
            {
                new DashboardInfo()
                {
                    ID = "0001",
                    Name = "Teste"
                }
            };
        } 

        public XDocument LoadDashboard(string dashboardID)
        {
            //Buscar o dashboard por ID no banco de dados
            var dashboard = new Dashboard();
            var connectionParameters = new CustomStringConnectionParameters(ConnectionService.GetConnectionString());
            var dataSource = new DashboardSqlDataSource(connectionParameters);
            dataSource.Name = "Analytics DB";
          
            dataSource.Fill();
            
            dashboard.DataSources.Add(dataSource);

            return dashboard.SaveToXDocument();
        }

        public void SaveDashboard(string dashboardID, XDocument dashboard)
        {
            using (var stream = new MemoryStream())
            {
                dashboard.Save(stream);
                //salvar no banco de dados
            }
        }
    }
}