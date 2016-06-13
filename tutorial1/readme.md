# Tutorial 1: Convert DWG to PDF
In this tutorial you will convert a public sample dwg file to a pdf file on AWS S3. The conversion is a predefined [activity] (https://developer.autodesk.com/en/docs/design-automation/v2/overview/field-guide/) of the service so we simply submit a workitem. The steps were tested on Windows 10 and Ubuntu Linux but they should work on your operating system of choice (they only require a web browser and curl utility).
## Step 1: Create a Forge app
This step is common for any Forge workflows. Follow the steps [here] (https://developer.autodesk.com/en/docs/oauth/v2/tutorials/create-app/).
## Step 2: Make sure you have Curl
Open a terminal window and type
```
curl
```
If the command is recognized, then you are good to go. Otherwise, you must obtain curl. Visit https://curl.haxx.se/ to download.
## Step 3: Get a 2-legged Oauth token
This step is common for most Forge workflows. Follow the steps [here] (https://developer.autodesk.com/en/docs/oauth/v2/tutorials/get-2-legged-token/). **IMPORTANT**: you must specify `scope=code:all` instead of `scope=data:read`.
Step 4: Prepare output URL on AWS S3
Follow the [steps] (http://docs.aws.amazon.com/AmazonS3/latest/dev/PresignedUrlUploadObject.html) in the AWS documentation to create presigned url in your AWS account. The following screenshots show how to do this if you have [AWS Toolkit for Visual Studio 2015 installed] (https://aws.amazon.com/visualstudio/).

1. Right click on the bucket where you want the object to be stored.

![right click image] (right_click.png)

2. Fill out the dialog fields, generate and copy the URL.

![dialog image] (dialog.png)

## Step 5: Create a workitem
The WorkItem is where the source and destination are specified for the input and output files, respectively, and it references the desired Activity to be executed.
We will use the [POST WorkItems] (https://developer.autodesk.com/en/docs/design-automation/v2/reference/http/WorkItems-POST) endpoint to create the WorkItem. 
### Step 5.1 Create request body
Create a text file called `workitem.json` in the current folder for your terminal window with the following content:
```json
{
  "Arguments": {
    "InputArguments": [
      {
        "Resource": "http://download.autodesk.com/us/samplefiles/acad/visualization_-_aerial.dwg",
        "Name": "HostDwg"
      }
    ],
    "OutputArguments": [
      {
        "Name": "Result",
        "HttpVerb": "PUT",
        "Resource": "<your url from Step 4>"
      }
    ]
  },
  "ActivityId": "PlotToPDF"
}
```
### Step 5.2 Issue the HTTP request 
Execute the following command in your terminal window. 
```
curl https://developer.api.autodesk.com/autocad.io/us-east/v2/WorkItems -X POST -H "Content-Type: application/json" -H "Authorization: Bearer lY7xiNQfkuFs4t639HOf4bZRcOua" -d @workitem.json
```
### Step 5.3. Retrieve the workitem id
The above step results in the following response. Copy the Id field.
```
{
  "@odata.context": "https://developer.api.autodesk.com/autocad.io/us-east/v2/$metadata#WorkItems/$entity",
  "ActivityId": "PlotToPDF",
  "Arguments": {
    "InputArguments": [
      {
        "Resource": "http://download.autodesk.com/us/samplefiles/acad/blocks_and_tables_-_imperial.dwg",
        "Name": "HostDwg",
        "Headers": [

        ],
        "ResourceKind": null,
        "StorageProvider": null,
        "HttpVerb": null
      }
    ],
    "OutputArguments": [
      {
        "Resource": "<your url from Step 4>",
        "Name": "Result",
        "Headers": [

        ],
        "ResourceKind": null,
        "StorageProvider": null,
        "HttpVerb": "PUT"
      }
    ]
  },
  "Status": "Pending",
  "StatusDetails": {
    "Report": null
  },
  "AvailabilityZone": null,
  "TimeQueued": "<your queue time>",
  "TimeInputTransferStarted": null,
  "TimeScriptStarted": null,
  "TimeScriptEnded": null,
  "TimeOutputTransferEnded": null,
  "BytesTranferredIn": null,
  "BytesTranferredOut": null,
  "Timestamp": "<your time stamp>",
  "Id": "<your workitem id>"
}
```
## Step 6: Query the status of the WorkItem
In this scenario we will query the status of your workitem. **This is NOT recommended in production scenarios**. You would normally provide a callback URL to the service to notify you when the workitem concludes. Subsequent tutorial will demonstrate how to do this.
Execute the following command to query the status of your workitem. 
```
curl https://developer.api.autodesk.com/autocad.io/us-east/v2/WorkItems('<your workitem id>')/Status -H "Authorization: Bearer <your token>"
```
The response from the above request is the following. 
```
{
  "@odata.context": "https://developer.api.autodesk.com/autocad.io/us-east/v2/$metadata#WorkItems('<your workitem id>')/Status",
  "value": "Succeeded"
}
```
At this point you can go to your storage account and see the generated PDF file there.
## Step 7: Get the workitem report
Query the workitem object to obtain the “StatusDetails”
```
curl https://developer.api.autodesk.com/autocad.io/us-east/v2/WorkItems('<your workitem id>') -H "Authorization: Bearer <your token>"
```
The response you get will have the same structure as in Step 5.3. Find the StatusDetails object in the response and locate its Report attribute. It should be a longish URL by now (if status is no longer Pending). Then issue a GET requests on the URL:
```
curl <your report url>
```
 You will get something like this. You can use this report to understand the cause of any failures.
 ```
[06/12/2016 19:31:54] Starting work item f3a7d6c29d0b4a468e9c7dbb429f7471
[06/12/2016 19:31:54] Start download phase.
[06/12/2016 19:31:54] Start downloading file http://download.autodesk.com/us/samplefiles/acad/blocks_and_tables_-_imperial.dwg.
[06/12/2016 19:31:54] End downloading file http://download.autodesk.com/us/samplefiles/acad/blocks_and_tables_-_imperial.dwg. 227008 bytes have been written to C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\blocks_and_tables_-_imperial.dwg.
[06/12/2016 19:31:54] End download phase.
[06/12/2016 19:31:54] Start preparing script and command line parameters.
[06/12/2016 19:31:54] Start script content.
[06/12/2016 19:31:54] _tilemode 0 -export _pdf _all result.pdf

[06/12/2016 19:31:54] End script content.
[06/12/2016 19:31:54] Command line: /i "C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\blocks_and_tables_-_imperial.dwg" -suppressGraphics /s "C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\script.scr"
[06/12/2016 19:31:54] End preparing script and command line parameters.
[06/12/2016 19:31:54] Start script phase.
[06/12/2016 19:31:54] ### Command line arguments: /isolate job_f3a7d6c29d0b4a468e9c7dbb429f7471 "C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\userdata" /exe "Z:\Aces\AcesRoot\20.1\coreEngine\Exe\accoreconsole.exe"  /i "C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\blocks_and_tables_-_imperial.dwg" -suppressGraphics /s "C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\script.scr".
[06/12/2016 19:31:54] HostApp = Z:\Aces\AcesRoot\20.1\coreEngine\Exe\sandboxer.exe.
[06/12/2016 19:31:54] Start AutoCAD Core application output.
[06/12/2016 19:31:54] Redirect stdout (file: C:\Windows\TEMP\accc271223).
[06/12/2016 19:31:54] Isolating to userId=job_f3a7d6c29d0b4a468e9c7dbb429f7471, userDataFolder=C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\userdata.
[06/12/2016 19:31:55] Launching sandbox process: [Z:\Aces\AcesRoot\20.1\coreEngine\Exe\accoreconsole.exe /i C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\blocks_and_tables_-_imperial.dwg -suppressGraphics /s C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\script.scr  /isolate job_f3a7d6c29d0b4a468e9c7dbb429f7471 C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\userdata]
[06/12/2016 19:31:55] Setting TMP to [C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\userdata\temp]
[06/12/2016 19:31:55] Redirect stdout (file: C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\userdata\temp\accc35322).
[06/12/2016 19:31:55] AutoCAD Core Engine Console - Copyright 2015 Autodesk, Inc.  All rights reserved. (M.49.Z.1)
[06/12/2016 19:31:55] Running at low integrity.
[06/12/2016 19:31:55] Regenerating layout.
[06/12/2016 19:31:55] Regenerating model.
[06/12/2016 19:31:56] Command:
[06/12/2016 19:31:56] Command:
[06/12/2016 19:31:56] Command:
[06/12/2016 19:31:56] Command: _tilemode
[06/12/2016 19:31:56] Enter new value for TILEMODE <0>: 0
[06/12/2016 19:31:57] Command: -export Enter file format [Dwf/dwfX/Pdf] <Pdf>_pdf Enter plot area [Current layout/All layouts]<Current Layout>: _all
[06/12/2016 19:31:57] Enter file name <blocks_and_tables_-_imperial-D-size Plot.pdf>: result.pdf
[06/12/2016 19:31:57] Effective plotting area:  35.54 wide by 23.15 high
[06/12/2016 19:31:58] Effective plotting area:  12.03 wide by 16.49 high
[06/12/2016 19:31:58] Plotting viewport 2.
[06/12/2016 19:31:58] Effective plotting area:  8.69 wide by 9.93 high
[06/12/2016 19:31:58] Plotting viewport 3.
[06/12/2016 19:31:58] Plotting viewport 1.
[06/12/2016 19:31:58] Command: _quit
[06/12/2016 19:31:58] End AutoCAD Core Console output
[06/12/2016 19:31:58] End script phase.
[06/12/2016 19:31:58] Start upload phase.
[06/12/2016 19:31:58] Uploading C:\Aces\Jobs\f3a7d6c29d0b4a468e9c7dbb429f7471\result.pdf to https://albert-sandbox.s3-us-west-2.amazonaws.com/test?AWSAccessKeyId=AKIAIKV6JNTMZRDRQO2A&Expires=1465770540&Signature=3TbnDDRZ4UhRQakIQgNVnwnulPw%3D.
[06/12/2016 19:31:59] End upload phase.
[06/12/2016 19:31:59] Job finished with result Succeeded
```
This is the last step of this tutorial.
