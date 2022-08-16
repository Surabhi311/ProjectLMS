Library Management System is a Online Library Web Application made using ASP.NET MVC and Frontend built using HTML CSS It is a simple, user-friendly and easy to navigate.

FEATURES
For Admin :

Can Approve or Reject the request for any book that is requested by User.
Can perform Create,Read,Update,Delete operations on Account, Book, Author, Publisher tables.
Can View all the past Records/Requests of each user.
For User:

Can views all the books and Request it.
Can view all the issued books and Return it.
Can view all his past Records/Requests.

### Softwares
* Microsoft Visual Studio Community 2022 (64-bit) Version **17.3.0**
* Microsoft SQL Server Management Studio Version **18.2.1**
* .NET Framework Version **5.0.17**

### Packages
>Inside your Visual Studio Navigate to Tools > NuGet Package Manager > Manage NuGet Packages for Solution and install these packages

* Microsoft.EntityFrameworkCore Version **5.0.17**
* Microsoft.EntityFrameworkCore.Design Version **5.0.17**
* Microsoft.EntityFrameworkCore.Tools Version **5.0.17**
* Microsoft.EntityFrameworkCore.SqlServer Version **5.0.17**

### Commands
> Inside your Visual Stdio Navigate to 
**Tools** > **NuGet Package Manager** > **Package Manager Console**
and enter these commands
```sh
Scaffold-DbContext "Server=localhost;Database=<DB-NAME>;Trusted_Connection=True;"-OutputDir Models (To scaffold All Database tables in your MVC Application)
```
```sh
Add provider Name
Microsoft.EntityFrameworkCore.SqlServer
```
Make sure to update appsettings.json  inside MVC App with the **Database Name** you want to give.
```sh
"DBConnection": "Server=localhost;Database=<DB-NAME>;Trusted_Connection=True"
```
