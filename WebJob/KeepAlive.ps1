$progressPreference = "silentlyContinue"

Invoke-WebRequest -UseBasicParsing -URI https://southernarchery.azurewebsites.net
Invoke-WebRequest -UseBasicParsing -URI https://southernarchery.azurewebsites.net/Tournaments
Invoke-WebRequest -UseBasicParsing -URI https://southernarchery.azurewebsites.net/Schedules
Invoke-WebRequest -UseBasicParsing -URI https://southernarchery.azurewebsites.net/Clubs
Invoke-WebRequest -UseBasicParsing -URI https://southernarchery.azurewebsites.net/Classes
Invoke-WebRequest -UseBasicParsing -URI https://southernarchery.azurewebsites.net/Account/Login