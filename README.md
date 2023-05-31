# RefactoringChallenge

Make the Enviorenment ready.
Install dotnet core 3.1 sdk

Clone the repository.

Use either CMD or visual studio to run the application.

#### When using CMD

Open a command propmt in folder where the .sln is in and run 'dotnet restore'.

Go to the RefactoringChallenge.Api forlder and run 'dotnet run'.

Use 'https://localhost:5001/swagger/index.html' test the end points.

#### When using visual studio 

Double click the .sln will open the solution in Visual Studio.

Set the startup project by right clicking on the RefactoringChallenge.Api project.

Click the play icon at the top of visual studio to run the application.

Use 'https://localhost:5001/swagger/index.html' test the end points.

## Improvements done.

Please note I have used inmemory db due to resource limitations in my laptop.

Seperated the business logic, models and Entities to their own projects.
Which makes the code more clean and responsibility wise seperated.

Added a global exception handler so no need to repeat try catch blocks.

Used mapster to map instead of manual mapping.

Removed redundant Validation Attributes and used in fluent api validations(improved consistancy).

Added model validations to return early in case of a bad request.

Removed unnesassary indexes since they slow the create and update operations.

Used models to pass parameters instead of passing multiple parameters which make it more extensible when change comes.

Change the HTTP attributes to PATCH and DELETE where appropriate.

Read configurations from config file instead of hardcoding.

Read only the required data from the db instead of reading all the data.

Made the api call async to make it more scalable.

Added unit tests.

## Further improvements that can be done.

Add logging.

Add caching.

Database normalization.

Use 2 objects to do the mapping with mapster in order create and add order details.

Use repository pattern(Have not used repository patteren since there is only one service).

Dockerizing(Did not do due to resource limitations in my laptop)

## Changes can be done in production

Read secrets from secure offering such as keyvault.

Use api versioning.

Change the log level to information.

