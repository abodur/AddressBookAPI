# AddressBook API

This project runs in .NET Core environment. You can import this as a project in Visual Studio by cloning from the repo.

## For Development Environment

Before running the project, DB connection parameters must be added as an environment variable. Since these parameters include sensitive information, secrets.json file should be used.

Name of the variable must be <code>Connection:AdressBook</code> whereas the structure of the value must be as follows:

	server=<server>;port=<port>;user=<user>;password=<password>;database=<database_name>

After running the project in Visual Studio, a Swagger page will be opened and you will be able to test the endpoints.

## For Production Environment

Project is not currently suitable for production environment