using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApiV2.Application
{
    public class MyAppSettings
    {
        public string OnEnv { get; set; } = "";
        public string ApplicationName { get; set; } = "";
        public string ApplicationVersion { get; set; } = "";
        public string ApplicationVersionName { get; set; } = "";
        public string ApplicationEnvironment { get; set; } = "";
        public string AuthenticationIdentitySigningKey { get; set; } = "";
        public string AuthenticationIdentityIssuer { get; set; } = "";
        public string AuthenticationIdentityAudience { get; set; } = "";
        public string ClientUrl { get; set; } = "";
        public string DatabaseType { get; set; } = "";
        public string DatabaseServer { get; set; } = "";
        public string DatabaseInstance { get; set; } = "";
        public string DatabaseName { get; set; } = "";
        public string DatabaseNameLog { get; set; } = "";
        public string DatabaseUserName { get; set; } = "";
        public string ProtectedDatabasePassword { get; set; } = "";
        public bool EnableSqlConnectionEncryption { get; set; }
        public bool TrustSQLServerCertificate { get; set; }
        public string Server { get; set; } = "";
        public string BusinessCentralURL { get; set; } = "";
        public string BusinessCentralHostName { get; set; } = "";
        public string BusinessCentralInstanceName { get; set; } = "";
        public string BusinessCentralPort { get; set; } = "";
        public bool BusinessCentralHttps { get; set; }
        public string BusinessCentralServiceAccountName { get; set; } = "";
        public string BusinessCentralServiceAccountPassword { get; set; } = "";
        public string BusinessCentralServiceCompanyID { get; set; } = "";
        public string BusinessCentralActiveDirectoryDomain { get; set; } = "";
        public MyAppSettings_BusinessCentralServiceEndpoints BusinessCentralServiceEndpoints { get; set; } = new MyAppSettings_BusinessCentralServiceEndpoints();

        public string LocalHostConnectionString
        {
            get
            {
                return $"Server=CNNB922;Database=2024-11-01 14.32 DPA_DB_V2_UAT [DPA_UAT];User id=;Integrated Security=True;TrustServerCertificate=True;Encrypt=True;";
                //return $"Server={this.DatabaseServer};Database={this.DatabaseName};User id=;Integrated Security=True;TrustServerCertificate=True;Encrypt=True;";
            }
        }
    }

    public class MyAppSettings_BusinessCentralServiceEndpoints
    {
        public string Companies { get; set; } = "";
        public string GetBudgetnames { get; set; } = "";
        public string GetBudgetname { get; set; } = "";
        public string PatchBudgetname { get; set; } = "";
        public string PostBudgetname { get; set; } = "";
        public string AddBudget { get; set; } = "";
        public string AddBudgetWithProject { get; set; } = "";
        public string AddBudgetSpecials { get; set; } = "";
        public string GetbudgetRemaining { get; set; } = "";
        public string GetAccounts { get; set; } = "";
        public string GetExampleOData { get; set; } = "";
        public string CheckPurchaseInvoiceStatus { get; set; } = "";
        public string GLEntryTransaction { get; set; } = "";
        public string PostPurchaseRequest { get; set; } = "";
        public string PostedPurchaseInvoiceLines { get; set; } = "";
        public string GetVendors { get; set; } = "";
    }
}
