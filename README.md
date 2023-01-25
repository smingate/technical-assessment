# Swisscom SminGate Coding Challenge

## Background
A public JSON REST-API is available that returns a list of target assets from our postman mock server on the endpoint "/targetAsset". For the purposes of this task, this API will be referred to as the "Platform-API".

The objective of the task is to create a new JSON API, referred to as the "Demo-API", which will provide its own endpoint "/targetAsset". You can use the existing project 'Demo-API' for this purpose.

## Task
Create a new endpoint "/targetAsset" in the Demo-API. This should call the TargetAsset-API (GET on /targetAsset), then add the fields "isPatchable" and "parentTargetAssetCount" to the received target assets, and return the enriched target assets as JSON to the Demo-API caller.

- The "isPatchable" field should be  *true*  if, according to the current date (local system time of the Demo-API), it is the third day of the month and the "status" field of the target asset is "Running". Otherwise, "isPatchable" should be false.

- The "parentTargetAssetCount" field should contain the number of parent target assets that can be determined by the "parentId" field of each target asset. For example, if a target asset named "TestTargetAsset" has a "parentId" of 5, the target asset with ID 5 has a "parentId" of 1, and the target asset with ID 1 has a "parentId" of null, then the "parentTargetAssetCount" field of "TestTargetAsset" should be 3. The code for determining this should be written by yourself and should not come from a library.

Think about edge cases and possible sources of errors and try to handle them cleanly. The business logic should be covered with unit tests wherever necessary. We should be able to see how you generally test business logic with UnitTests and which cases you consider important/worthy of testing.

The code changes should be submitted as a merge request from a newly created branch to the main branch on the Github Repo.

## TargetAsset-API
REST Endpoint: [https://06ba2c18-ac5b-4e14-988c-94f400643ebf.mock.pstmn.io/targetAsset) 

No Authentication is needed to the TargetAsset-API.
