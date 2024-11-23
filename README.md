
# DirectoryReader - assignment for Siemens Healthineers

Assignement:

Your task is to create an application in C# that has one input—a path to a directory. The application will then load
information about the given directory, including all files and nested directories, and store this information:
- For each directory, save its name, a list of all files, and a list of all nested directories it contains.
- For each file, save its name and extension.
The application will support the following functionalities:
1. Print all unique file extensions.
2. Serialize the directory information into JSON format.
3. Save the JSON to a file.
4. Deserialize the JSON file containing the directory information.
Use an object-oriented approach for the development.


## Functionalities
- Simple console UI for loading up directory content as a structured object.
- Printing unique postfixes in directory system.
- Serializing directory structure object to JSON and then possibility of saving it.
- Deserializing JSON to directory structure object with a possibility of loading this JSON file.

## Testing
- Unit tests.
- Automated GitHub workflow action to test each push to the repo.
- Workflow generates .trx test logs, which are then published as a artifact under each ran action. 

## Remarks
- When serializing, output json file will be saved to the bin\Debug\8.0net directory.
