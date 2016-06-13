# Tutorial 3: Creating your own AppPackage
In this tutorial you will create an AppPackage (a C# dll) that implements an AutoCAD command which extracts layer names from the input drawing and saves them as a text file. You will also create an a custom Activity that uses this command.
## Prerequisites
+ Working C# 6.0 compiler (this is part of Visual Studio 2015 or you can download it from [here] (https://www.microsoft.com/en-us/download/details.aspx?id=49982)
+ Zip utility

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
