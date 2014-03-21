ECHO delete temp folders
rmdir /s /q "%TEMP%\WebPI"
rmdir /s /q "../../../SDK\Setup\VirtoCommerce.WebPI\bin"

ECHO create frontend temp
robocopy "%TEMP%\FrontEnd" "%TEMP%\WebPI\VirtoCommerce" /mir /NFL /NDL
robocopy "../../../SDK/Setup/VirtoCommerce.WebPI/Package" "%TEMP%\WebPI" /E /NFL /NDL
robocopy "..\..\..\src\Extensions\Setup\VirtoCommerce.PowerShell" "%TEMP%\WebPI\VirtoCommerce\App_Data\Virto\SampleData\Database" *.sql /mir /NFL /NDL
robocopy "..\..\..\src\Presentation\Admin\Presentation.Application\bin\debug\app.publish" "%TEMP%\WebPI\VirtoCommerce\App_Admin" /mir /NFL /NDL

"../../../SDK\Setup\VirtoCommerce.WebPI\Tools\7z.exe" a -r ../../../SDK\Setup\VirtoCommerce.WebPI\bin\VirtoCommerce.WebPI.zip %temp%/webpi/*.*