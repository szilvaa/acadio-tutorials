# Tutorial 6: Execute an input script
In this tutorial you will create a custom activity that downloads a script from an input URL and executes it.
## Follow Step 1 through 3 in [Tutorial 1](../tutorial1/readme.md)
## Step 4: Create new activity
Creating a new activity is like defining a function in most progrramming languages: you must specify input and output parameters and the instructions that the function carries out. Our activity will have 2 input arguments and 1 output argument. 
1. The `HostDwg` input argument is the drawing that will be loaded when we run the `Instruction`. 
2. The `MyScript` input argument contains the custom script that we want to run. 
3. The `Instruction` is runs the script in `MyScript` and then exports the drawing to PDF.
4. The `MyResult` output argument is the URL where the resulting PDF will be uploaded.

**Note:** 
+ Once you have created the activity the service stores it for you and you can submit any number of workitems that use this activity.
+ This activity deals with 2 scripts which might be confusing at first. The activiy *contains* a script which uses the SCRIPT command to run the 2nd (`MyScript` input argument) script.

### Step 4.1 Create request body
Create a text file called `activity.json` in the current folder for your terminal window with the following content:
```json
{
  "Id": "ExecuteAScript",
  "Instruction": {
    "Script": "_.SCRIPTCALL my.scr\n_layoutcreateviewport 1\n_tilemode 0\n-export _pdf _all result.pdf\n"
  },
  "Parameters": {
    "InputParameters": [
      {
        "LocalFileName": "$(HostDwg)",
        "Name": "HostDwg"
      },
      {
        "LocalFileName": "my.scr",
        "Name": "MyScript"
      }
    ],
    "OutputParameters": [
      {
        "LocalFileName": "result.pdf",
        "Name": "MyResult"
      }
    ]
  },
  "RequiredEngineVersion": "23.0"
}
```
### Step 4.2: Post Activity resource
Execute the following command in your terminal window. 
```
curl https://developer.api.autodesk.com/autocad.io/us-east/v2/Activities -X POST -H "Content-Type: application/json" -H "Authorization: Bearer <your token>" -d @activity.json
```
## Step 5: Follow Step 4 in [Tutorial 1](../tutorial1/readme.md)
You need to create 2 URLs (to satisfy `MyScript` and `MyResult` parameters of the activity). [Tutorial 1](../tutorial1/readme.md) shows how to create the output URL. Creating the input URL is almost the same with 2 minor differences:
1. You actually need to have an input file in S3. It can be any AutoCAD script. If you don't have any then use this (it will draw a circle):
```
._CIRCLE 0,0 1

```
2. You should select the GET radio button in the dialog if you use Visual Studio to create the pre-signed URL.

## Step 6: Create a workitem
The WorkItem is where the source and destination are specified for the input and output files, respectively, and it references the desired Activity to be executed.
We will use the [POST WorkItems](https://developer.autodesk.com/en/docs/design-automation/v2/reference/http/WorkItems-POST) endpoint to create the WorkItem. 

### Step 6.1 Create request body
Create a text file called `workitem.json` in the current folder for your terminal window with the following content:
```json
{
  "Arguments": {
    "InputArguments": [
      {
        "Resource": "http://download.autodesk.com/us/samplefiles/acad/blocks_and_tables_-_imperial.dwg",
        "Name": "HostDwg"
      },
      {
        "Resource": "<your s3 presigned input URL>",
        "Name": "MyScript"
      }
    ],
    "OutputArguments": [
      {
        "Name": "MyResult",
        "HttpVerb": "PUT",
        "Resource": "<your s3 presigned output URL>"
      }
    ]
  },
  "ActivityId": "ExecuteAScript"
}
```
### Remaining Steps: Follow Step 5.2 onward in [Tutorial 1](../tutorial1/readme.md)
End of Tutorial
---

