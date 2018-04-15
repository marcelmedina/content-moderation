# Content Moderation
A sample created for the Auckland Azure Bootcamp.

## Setting things up
#### Create an access key
- To easily create an access key, login to the content moderation [Review Tool](https://contentmoderator.cognitive.microsoft.com/) using your Microsoft Account
- Click on the settings icon and click on "Credentials".
- Copy your Ocp-Apim-Subscription-Key into a text file for use in later parts of this demo
#### Content Moderator API project
- Clone the api project onto your local folder from Github using the following command:
```
git clone https://github.com/marcelmedina/ContentModeration.git
```
- Open the solution in visual Studio (*...or you could use the dotnet cli to run builds, it is built in .net core 2.0*)
- Edit the appsettings.json file to add your Ocp-Apim-Subscription-Key that was generated from the review tool earlier.
- Build the solution.
- Create a new Web App on Azure and deploy the API project to that Web App (*I find the quickest way to do this is to zip up the files in /debug/bin folder of the ContentModeration project and then drag and drop the zip file into https://[**YourWebAppName**].scm.azurewebsites.net/ZipDeploy*)
- You may need to wait a few minutes before the server can restart and your API is now ready.
#### Storage accounts
- Using the Azure Portal, deploy a storage account and create 2 blob containers. One for unmoderated content and one for moderated content.
- In the settings section, ensure public access is granted for content.
#### Logic App
- To setup the logic app, login to the Azure Portal and create a new logic app, and a logic app connector.
- Copy the contents of the json file /LogicApp/contentmoderation.json and paste it into the code view of the logic app that was created from the Azure Portal.
- This may prompt requests to login and you may need to update the values for storage account URLs
- Save the logic app.