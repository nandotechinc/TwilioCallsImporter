# Twilio Calls Importer (.NET Core Console App)

Simple console application that allows one to download and process all calls in a given date range from Twilio.

Console will prompt for Start & End Date, sit back and watch it work.

Open your `appsettings.json` file and change configuration values to ones matching your account.

The only real variables to change for _live_ use are `DefaultConnection` for your database (although you can use the local db just fine) and then your `Credentials` should be your `AccountID:APIkey`
