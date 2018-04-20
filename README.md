# Content Moderation

### Summary ###
A sample created for the Auckland Azure Bootcamp.

### Solution ###
Author(s) | Contact
-----------|--------
Marcel Medina | [sharepoint4developers.net](http://sharepoint4developers.net)
Nawaz Gayoom | [@ngayoom](https://twitter.com/ngayoom)

### Version history ###
Version  | Date | Comments
---------| -----| --------
1.0  | April 20th 2018 | Initial release

### Setting things up ###
#### Create an access key ####
- To easily create an access key, login to the content moderation [Review Tool](https://contentmoderator.cognitive.microsoft.com/) using your Microsoft Account
- Click on the settings icon and click on "Credentials".
- Copy your Ocp-Apim-Subscription-Key into a text file for use in later parts of this demo
#### Content Moderator API project ####
- Clone the api project onto your local folder from Github using the following command:
```
git clone https://github.com/marcelmedina/ContentModeration.git
```
- Open the solution in visual Studio (*...or you could use the dotnet cli to run builds, it is built in .net core 2.0*)
- Edit the appsettings.json file to add your Ocp-Apim-Subscription-Key that was generated from the review tool earlier.
- Build the solution.
- Create a new Web App on Azure and deploy the API project to that Web App (*I find the quickest way to do this is to zip up the files in /debug/bin folder of the ContentModeration project and then drag and drop the zip file into https://[**YourWebAppName**].scm.azurewebsites.net/ZipDeploy*)
- You may need to wait a few minutes before the server can restart and your API is now ready.
#### Storage accounts ####
- Using the Azure Portal, deploy a storage account and create 2 blob containers. One for **unmoderated** content and one for **moderated** content.
- In the settings section, ensure public access is granted for content.
#### Logic App Connector ####
- To setup the logic app connector, login to the Azure Portal and create a new logic app connector.
- Export the swagger.json definition from the swagger endpoint:
https://YourWebAppName.azurewebsites.net/**swagger**
- When configuring the Custom Connector, a wizard will guide you through the setup process. Follow through these steps.
1. On General tab:
- How do you want to create your connector?
	* API endpoint = **REST**
	* Upload an OpenAPI file = **swagger.json** (that was exported)
- General Information
	* **Host** - set to your azure web app URL 
	YourWebAppName.azurewebsites.net
2. On Security tab:
- Leave the Authentication type as is = **No authentication**
3. On Definition tab:
- Leave as it is, or update the description of endpoints if you prefer. 
*(The Values endpoint can be removed as they are not used).*

After the changes make sure you click **Update the connector**.
#### Logic App ####
- To setup the logic app, login to the Azure Portal and create a new logic app.
- Copy the contents of the json file /LogicApp/contentmoderation.json and paste it into the *Code view* of the logic app that was created from the Azure Portal.
	* Remove the json section **$connections** completely
	* Save the Logic App
- Switch to the *Design view*. Some components will be greyed out, this is because the connections are not configured.
- Add the following extra actions to the workflow:
1. Azure Blob Storage action
	* Select the trigger *Azure Blob Storage - when a blob is added or modified (properties only)*
	* Provide the connection name = **azureblob**
	* Select the Storage Account created and click *OK*
	* Then select the **unmoderated** container.

*NOTE: At this stage jump to the Code view and update the references of previous connections to the new azureblob. Basically you need to search the json for old connection names like azureblob_1 and replace with azureblob.*
*Remove any old connection references related to the blob storage.*

2. Logic App Connector (the one just created)
* Select the custom connector **content-moderator-connector**
* The connection name will be mapped automatically on Code view as the name of the custom connector.

*Note: The same approach should be adopted here to update the references of previous connections. Search for contentmodeapi and replace it with the new connection*
*Remove any old connection references as well.*

3. Office 365 Outlook - Send an email
* Select the action *Office 365 Outlook - Send an email*
	* Then sign in with your service account (it can be any account)

*NOTE: At this stage jump to the Code view and update the references of previous connections to the new azureblob. Basically you need to search the json for old connection names like office365_1 and replace with office365.*
*Remove any old connection references related.*

- Right now all the connections are established and all the additional actions added should be removed.
- **Save** the logic app.