using IpaddressesWebAPI.Jobs;

namespace IpaddressesWebAPI.Managers
{
    public class MainManager
    {
        private readonly DatabaseRefreshTask databaseRefreshTask;
        public MainManager(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            databaseRefreshTask = scope.ServiceProvider.GetService<DatabaseRefreshTask>();
            databaseRefreshTask.InsertTask();
        }
    }
}
