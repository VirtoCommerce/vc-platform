# Genetaring a pdf-file

### How to generate a pdf-file
1. Need to download a tool wich can convert html to pdf
    * WkHtmlToPdf from [this](https://wkhtmltopdf.org/downloads.html)  
    * Need to chouse a package wich depends of your OS.
2. Then need to check your process setting, for example look at **_VirtoCommerce.OrdersModule.Tests.ProcessHelperIntegrationTests_**
    * Need to set WorkingDirectory
    * Need to set Arguments
3. the tool running:
    * like that:

    ```cmd
        "c:\Program Files\wkhtmltopdf\bin\wkhtmltopdf" --dpi 300 --page-size A4 --encoding "utf-8" --viewport-size "1920x1080" input.html output.pdf   
    ```

     * look at [manual](https://wkhtmltopdf.org/index.html) for details
3. Working with the Azure 
   * **_coming soon.._**    
