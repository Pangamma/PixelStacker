# How to deploy the web API to the linux server
This guide covers the deployment process for deploying the API project onto a linux server. 
[Try out the API](https://taylorlove.info/projects/pixelstacker/swagger/index.html)

## Prerequisites:
1. Ubuntu OS installed
2. .NET 6.0 or higher installed on the Ubuntu OS
4. Visual Studio 22 or higher on your local machine.
5. Upload the service file ["pixelstacker.web.net.service"](https://github.com/Pangamma/PixelStacker/blob/master/src/PixelStacker.Web.Net/Properties/DevOps/pixelstacker.web.net.service) to your linux server's ```/etc/systemd/system``` directory.
6. Add [these lines](https://github.com/Pangamma/PixelStacker/blob/master/src/PixelStacker.Web.Net/Properties/DevOps/nginx.txt) to your nginx config. (assuming you're using nginx for your hosting) 
```
location /projects/pixelstacker/ {
	proxy_pass http://localhost:5005/;
	proxy_http_version  1.1;
	proxy_set_header Upgrade $http_upgrade;
	proxy_set_header Connection keep-alive;
	proxy_set_header Host $host;
	proxy_cache_bypass  $http_upgrade;
	proxy_set_header X-Forwarded-Host $host;
	proxy_set_header X-Real-IP $remote_addr; # remote IP addr again?
	proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for; # remote IP addr
	proxy_set_header X-Forwarded-Proto $scheme;
	proxy_redirect off;
}
```
5. Enable the web service to begin on system startup: ```sudo systemctl enable pixelstacker.web.net``` (Might need to deploy the app first)

## Deploying the API
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
  "SSH_PASSWORD": "ssh-passwordd"
}
```
7. Run the DeployApiToWebServer test method.
8. You are done! Try it out in your own website now.