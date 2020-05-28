# Pdf-file creation support

### Steps to add support for pdf-file generation
1. Download and install the recommended html to pdf conversion tool:
    * WkHtmlToPdf from [this](https://wkhtmltopdf.org/downloads.html)  
    * Choose right package, depending on your operating system.
2. Check process start settings, for example **_VirtoCommerce.OrdersModule.Tests.ProcessHelperIntegrationTests_**:
    * WorkingDirectory
    * Arguments
3. Starting of the conversion tool process:
    * like that:

    ```cmd
        "c:\Program Files\wkhtmltopdf\bin\wkhtmltopdf" --dpi 300 --page-size A4 --encoding "utf-8" --viewport-size "1920x1080" input.html output.pdf   
    ```

     * for details, look at [manual](https://wkhtmltopdf.org/index.html)
3. Working with the Azure 
   * **_coming soon.._**  
