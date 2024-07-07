:: Open up developer command prompt in VS via: Tools > Command Line > Developer Command prompt.
:: This file won't work on linux or Mac due to the file move commands not being OS agnostic.
:: Apologies in advance for that. Feel free to submit a PR with a mac or linux script.
:: 
:: Execute these commands.
:: /PixelStacker/Properties/PublishProfiles/publish-solo.bat
:: or
:: PixelStacker\Properties\PublishProfiles\publish-solo.bat
:: 
:: 



rmdir /s /q PixelStacker\bin\publish^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-x64-windows^
 && move PixelStacker\bin\publish\sc\win-x64\PixelStacker.exe PixelStacker\bin\publish\pixelstacker-x64.exe^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-x86-windows^
 && move PixelStacker\bin\publish\sc\win-x86\PixelStacker.exe PixelStacker\bin\publish\pixelstacker-x86.exe^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-gpu-x64-windows^
 && move PixelStacker\bin\publish\sc-gpu\win-x64-gpu\PixelStacker.exe PixelStacker\bin\publish\pixelstacker-x64-gpu.exe^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-gpu-x86-windows^
 && move PixelStacker\bin\publish\sc-gpu\win-x86-gpu\PixelStacker.exe PixelStacker\bin\publish\pixelstacker-x86-gpu.exe^
 && rmdir /s /q PixelStacker\bin\publish\sc^
 && rmdir /s /q PixelStacker\bin\publish\sc-gpu