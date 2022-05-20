[![Build status](https://ci.appveyor.com/api/projects/status/p4gqxsi0gk7sxo4x?svg=true)](https://ci.appveyor.com/project/igi/npctextprediction)

---

# npctextprediction

A webapi that using machine learning to determine which NPC from the Baldur's Gate series of games would be most likely to say a given sentence.

## Usage

Make a GET request to the api endpoint specifying the text parameter on the query string and model on the query string, e.g.

https://npctextpredictionapi.iimods.com/api/PredictBG?text=This%20is%20a%20bad%20situation&modeltype=0

There are 3 values available for the modeltype:
 0 uses a model trained on both BG1 and BG2 data
 1 uses a model trained solely on BG1 data
 2 uses a model trained solely on BG2 data 

The api will return an array of key value pairs, containing the top 5 NPC names and the confidence the ML model gives of that NPC of speaking that text.

## Download

No explicit releases are available, though you can clone this repository and build the project.


## Technologies

npctextprediction is written in C# Net Core 3 and is hosted as Azure functions.


## Compiling

To clone and run this application, you'll need [Git](https://git-scm.com) and [.NET](https://dotnet.microsoft.com/) installed on your computer. From your command line:

```
# Clone this repository
$ git clone https://github.com/btigi/npctextprediction

# Go into the repository
$ cd npctextprediction

# Build  the app
$ dotnet build
```


## License

npctextprediction is licensed under [CC BY-NC 4.0](https://creativecommons.org/licenses/by-nc/4.0/)
