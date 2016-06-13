# Tutorial 3: Creating your own AppPackage
In this tutorial you will create an AppPackage (a C# dll) that implements an AutoCAD command which extracts layer names from the input drawing and saves them as a text file. You will also create an a custom Activity that uses this command.
## Prerequisites
+ Working C# 6.0 compiler. (This is part of Visual Studio 2015 or you can download it from [here] (https://www.microsoft.com/en-us/download/details.aspx?id=49982)). Mono C# compiler has _not_ been tested at this time.
+ Working Zip utility. (Download one from http://www.7-zip.org/download.html)

## Follow Step 1 through 3 in [Tutorial 1] (..\tutorial1\readme.md)
## Step 4: Create the C# dll
An AppPackage is an AutoCAD plugin written in any .net language (AutoLisp and C++ are also supported). The plugin interacts with the AutoCAD API to implement a custom command that your activity may call.
### Step 4.1: Write the C# code
Create a text file `command.cs` with the following content:

```c#
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices.Core;
using System;
using System.IO;
[assembly: CommandClass(typeof(CrxApp.Commands))]
[assembly: ExtensionApplication(null)]
namespace CrxApp
{
    public class Commands
    {
        [CommandMethod("MyTestCommands", "test", CommandFlags.Modal)]
        static public void Test()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            try
            {
                //extract layer names and save them to layers.txt
                var db = doc.Database;
                using (var writer = File.CreateText("layers.txt"))
                {
                    dynamic layers = db.LayerTableId;
                    foreach (dynamic layer in layers)
                        writer.WriteLine(layer.Name);
                }
            }
            catch (System.Exception e)
            {
                ed.WriteMessage("Error: {0}", e);
            }
        }
    }
}
```
### Step 4.2: Download nuget.exe
Nuget is a package manager for .net. We need it to download dependencies that our C# code requires. Download it with the following command:
```
curl https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
```
### Step 4.3: Download dependencies
Our C# code relies on the AutoCAD .NET API. You need to obtain these assemblies from https://nuget.org. Execute the following command to download them:
```
nuget install AutoCAD.NET.Core -Version 21.0.0 -ExcludeVersion
```
### Step 4.4: Build the C# code
Run the following command. It will generate command.dll.
```
csc command.cs /r:AutoCAD.NET.Model\lib\45\acdbmgd.dll /r:AutoCAD.NET.Core\lib\45\accoremgd.dll /t:library
```
## Step 5: Package the C# dll into an Autoloader zip
When uploading code to the service you must package it into a zip file with manifest. This zip file is called an autoloader package and it is the same format used by [Autodesk Exchange Apps] (https://apps.autodesk.com).
### Step 5.1: Create autoloader manifest
Create a text file named `PackageContents.xml` with the following content:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ApplicationPackage
    SchemaVersion="1.0"
    Version="1.0">
    <Components>
        <RuntimeRequirements 
            OS="Win64" 
            Platform="AutoCAD" />
        <ComponentEntry
            AppName="MyTestPackage"
            ModuleName="./Contents/Command.dll"
            AppDescription="AutoCAD.IO .net test app"
            LoadOnCommandInvocation="True"
            LoadOnAutoCADStartup="False">
            <Commands GroupName="MyTestCommands">
                <Command Global="TEST" Local="TEST" />
            </Commands>
        </ComponentEntry>
    </Components>
</ApplicationPackage>
```
### Step 5.2: Zip files
First lay out the files such that we can easily zip them and then zip the top level folder. Run the following commands:
```
xcopy PackageContents.xml MyTestPackage.bundle\
xcopy command.dll MyTestPackage.bundle\Contents\
zip -r package.zip MyTestPackage.bundle
```
You now have an autoloader package `package.zip` that you can upload to the service.
## Step 6: Create AppPackage resource
Creating the AppPackage resource is a 3 steps process. 

1. Get upload url
2. Upload autoloader package
3. Post AppPackage with upload url

### Step 6.1: Get upload url
```
curl https://developer.api.autodesk.com/autocad.io/us-east/v2/AppPackages/Operations.GetUploadUrl -H "Authorization: Bearer <your token>"
```
The _response_ will be:
```json
{
  "@odata.context":"https://developer.api.autodesk.com/autocad.io/us-east/v2/$metadata#Edm.String",
  "value":"<your upload url"
}
```

### Step 6.2: Upload autoloader package
```
curl <upload url> -X PUT -T package.zip
```
### Step 6.3: Post AppPackage
Create text file `app.json` with the following content:
```json
{
      "Resource": "<your upload url>",
      "RequiredEngineVersion": "21.0",
      "Id": "MyTestPackage",
      "Version": 1
}
```
```
curl https://developer.api.autodesk.com/autocad.io/us-east/v2/AppPackages -X POST -H "Content-Type: application/json" -H "Authorization: Bearer <your token>" -d @app.json
```
The response will be:
```json
{
  "@odata.context": "https://developer.api.autodesk.com/autocad.io/us-east/v2/$metadata#AppPackages/$entity",
  "References": [
  ],
  "Resource": "<your upload url>",
  "RequiredEngineVersion": "21.0",
  "IsPublic": false,
  "IsObjectEnabler": false,
  "Version": 1,
  "Timestamp": "<your time stamp>",
  "Description": "",
  "Id": "MyTestPackage"
}
```
You now have an AppPackage resource that you can reuse again and again in your Activities.
## Follow Step 4 in [Tutorial 2] (../tutorial2/readme.md)
There are 2 things that you must change though when you create activity.json
1. Set `"ActivityId": "ExtractLayers"`. 
2. Set `"Instruction": "_.test\n"`
**Note**: this works because the new activity `ExtractLayers` happens to have the exact same input and output parameters as the `CreateALine`.
## Follow Step 5 through 7 in [Tutorial 1] (../tutorial1/readme.md)
The only difference is that when you post the workitem you should set `"ActivityId": "ExtractLayers"`. **Note**: this works because the built in activity `PlotToPDF` happens to have the exact same input and output arguments as the `CreateALine` custom activity that you created above.

End of Tutorial
---
