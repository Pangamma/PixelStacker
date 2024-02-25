# How to deploy the web API to the linux server
This guide covers the deployment process for deploying the API project onto a linux server. 
[Try out the API](https://taylorlove.info/projects/pixelstacker/swagger/index.html)

## Prerequisites:
1. Ubuntu OS installed
2. .NET 6.0 or higher installed on the Ubuntu OS
4. Visual Studio 22 or higher on your local machine.
5. Add [these lines](https://github.com/Pangamma/PixelStacker/blob/master/src/PixelStacker.Web.Net/Properties/DevOps/nginx.txt) to your nginx config. (assuming you're using nginx for your hosting) 
```
location ^~ /projects/pixelstacker/demo {
	# Set the document root to a public folder.
	# This should point to where the actual UI code is hosted.
	root /var/www/vhosts/taylorlove.info/httpdocs/projects/pixelstacker.web.react;

	# Redirect requests without the trailing slash (at /demo) to HAVE the trailing slash.
	rewrite ^/projects/pixelstacker/demo$  /projects/pixelstacker/demo/ redirect;

	# /demo should give the index.html file for the react app.
	try_files $uri $uri/index.html index.html;

	# Redirect all files after /demo/ to the correct folder directory.
	rewrite "(?i)/projects/pixelstacker/demo/(.*)" /$1  break;
}
```
6. `yarn build` the react project for the UI. Hopefully you know how to do this. I am mostly writing this instruction set for future me right now as I doubt many people would go through the effort of setting up a self hosted UI. You can always ask me questions if needed though.

## Deploying the Web UI
1. Open PixelStacker.Web.sln in Visual Studio
2. In the solution explorer, right click the ```PixelStacker.Web.Net``` project, and select ```publish``` option.
3. Select ```SendToWebHost.pubxml``` from the dropdown, then click ```publish```. (This builds the project and stores it to a local publish directory)
4. Open TestExplorer for the PixelStacker.CodeGeneration project.
5. Right click the code generation project in solution explorer, and then ```manage user secrets```.
6. Ensure these secrets have values that work for your use case:
```
{
  "FTP_HOST": "ftp.yourhost.info",
  "FTP_USERNAME": "ftp-username",
  "FTP_PASSWORD": "ftp-password",
  "SSH_USERNAME": "ssh-user",
  "SSH_PASSWORD": "ssh-password"
}
```
7. Run the DeployUiToWebServer test method.
8. You are done! Try it out in your own website now.