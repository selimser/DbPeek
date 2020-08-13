# DbPeek
DbPeek is a Microsoft Visual Studio extension that lets you peak into stored procedures (for now) of a database with ease. Project started because of need to frequently view stored procedures. It was (and being) developed because there was a need for it!

## Documentation
You can find the documentation on the repository's [Wiki section](https://github.com/selimser/DbPeek/wiki).


## Contribution
If you want to improve DbPeek, add some functionality to it and want everyone to benefit from your awesome work, then awesome you! Just fork your repository and PR your changes for review. It might be a good idea to raise an issue before really start digging into it. 

If you don't feel like sharing with anyone? That's alright too! Just clone the repository and play around as you like.

# How to Install the Extension
You have a few options. If you want to use the "official" release of it, simply navigate to the Extension Manager in MS Visual Studio and search for "DbPeek". Simply install and enjoy.

Alternatively, if you fancy building your own version (adding / removing features etc.), simply clone the repository, build the soltuion in Release mode. Find the DbPeek.vsix file in your bin/Release folder and install that way.

# How to use?
That's the good part. Once the extension is enabled and configured, simply highlight a text in your code, right click and select `View Stored Procedure` context menu item. This will automatically fetch the contents of the database object and display you in a new window within Visual Studio. It's simple as that.

# FAQ
* Is this safe?

   Short answer is, yes. It is open source, and you see what the code does. However, storing sensitive information like connection strings etc. through this extension is your responsibility.

* How are the connection strings stored?

   The connection strings, and every other setting managed are stored in the Visual Studio's internal settings storage called `SettingStore`. It is accessed via a `WritableSettingStore` and `ShellSettingManager`, and the settings are scoped to User Settings, so will not be visible to other users. However, storing sensitive information is at your own risk. Treat this method as any other storage method you may choose.

* Can I use this on a production environment?

   Technically, yes - assuming that you have access to it. However, if you are asking if you should or not, you should be the judge of it. Essentially, the extension performs basic operations on a SQL Server database, like opening a connection and calling sp_helptext command. Recommendation is to limit the usage to development environments.


* What is being cached?

   As you use the extension, you will realise that extension files are created in your temporary folder. These are the contents of the  SQL Server object you extracted. Each file is given a random GUID as filename and stored under `DbPeek_Dump` folder under temporary files folder. As these temporary files are saved in `*.sql` format, you will have the freedom to quickly edit and execute the script (e.g. altering a stored procedure / scalar function etc.) if your environment is configured for that.

   Even though the cache files are likely to be tiny (proportional to the contents of the db object of course), you can easily get rid of them with a single button click. Simply go to your `Extensions > Db Peek > Configure` and click on `Clear Cache` button. You can also monitor how many cache files you have and how much space they are occupying on your drive from the same window.


# Planned Features

There are a few features planned and sitting in the backlog currently. Some of these features includes:

* Multiple database connection support
* Project-based project load
* Read connection string from project configuration files (e.g. `Web.Config` or `appsettings.json`) automatically
* Detect SQL Server version automatically from connection string and optimise executing queries for the version
Quick load of a connection string from user settings store
* Match the configuration window theme to user's active Visual Studio theme (e.g. Light Blue, Dark etc.)

If you have any ideas / suggestions on improving the tool, please raise an issue on this repository and we all can discuss the possibilities together.

Happy coding!