# CRUD-Operations

## What it is
This NuGet-Package supports all List functions with the additional function to instantly save new data into a JSON File. In the [FILE](JSON-CRUD-EXAMPLE/Program.cs) is a example of how you can use the commands. If you need LINQ use the Get() and Set() Function where you have full LINQ support because its a normal List. If you are missing a feature, please create an issue.

## How it works
### Example
In [here](JSON-CRUD-EXAMPLE/Program.cs) you can find an example of how you can use it.

### Short Version
```c#
CRUD<yourClass> itemList = new CRUD<yourClass>("yourFileName.json");
```
With the following code you get the list: 
```c#
itemList.whateverYouWant
```

## Licence
You can use this project everywhere you need it, just always refer to this [GITHUB repo](https://github.com/robinmuff/CRUD-Operations).