# ContactGroupAPI
a Simple application API for FrontEnd to use. Just testing technologies like Entityframework, Database first approuch, JWT, Httponly cookie. Joining and delivering data from controller. 

## ER model

<img src="er_model.png">

## Environmental variables
For development (local machine) I created environment variable called "DATABASE_URL" for MySql connection string and "SecredKey" for JWT encryption part and "SacredBKey" for Bcrypt password encryption in Program.cs. 

## JOIN Queries
I used technique to join data in controller end and just simple queries to database. Works well in small and medium data masses. Would not do this in large scale user environment. Then I would use sql to reduse the data amount moving between DB and BackEnd and eventually to FrontEnd.

## Install notes
Use Visual Studio and [Ctrl] + . or NuGet package manager to add missing pacgakes listed below<br/>
Microsoft.AspNet.Cors<br/>
Microsoft.AspNetCore.Authentication.JwtBearer<br/>
System.IdentityModel.Tokens.Jwt<br/>
Microsoft.EntityFrameworkCore.Design<br/>
Microsoft.EntityFrameworkCore.Tools<br/>
Pomelo.EntityFrameworkCore.MySql<br/>
MySqlConnector<br/>
BCrypt.Net-Next<br/>
Swashbuckle.AspNetCore<br/>
Swashbuckle.AspNetCore.Filters<br/><br/>

Goto project directory with terminal and type dotnet run.<br/>

## List of command used to create this project. Just a reminder.
-dotnet new gitignore <br/>
-Scaffold-dBContext "server=127.0.0.1;user id=netuser;password=netpass;port=3306;database=contactgroup;" Pomelo.EntityFrameworkCore.MySql -OutputDir Models<br/>
-dotnet add package BCrypt.Net-Next<br/>
