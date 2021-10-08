# CRUD-Operations

A libary made in c# using Newtonsoft JSON for saving.<br>
To use this libary in c# just use the following code:

```c#
CRUD<yourClass> itemList = new CRUD<yourClass>("yourFileName.json");
```
With the following code you get the list: 
```c#
itemList.list.whateverYouWant
```

For more examples go to the testing file and see a implementation.
[FILE](CRUD-Operations-Testing/Program.cs)