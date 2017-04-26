#Tutorial 5: Storing output with Forge Data Management API
This tutorial shows how to use the Forge Data Managemeent API to store outputs (fetching inputs can also be done in similar fashion). It demonstrates that input/output argument can also specify arbitrary HTTP headers in addition to the URL.
## Follow Step 1 through 3 in [Tutorial 1](../tutorial1/readme.md)
## Step 4 Get a 2-legged Oauth token to create bucket on [OSS](https://developer.autodesk.com/en/docs/data/v2/overview/basics/)
Follow the steps [here](https://developer.autodesk.com/en/docs/oauth/v2/tutorials/get-2-legged-token/). **IMPORTANT**: you must specify `scope=bucket:create`. We will use this token in Step 5 to create the bucket where outputs will be placed.
## Step 5 Create a bucket on OSS
### Step 5.1 Create request body
Create a text file called bucket.json in the current folder for your terminal window with the following content. Make sure that you replace <your_bucket_name> with your preferred name for the bucket. __NOTE:__ bucket names must be globally unique. You will receive `Bucket already exists` error if your chosen name is already taken. 
```json
{
  "bucketKey":"<your bucket name>",
  "policyKey":"transient"
}
```
### Step 5.2 Post the bucket resource
Execute the following command in your terminal window.
```
curl -v https://developer.api.autodesk.com/oss/v2/buckets -X POST -H "Content-Type: application/json" -H "Authorization: Bearer <your token from step 4>" -d @bucket.json
```
## Step 6 Get a 2-legged Oauth token to access [OSS](https://developer.autodesk.com/en/docs/data/v2/overview/basics/)
Follow the steps [here](https://developer.autodesk.com/en/docs/oauth/v2/tutorials/get-2-legged-token/). **IMPORTANT**: you must specify `scope=data:write data:create data:read`. We will use this token as an `Authorization` header value for the `Result` output argument in the next step. We could have created one token in Step 4 that includes `bucket:create` __and__ `scope=data:write data:create data:read` but it is good security practice to always use minimum privilege.
## Step 7 Create workitem that directs its output to OSS
### Step 7.1 Create request body
Create a text file called workitem.json in the current folder for your terminal window with the following content. Make sure that you replace both <your bucket name from step 5.1> and <your_token_from_step6>.
```json
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
        "Resource": "https://developer.api.autodesk.com/oss/v2/buckets/<your bucket name from step 5.1>/objects/test.pdf",
        "Headers" : [ {
          "Name" : "Authorization",
          "Value" : "Bearer <your token from step6>"
        }]
      }
    ]
  },
  "ActivityId": "PlotToPDF"
}
```
### Post the WorkItem resource
Execute the following command in your terminal window.
```
curl https://developer.api.autodesk.com/autocad.io/us-east/v2/WorkItems -X POST -H "Content-Type: application/json" -H "Authorization: Bearer <your token from step 3>" -d @workitem.json
```
