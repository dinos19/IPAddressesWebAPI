# IpaddressesWebAPI
Web API using .net 6

Functionality :
1)An endpoint which accept an IP and uses a free service to find the location.
2)Save data on a MSSQLLocalDB (IPAddresses, Countries).
3)A cron job that is is validating that the saved data are not outdated.
4)An endpoint which accept a list of TwOLetter string or nothing and it ll report everything that suits the request from local db.

Libraries used :
*) Hangfire for the cron job and dashboard to manage jobs
*) Entity Framework for queries
*) Microsoft.Extension.Caching.Memory to cache a list of ips

*controllers
a) Country controller to get all saved countries db
b) IPAddresses controller to get all IPaddresses from db
c) Main controller which has the requested functionality.