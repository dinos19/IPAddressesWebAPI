<h1 align="center">Web API in .net 6</h1>



## __Functionality__
#### 1) An endpoint which accepts an IP and uses a free service to find the location
#### 2) Save data on a MSSQLLocalDB (IPAddresses, Countries)
#### 3) A cron job that is validating that the saved data are not outdated
#### 4) An endpoint which accepts a list of two letter strings or nothing and it will report everything that suits the request from local db

## __Libraries used__
####   i) Hangfire for the cron job and dashboard to manage jobs
####  ii) Entity Framework for queries
#### iii) Microsoft.Extension.Caching.Memory to cache a list of IPs

## __Controllers__
#### a) Country controller to get all saved countries db
#### b) IPAddresses controller to get all saved IP addresses from db
#### c) Main controller which has the requested functionality
