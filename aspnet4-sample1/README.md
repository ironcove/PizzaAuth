# Auth0 + ASP.NET MVC4 Seed
This is the seed project you need to use if you're going to create an ASP.NET application.

# Running the example
In order to run the example you need to have Visual Studio 2013/2015 installed.

Install package Auth0.ASPNET via Package Manager or running the following command:

```Powershell
Install-Package Auth0-ASPNET
```

You also need to set the ClientSecret and ClientId of your Auth0 app in the `web.config` file. To do that just find the following lines and modify accordingly:
```CSharp
<add key="auth0:ClientId" value="BmxJB2opc1Fyeo62R_05f82612RhlfsH" />
<add key="auth0:ClientSecret" value="UbOtgBbGQqeEzIbrNZA4TVw-g-n3jSXHpqi4QJJf8NHAlG6x3A7mhS2D7vmugtfG" />
<add key="auth0:Domain" value="shanewhite.auth0.com" />
```

Don't forget to add `http://localhost:4987/LoginCallback.ashx` as **Allowed Callback URLs** and `http://localhost:4987/` as **Allowed Logout URLs** in your app settings.

After that just press **F5** to run the application. It will start running in port **4987**. Navigate to [http://localhost:4987/](http://localhost:4987/).

**Note:** You can change CallbackURL in `Views/Shared/_Layout.cshtml` on the line 53.