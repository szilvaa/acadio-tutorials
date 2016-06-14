#Tutorial 4: Using Status callback instead of polling
This tutorial how to use the `Status` output argument in your workitem to obtain the status of your workitem asynchronously. We will direct the `Status` output argument to a storage account in this tutorial but you can (and normally would) specify a REST API endpiont for this.

#Follow the steps in [Tutorial 1] (../tutorial1/readme.md)
The following is where you have to diverge from the original steps:
+ Create 2 presigned urls in Step 4. IMPORTANT: Make sure that your 2nd presigned URL is generated with Content-Type: application/json; charset=utf-8 as pictured below.

![Presigned URL] (dialog.png)
+ Use the following workitem.json in Step 5.1.

```JSON
{
  "Arguments": {
    "InputArguments": [
      {
        "Resource": "http://download.autodesk.com/us/samplefiles/acad/blocks_and_tables_-_imperial.dwg",
        "Name": "HostDwg"
      }
    ],
    "OutputArguments": [
      {
        "Name": "Result",
        "HttpVerb": "PUT",
        "Resource": '<your_first_presigned_url_for_upload>'
      },
      {
        "Name": "Status",
        "HttpVerb": "PUT",
        "Resource": '<your_second_presigned_url_for_upload>'
      }

    ]
  },
  "ActivityId": "PlotToPDF"
}
```
Note the `Status` output argument. The service will execute an HTTP PUT operation with a json request body that contains the status of the workitem. Note that you can also use POST if desired.
