# How to build a release
This guide covers the build and packaging process for deploying the desktop application.

## Prerequisites:
1. Visual Studio 2022 installed

## Building 
1. Open up developer command prompt in VS via: Tools > Command Line > Developer Command prompt
2. Type into console, `%cd%\PixelStacker\Properties\PublishProfiles\publish-solo-zipped.bat`
3. Your files will be zipped into `%cd%/PixelStacker/bin/publish`

## Code signing
If you want to add code signing to the process, that is totally doable. I used to have it working, but in order to continue doing so it requires me to pay money for a code signing certificate. To re-enable code signing, you must add the following lines to the bottom of the PixelStacker.csproj file. Then, make sure you have the code signing tool installed, and follow any other steps from certum or your code signing certificate provider of choice.
```
	<Target Name="SignTheExe" AfterTargets="Publish">
		<Exec WorkingDirectory="$(PublishDir)" Command="call &quot;$(VSAPPIDDIR)..\Tools\VsDevCmd.bat&quot;&#xD;&#xA;echo &quot;Signing this exe file after publishing. $(MSBuildProjectDirectory)\$(PublishDir)PixelStacker.exe&quot;&#xD;&#xA;signtool.exe sign /fd sha256 /t http://timestamp.comodoca.com/authenticode /n &quot;Open Source Developer, Taylor Love&quot; &quot;$(MSBuildProjectDirectory)\$(PublishDir)PixelStacker.exe&quot;" />
	</Target>
```