# <a name="AdditionalData">Location and Additional Data</a>
The application provides a default database of location and additional data. The default database provides a general framework so that users can add or modify it. The default database provides additional data and location data for a limited number of calling party numbers.

When the application runs, it checks for the presence of additional data for the current Windows user. If it does not find additional data for the current user then it copies its default database to a location specific to the current user so that each user can manage their own additional data an location data database.

The following table presents a summary of the location and additional data available in the default database for each calling party number (From).

| From | Location | Comment | Device Info | Provider Info | Service Info | Subscriber Info | AACN |
|------|----------|---------|-------------|---------------|--------------|-----------------|------|
| 6306810001 | Yes | No  | No  | No  | Yes | Yes | No  |
| 6306820002 | Yes | No  | No  | No  | Yes | Yes | No  |
| 6306820003 | Yes | No  | No  | No  | Yes | Yes | No  |
| 6306820004 | Yes | No  | No  | No  | Yes | Yes | No  |
| 6306820005 | Yes | Yes | Yes | Yes, multiple  | Yes | Yes | Yes |

AACN = Automated Advanced Crash Notification

The following table describes the location data in the default database for each calling party number (From).

| From | Civic Location | Geographic Location |
|------|----------------|---------------------|
| 6306820001 | 2106 E NEW YORK ST, AURORA, IL | None |
| 6306820002 | 16010 PENNY LN, HOMER GLEN, IL 16010 | None |
| 6306820003 | 2190 E NEW YORK ST, AURORA, IL | None |
| 6306820004 | 1102 CORNELL LN, YORKVILLE, IL | Point: Lat = 41.65728, Lon = -88.4603 |
| 6306820005 | None | Circle: Lat = 37.09024, Lon = -95.712891, Radius = 50 meters |

# <a name="ManageCustomData">Managing Custom Data</a>
Each Windows user’s database is located in the application data directory for the user. For example, it the user’s Windows user name is John, the additional data and location database will be:

> C:\Users\John\AppData\Local\OspSimulator\AdditionalData

The AdditionalData directory contains subdirectories. The name of each subdirectory is a 10-digit telephone number that identifies the simulated call’s calling party number. Each subdirectory contains one or more XML files that contain location or additional data.

The user can modify the files under each directory and add location and additional data for new calling party numbers.

Users can add additional data and location data for new calling party numbers by following these steps.
1. Create a new directory under the AdditionalDataDirectory. The new directory name should be a new 10-digit number.
1. Add additional data and location data files to the new directory.

**Note:** The application reads the additional data and location for all calling party numbers when the user clicks on the Start Server button in the main application window. If any changes are made to the additional data or location data after the application's server has been started then the server must be stopped and the started again.

This application uses the following file naming convention for location and additional data.

| Data Type | File Name | Description |
|-----------|-----------|-------------|
| Location  | PidfLo.xml | Location data in PIDF-LO XML format. There are many ways that location can be represented. [See Working with PIDF-LO Data](https://phrsite.github.io/Ng911Lib/articles/WorkingwithPIDF-LOData.html). |
| Comments  | Comment*.xml | Provides human readable comments about some aspect about the call such as information about the subscriber. See Section 4.5 of RFC 7852. There may be multiple comments files as described below.|
| Device Information | DeviceInfo.xml | Provides information about the device that made the call. See Section 4.3 of RFC 7852. |
| Provider Information | ProviderInfo*.xml | Provides information about the provider of one or more of the other additional data or location blocks. See Section 4.1 or RFC 7852. There may be multiple provider information files as described below. |
| Service Information  | ServiceInfo.xml | This file describes the service that the service provider provides to the caller. See Section 4.2 of RFC 7852. |
| Subscriber Information | SubscriberInfo.xml | This file provides information about the subscriber of the telephone service that placed the call. See Section 4.4 of RFC 7852. |
| Automated Crash Notification | AutomatedCrashNotification.xml | Provides information from an automated crash notification system. See RFC 8148. |

All or none of the above location and additional data may be available for a simulated calling party.

There can only be one file for each of the following data types.
1. Location
1. Device Information
1. Service Information
1. Subscriber Information
1. Automated Crash Notification

The file names for each of the additional data/location data must exactly match the file names shown in the above table.

It is possible to have multiple files for Comments and Provider Information. The asterisk shown in File Name column in the above table represents a multi-character wild card.

For example, there could be comments relating to the caller's location and comments relating to the subscriber. In this case, the first comment file could be named "Comments1.xml and the second comments file could be called "Comments2.xml". The same rule applies to provider information.






