:: Open up developer command prompt in VS via: Tools > Command Line > Developer Command prompt.
:: This file won't work on linux or Mac due to the file move commands not being OS agnostic.
:: Apologies in advance for that. Feel free to submit a PR with a mac or linux script.
:: 
:: Execute these commands.
:: /PixelStacker/Properties/PublishProfiles/publish-solo-zipped.bat
:: or
:: PixelStacker\Properties\PublishProfiles\publish-solo-zipped.bat
:: 
:: 

@ECHO OFF

dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-windows-x64^
 && del .\PixelStacker\bin\publish\windows-x64.zip^
 && cd .\PixelStacker\bin\publish\sc\windows-x64^
 && jar -cfM ..\..\windows-x64.zip .\^
 && cd ..\..\..\..\..\^
 && rmdir /s /q PixelStacker\bin\publish\sc

dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-windows-x64-gpu^
 && del .\PixelStacker\bin\publish\windows-x64-gpu.zip^
 && cd .\PixelStacker\bin\publish\sc\windows-x64-gpu^
 && jar -cfM ..\..\windows-x64-gpu.zip .\^
 && cd ..\..\..\..\..\^
 && rmdir /s /q PixelStacker\bin\publish\sc

dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-windows-x86-gpu^
 && del .\PixelStacker\bin\publish\windows-x86-gpu.zip^
 && cd .\PixelStacker\bin\publish\sc\windows-x86-gpu^
 && jar -cfM ..\..\windows-x86-gpu.zip .\^
 && cd ..\..\..\..\..\^
 && rmdir /s /q PixelStacker\bin\publish\sc

dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-windows-x86^
 && del .\PixelStacker\bin\publish\windows-x86.zip^
 && cd .\PixelStacker\bin\publish\sc\windows-x86^
 && jar -cfM ..\..\windows-x86.zip .\^
 && cd ..\..\..\..\..\^
 && rmdir /s /q PixelStacker\bin\publish\sc