# Tutorial 2: Create a line
In this tutorial you will create a custom activity that adds a line to the input drawing.
## Follow Step 1 through 3 in [Tutorial 1] (../tutorial1/readme.md)
## Step 4: Create new activity
Creating a new activity is like defining a function in most progrramming languages: you must specify input and output parameters and the instructions that the function carries out. We will do something extremely simple: draw a line from 0,0, to 10,10 and save the drawing.

**Note:** 
+ Once you have created the activity the service stores it for you and you can submit any number of workitems that use this activity.

### Step 4.1 Create request body
Create a text file called `activity.json` in the current folder for your terminal window with the following content:
```json
{
  "Id": "CreateALine",
  "Instruction": {
    "Script": "_.line\n0,0\n10,10\n\n_.saveas\n\nresult.dwg\n"
  },
  "Parameters": {
    "InputParameters": [
      {
        "LocalFileName": "$(HostDwg)",
        "Name": "HostDwg"
      }
    ],
    "OutputParameters": [
      {
        "LocalFileName": "result.dwg",
        "Name": "Result"
      }
    ]
  },
  "RequiredEngineVersion": "20.1"
}
```
### Step 4.2: Post Activity resource
Execute the following command in your terminal window. 
```
curl https://developer.api.autodesk.com/autocad.io/us-east/v2/Activities -X POST -H "Content-Type: application/json" -H "Authorization: Bearer lY7xiNQfkuFs4t639HOf4bZRcOua" -d @activity.json
```
The _response_ body will be this:

```json
{
  "@odata.context": "https://developer.api.autodesk.com/autocad.io/us-east/v2/$metadata#Activities/$entity",
  "AppPackages": [
  ],
  "HostApplication": "",
  "RequiredEngineVersion": "20.1",
  "Parameters": {
    "InputParameters": [
      {
        "Name": "HostDwg",
        "LocalFileName": "$(HostDwg)",
        "Optional": null
      }
    ],
    "OutputParameters": [
      {
        "Name": "Result",
        "LocalFileName": "result.dwg",
        "Optional": null
      }
    ]
  },
  "Instruction": {
    "CommandLineParameters": null,
    "Script": "_.line\n0,0\n10,10\n\n_.saveas\n\nresult.dwg\n"
  },
  "AllowedChildProcesses": [
  ],
  "IsPublic": false,
  "Version": 2,
  "Timestamp": "<your time stamp>",
  "Description": "",
  "Id": "CreateALine"
}
```
## Follow Step 5 through 7 in [Tutorial 1] (../tutorial1/readme.md)
The only difference is that when you post the workitem you should set `"ActivityId": "CreateALine"`. **Note**: this works because the built in activity `PlotToPDF` happens to have the exact same input and output arguments as the `CreateALine` custom activity that you created above.

End of Tutorial
---

