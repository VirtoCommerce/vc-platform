How to build a module package:
1. Open Tools > NuGet Package Manager > Package Manager Console
2. In the "Default project" list select your module project (the one with module.manifest file)
3. Execute the following command:
Compress-Module
The module package will be saved in the project directory.

Syntax:
Compress-Module [-ProjectName <String>] [-OutputDir <String>]
