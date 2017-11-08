# QuickStart

## Clone Beatrice and create new setting JSON
- Clone this repository.
    - If using Raspberry Pi, please download and use a pre-built binary from [Releases](https://github.com/mayuki/Beatrice/releases) page.
- cd **Beatrice.Web**
- Copy **"appsettings.Beatrice.sample.json"** to **"appsettings.Beatrice.json"**.
- Copy **"actions.sample.json"** to **"actions.json"**.

## Create a "Actions on Google" project
- Navigate to the [Actions on Google](http://actions.google.com)
- [Click "Actions Console"](https://console.actions.google.com/)
- Click **"Add/import project"** to create new project.
- Input your project name and select country. (e.g. MyBeatriceApp and Japan)
- Note your project ID. You can see the project ID in `Settings` page or in console URL (e.g. https://console.actions.google.com/project/mybeatriceapp/ -> mybeatriceapp). 

## Create a API Key for sync request
- Navigate to the [Google Cloud Console API Manager](https://console.developers.google.com/apis)
- Click **"Credentials"**
- Select your project.
- Click **"Create Credentials"**
- Click **"API key"**
- Copy API Key and paste to **"SyncRequestApiKey"** value in **"appsettings.Beatrice.json"**

## Run ngrok (proxy server)
- Open a terminal and run `ngrok http 5000 --host-header=localhost`
- In the output, note the assigned subdomain name on ngrok. (e.g. https://example.ngrok.io/ -> example)

## Configure Beatrice (**appsettings.Beatrice.json**)
- Configure Sign-in account
    - Insert a username to **Beatrice:Security:User**.
    - Create a password. The value must be a SHA256 hash digest of "UserName:Password" format.
        - Linux/macOS: `echo -n UserName:Password | openssl sha256`
        - Windows: `((New-Object System.Security.Cryptography.SHA256Managed).ComputeHash([System.Text.Encoding]::UTF8.GetBytes("UserName:Password")) | %{ $_.ToString("x2") }) -join ""`
    - Insert the password to **Beatrice:Security:Password**.
- Configure OAuth Credentials
    - Create `ClientId` and insert to **Beatrice:Security:OAuth:ClientId**.
    - Create `ClientSecret` and insert to **Beatrice:Security:OAuth:ClientSecret**.
        - `ClientId` and `ClientSecret` are any unguessable random string of your choice.
    - In **Beatrice:Security:OAuth:RedirectUrls**, replace placeholder in `https://oauth-redirect.googleusercontent.com/r/<ProjectId>` for your project's.

## Configure Project (Actions on Google)
- Navigate to the Actions project.
- Click "Use Actions SDK"
- Update URLs in **"actions.json"** with where you hosted app location. 
    - `https://<AssignedSubDomain>.ngrok.io/Automation/` -> `https://example.ngrok.io/Automation/`
    - `https://server.home.example.com/Automation/`
    - etc...
- To update the project settings, execute `gactions` command with the **"actions.json"**
    - [Download gactions CLI](https://developers.google.com/actions/tools/gactions-cli)
    - Execute `gactions update --project <Project ID> --action_package actions.json` in a terminal.
- Click "OK"
- Click "ADD" under App information.
- Fill your app information. For example. App name, some description, contact info and more.
- Click "Account linking (optional)" and Click "ADD".
- Fill fields with your Beatrice OAuth configuration values.
    - Grant type: `Authorization code`
    - Client information
        - Client ID: same as **Beatrice:Security:OAuth:ClientId**
        - Client secret: same as **Beatrice:Security:OAuth:ClientSecret**
        - Authorization URL: `https://<Hosted address>/connect/authorize`
        - Token URL: `https://<Hosted address>/connect/token`
- Click "Save".
- Click "TEST DRAFT".

## Run Beatrice
- cd `Beatrice.Web`
- Run `dotnet run`
    - If using Raspberry Pi, Run `./Beatrice.Web` instead.
    - **Recommend**: At this time, You should check reachability to the app from the Internet. For example, you try to access the app via ngrok.io and sign in.

## Connect Beatrice (your project) to your Google Assistant account
- On a device with the Google Assistant logged into the same account used to create the project in Actions Console.
- Open Google Home or Assistant settings.
- Click "Home Control"
- Click the "+"
- Find your app project on the list.
- Sign in to the Beatrice.
- You can see the virtual devices on Beatrice in your smarthome device list.

## Ready for use Beatrice
Try saying `"Turn on Outlet1"` to Google Assistant(Home) or Simulator on Console. If there no problems, you can see a message "Hello! Konnichiwa!" in your terminal. 

## Optional: Configure Beatrice
- Configure web server
    - By default, the web server listens on port **5000**. If you want to listen on another port, you can override the port setting.
        - Set `http://*:1234` to `ASPNETCORE_URLS` environment variable.