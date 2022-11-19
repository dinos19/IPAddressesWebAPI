using Hangfire;
using IpaddressesWebAPI.HelperClasses;
using IpaddressesWebAPI.Models;
using IpaddressesWebAPI.Repositories;
using System.Net;
using System.Reflection.Metadata;

namespace IpaddressesWebAPI.Jobs
{
    public class DatabaseRefreshTaskManager
    {

        private readonly IPAddressesRepository iPAddressesRepository;
        private readonly RecurringJobManager recurringJobManager;
        private readonly DatabaseCronJob databaseCronJob;
        public DatabaseRefreshTaskManager(IPAddressesRepository iPAddressesRepository, RecurringJobManager recurringJobManager, DatabaseCronJob databaseCronJob)
        {
            this.iPAddressesRepository = iPAddressesRepository;
            this.recurringJobManager = recurringJobManager;
            this.databaseCronJob = databaseCronJob;

        }

        public async Task InsertTask()
        {
            recurringJobManager.AddOrUpdate(
    "run every 1 hour",
    () => UpdateDBJob(),
    "0 * * * *");
        }


        public async Task UpdateDBJob()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Console.WriteLine($"Job started at {DateTime.Now}");
            //get 100 of items updated time < mpw -1 min
            var iPAddresses = await iPAddressesRepository.GetIPAddresses();
            if (iPAddresses == null || iPAddresses.Count == 0) return;
           var batches =  DummyHelper.ChunkBy<IPAddresses>(iPAddresses, 100);
            foreach (var batch in batches)
            {
               await databaseCronJob.HandleBatch(batch);
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"@@ UpdateDBJob method took {elapsedMs}ms");
        }

        
    }
}
