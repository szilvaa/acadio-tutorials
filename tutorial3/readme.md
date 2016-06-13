# Tutorial 3: Creating your own AppPackage
In this tutorial you will create an AppPackage (a C# dll) that implements an AutoCAD command which extracts layer and block names from the input drawing and saves them as a json file. You will also create an a custom Activity that uses this command.
## Follow Step 1 through 3 in [Tutorial 1] (..\tutorial1\readme.md)
## Step 4: Create the C# dll
An AppPackage is an AutoCAD plugin written in any .net language (AutoLisp and C++ are also supported). The plugin interacts with the AutoCAD API to implement a custom command that your activity may call.
Step 4.1: Write the C# code
Create a text file `command.cs` with the following content:

```c#

```
