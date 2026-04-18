# Additional Data and Location Data



# Managing Custom Data
The application provides a default database of location and additional data. The default database provides a general framework so that users can add or modify it. The default database provides additional data and location data for a limited number of calling party numbers.

When the application runs, it checks for the presence of additional data for the current Windows user. If it does not find additional data for the current user then it copies its default database to a location specific to the current user so that each user can manage their own additional data an location data database.

A user’s database is located in the application data directory for the user. For example, it the user’s Windows user name is John, the additional data and location database will be:

> C:\Users\John\AppData\Local\OspSimulator\AdditionalData

The AdditionalData directory contains subdirectories. The name of each subdirectory is a 10-digit telephone number that identifies the simulated call’s calling party number. Each subdirectory contains one or more XML files that contain location or additional data.

