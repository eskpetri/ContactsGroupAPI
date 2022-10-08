# ContactGroupAPI

a Simple application API for FrontEnd to use. Just testing technologies like Entityframework Database first approuch. Joining and delivering data from controller. JWT is aim. 

## ER model

<img src="er_model.png">

## Database connection

For development (local machine) I created environment variable called "DATABASE_URL" in Program.cs. And in Database.cs I use that variable. So in production environment (example Heroku), I set enviroment variable which is configured for the Heroku MySQL.

## JOIN Queries
I used technique to join data in controller end and just simple queries to database. Works well in small and medium data masses. Would not do this in large scale user environment. Then I would use sql to reduse the data amount moving between DB and BackEnd and eventually to FrontEnd.


##List of command used to create this project. Just a reminder.
-dotnet new gitignore
-Scaffold-dBContext "server=127.0.0.1;user id=netuser;password=netpass;port=3306;database=contactgroup;" Pomelo.EntityFrameworkCore.MySql -OutputDir Models
-dotnet add package BCrypt.Net-Next
