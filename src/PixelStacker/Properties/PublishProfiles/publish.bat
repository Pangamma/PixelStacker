:: Open up developer command prompt in VS via: Tools > Command Line > Developer Command prompt
:: Execute these commands.
:: 
:: && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=fd-x64-windows-gpu^
:: && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=fd-x86-windows-gpu^

dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=fd-x64-windows^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=fd-x86-windows^
 && del .\PixelStacker\bin\publish\framework-dependent-releases.zip^
 && cd .\PixelStacker\bin\publish\fd^
 && jar -cfM ..\framework-dependent-releases.zip .\^
 && cd ..\..\..\..\^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-x64-windows^
 && dotnet publish .\PixelStacker\PixelStacker.csproj /p:PublishProfile=sc-x86-windows^
 && del .\PixelStacker\bin\publish\self-contained-releases.zip^
 && cd .\PixelStacker\bin\publish\sc^
 && jar -cfM ..\self-contained-releases.zip .\^
 && cd ..\..\..\..\
