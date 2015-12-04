using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace DataImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            JobHost host = new JobHost();
            host.RunAndBlock();
        }
    }

    public static class Functions
    {
        public static void ProcessQueueMessage([QueueTrigger("dataimportqueue")] dynamic message, TextWriter logger)
        {
            Console.WriteLine("{0} from {1} received", message.FileName, message.Tenant);
        }
    }
}
