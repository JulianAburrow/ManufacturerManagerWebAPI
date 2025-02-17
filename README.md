# ManufacturerManagerWebAPI

A new take on an old theme. This time there is a WebAPI to manage all the Manufacturers, Widgets and associated admin items.
The front end is a Blazor WASM application with added MudBlazor for functionality and the back end is an ASP.Net Core WebAPI.

# Business Rules

# Colours and ColourJustifications

The WebAPI will not allow duplicate colours or colour justifications and the endpoints will return a 409 Conflict response if there
is an attempt to insert a duplicate or update an existing item to match another item in the database. The front end
handles these responses and displays an error message if appropriate.

Currently it only replaces spaces before checking for duplicate values but it would obviously be very simple also to check
for hyphens, apostrophes etc.

# Manufacturers and Widgets

The WepAPI will allow duplicate Manufacturers and Widgets but the front end should not. There is now and enpoint in the ManufacturerController which checks for duplicates in the database and return a Conflict reponse if any are found.

The front end Manufacturer create and update code now calls the appropriate endpoints and responds appropriately if duplicates are found.

# Setting Up

You will need to run the ManufacturerManagerWithMudBlazor_Db_Script.sql script against a SQL Server and then change the connection string in appsettings.json
to the appropriate server in order to run this application.
