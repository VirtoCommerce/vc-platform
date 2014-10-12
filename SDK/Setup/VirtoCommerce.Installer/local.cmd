ECHO delete temp folders
cmd.exe /c rmdir /s /q "%TEMP%\FrontEnd"

ECHO delete temp folders
cmd.exe /c rmdir /s /q "%TEMP%\Configuration"
cmd.exe /c rmdir /s /q "%TEMP%\Database"
cmd.exe /c rmdir /s /q "%TEMP%\Catalog"
cmd.exe /c rmdir /s /q "%TEMP%\Admin"

ECHO create frontend temp
cmd.exe /c robocopy "../../../src/Presentation/FrontEnd/StoreWebApp" "%TEMP%\FrontEnd" /s /NFL /NDL /purge /xd "obj" "Service References" "Storage" "App_Data" /xf *.user *.vspscc *.pdb *.xml packages.config
cmd.exe /c robocopy "../../../src/Presentation/FrontEnd/StoreWebApp/App_Data" "%TEMP%\FrontEnd/App_Data" /s /NFL /NDL /purge /xd "admin" "catalog" "search" "logs"
cmd.exe /c xcopy "..\..\..\src\GlobalAssemblyInfo.cs" "%TEMP%\FrontEnd\Properties"
cmd.exe /c robocopy "../../../src/Presentation/FrontEnd/StoreWebApp/App_Data/Virto/Storage/catalog" "%TEMP%\Catalog" /s /NFL /NDL /purge

ECHO create config temp
cmd.exe /c robocopy "../../Setup\VirtoCommerce.ConfigurationUtility.Application\bin\Debug" "%TEMP%\Configuration" /mir /NFL /NDL /xf *.pdb *.xml *.vshost.exe *.vshost.exe.config
cmd.exe /c robocopy "..\..\..\src\Extensions\Setup\VirtoCommerce.PowerShell" "%TEMP%\Database" *.sql /mir /NFL /NDL

ECHO create admin temp
cmd.exe /c robocopy "../../../src/Presentation/Admin/Presentation.Application/bin/Debug/app.publish " "%TEMP%\Admin" /mir /NFL /NDL /xf *.exe

if [%1]==[] goto End

ECHO ******************* STARTING CODE GENERATION

move "%TEMP%\Admin\Application Files\VirtoCommerce_1_0_0_13" "%TEMP%\Admin\Application Files\VirtoCommerce_$(var.Version)"

ECHO Heat frontend
"c:\Program Files (x86)\WiX Toolset v3.7\bin\heat.exe" dir "%TEMP%\FrontEnd" -gg -sfrag -template fragment -out Frontend.wxs -cg FrontEnd -var var.FrontendSourcePath -dr Resources -sreg -scom

ECHO Heat frontend catalog images
"c:\Program Files (x86)\WiX Toolset v3.7\bin\heat.exe" dir "%TEMP%\Catalog" -gg -sfrag -template fragment -out Catalog.wxs -cg Catalog -var var.FrontendCatalogPath -dr Resources -sreg -scom

ECHO Heat commerce manager
"c:\Program Files (x86)\WiX Toolset v3.7\bin\heat.exe" dir "%TEMP%\Admin" -cg Admin -gg -scom -sfrag -sreg -srd -dr Admin -var var.AdminPath -template fragment -out Admin.wxs
move "%TEMP%\Admin\Application Files\VirtoCommerce_$(var.Version)" "%TEMP%\Admin\Application Files\VirtoCommerce_1_0_0_13" 

ECHO Heat configuration
"c:\Program Files (x86)\WiX Toolset v3.7\bin\heat.exe" dir "%TEMP%\Configuration" -gg -sfrag -template fragment -out Configuration.wxs -cg Configuration -var var.ConfigSourcePath -dr INSTALLFOLDER_SDK -sreg -scom
"c:\Program Files (x86)\WiX Toolset v3.7\bin\heat.exe" dir "%TEMP%\Database" -gg -sfrag -template fragment -out Database.wxs -cg Database -var var.SqlScriptSourcePath -dr Resources -sreg -scom

REM e:\Utils\Checksum\fciv.exe -sha1 

:End