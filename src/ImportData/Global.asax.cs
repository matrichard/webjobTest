using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace ImportData
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            EnsureQueueStorage();
        }

        private bool EnsureQueueStorage()
        {
            return QueueStorage.Initialize();
        }
    }

    public static class QueueStorage
    {
        private static CloudQueue Queue { get; set; }

        public static bool Initialize()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
            // Create the queue client
            var queueClient = storageAccount.CreateCloudQueueClient();
            // Retrieve a reference to a queue
            Queue = queueClient.GetQueueReference("dataimportqueue");

            // Create the queue if it doesn't already exist
            Queue.CreateIfNotExists();

            return true;
        }

        public static void AddMessage<T>(T message)
        {
            //var blobInfo = new {Tenant = "sfmm", Files = "members.csv"};
            var queueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(message));
            Queue.AddMessage(queueMessage);
        }
    }
}
