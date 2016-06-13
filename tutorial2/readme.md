# Tutorial 2: Create a line
## Follow Step 1 through 3 in [Tutorial 1] (..\tutorial1\readme.md)
## Step 4: Create new activity
Creating a new activity is like defining a function in most progrramming languages: you must specify input and output parameters and the instructions that the function carries out. We will do something extremely simple: draw a line from 0,0, to 10,10 and save the drawing.
**Note:** Once you have created the activity the service stores it for you and you can submit any number of workitems that use this activity.

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
### Step 4.2 Issue the HTTP request 
Execute the following command in your terminal window. 
```
curl https://developer.api.autodesk.com/autocad.io/us-east/v2/Activities -X POST -H "Content-Type: application/json" -H "Authorization: Bearer lY7xiNQfkuFs4t639HOf4bZRcOua" -d @activity.json
```
## Follow Step 5 through 7 in [Tutorial 1] (../tutorial1/readme.md)
The only difference is that when you post the workitem you should set `"ActivityId": "CreateALine"`

End of Tutorial
---

