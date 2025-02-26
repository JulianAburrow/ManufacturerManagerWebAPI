# ManufacturerManagerWebAPI

A new take on an old theme. This time there is a WebAPI to manage all the Manufacturers, Widgets and associated admin items.
The front end is a Blazor WASM application with added MudBlazor for functionality and the back end is an ASP.Net Core WebAPI.

# Business Rules

The business roles for the WebAPI and the Blazor WASM are deliberately different to demonstrate two ways of approaching essentially the same issue.

# -  Colours and ColourJustifications

The WebAPI will not allow duplicate colours or colour justifications and the endpoints will return a 409 Conflict response if there
is an attempt to insert a duplicate or update an existing item to match another item in the database. The front end
handles these responses and displays an error message if appropriate.

Currently it only replaces spaces before checking for duplicate values but it would obviously be very simple also to check
for hyphens, apostrophes etc.

# -  Manufacturers and Widgets

The WepAPI will allow duplicate Manufacturers and Widgets but the business rules for the particular UI consuming the api do not. There are now endpoints in the ManufacturerController and the WidgetController to checks for duplicates in the database and return a Conflict reponse if any are found.

The front end create and update code now calls the relevant endpoint and responds appropriately if duplicates are found. It is not possible to update an item to have the same name as an existing one.

# Setting Up

You will need to run the ManufacturerManagerWithMudBlazor_Db_Script.sql script against a SQL Server and then change the connection string in appsettings.json
to the appropriate server in order to run this application.

# NB

Some tests are not yet working correctly. They pass when run in isolation but fail when run as part of a batch. Still trying to figure out why...
