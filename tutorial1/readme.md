# Tutorial 1: Convert DWG to PDF
In this tutorial you will convert a public sample dwg file to a pdf file to AWS S3. The steps were tested on Windows 10 and Ubuntu Linux but they should work on your operating system of choice (they only require a web browser and curl utility).
## Step 1: Create a Forge app
This step is common for any Forge workflows. Follow the steps [here] (https://developer.autodesk.com/en/docs/oauth/v2/tutorials/create-app/).
## Step 2: Make sure you have Curl
Open a terminal window and type
```
curl
```
If the command is recognized, then you are good to go. Otherwise, you must obtain curl. Visit https://curl.haxx.se/ to download.
## Step 3: Get a 2-legged Oauth token
This step is common for most Forge workflows. Follow the steps [here] (https://developer.autodesk.com/en/docs/oauth/v2/tutorials/get-2-legged-token/). IMPORTANT: you must specify `scope=code:all` instead of `scope=data:read`.
Step 4: Prepare output URL on AWS S3
Follow the [steps] (http://docs.aws.amazon.com/AmazonS3/latest/dev/PresignedUrlUploadObject.html) in the AWS documentation to create presigned url in your AWS account. The following screenshots show how to do this if you have [AWS Toolkit for Visual Studio 2015 installed] (https://aws.amazon.com/visualstudio/).

1. Right click on the bucket where you want the object to be stored.

![right click image] (right_click.png)

2. Fill out the dialog fields, generate and copy the URL.

![dialog image] (dialog.png)
