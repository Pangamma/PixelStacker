:: Open up developer command prompt in VS via: Tools > Command Line > Developer Command prompt
:: Execute these commands.
:: /PixelStacker/Properties/PublishProfiles/publish.bat
:: 
:: 

dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=fd-x64-windows^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=fd-x86-windows^
 && del .\PixelStacker\bin\publish\release-recommended.zip^
 && cd .\PixelStacker\bin\publish\fd^
 && jar -cfM ..\release-recommended.zip .\^
 && cd ..\..\..\..\^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-x64-windows^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-x86-windows^
 && del .\PixelStacker\bin\publish\release-with-dotnet-runtime.zip^
 && cd .\PixelStacker\bin\publish\sc^
 && jar -cfM ..\release-with-dotnet-runtime.zip .\^
 && cd ..\..\..\..\^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=fd-gpu-x64-windows^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=fd-gpu-x86-windows^
 && del .\PixelStacker\bin\publish\release-with-gpu-feature.zip^
 && cd .\PixelStacker\bin\publish\fd-gpu^
 && jar -cfM ..\release-with-gpu-feature.zip .\^
 && cd ..\..\..\..\^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-gpu-x64-windows^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-gpu-x86-windows^
 && del .\PixelStacker\bin\publish\release-with-gpu-feature-and-dotnet-runtime.zip^
 && cd .\PixelStacker\bin\publish\sc-gpu^
 && jar -cfM ..\release-with-gpu-feature-and-dotnet-runtime.zip .\^
 && cd ..\..\..\..\^
