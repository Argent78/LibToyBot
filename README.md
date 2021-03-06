
# Toy Robot Library
[![Build Status](https://dev.azure.com/argent78/LibToyBot/_apis/build/status/Argent78.LibToyBot?branchName=master)](https://dev.azure.com/argent78/LibToyBot/_build/latest?definitionId=1&branchName=master) [![LibToyBot package in ToyBotFeed feed in Azure Artifacts](https://feeds.dev.azure.com/argent78/96844f69-5cd8-49ae-a3c7-2320a96fc969/_apis/public/Packaging/Feeds/22785776-25d8-473d-b4b0-855810cdaad5/Packages/5e977aee-68b6-4f29-ba97-6f60e360f6f5/Badge)](https://dev.azure.com/argent78/LibToyBot/_packaging?_a=package&feed=22785776-25d8-473d-b4b0-855810cdaad5&package=5e977aee-68b6-4f29-ba97-6f60e360f6f5&preferRelease=true) ![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/argent78/LibToyBot/1)

This library allows for a simulation of a toy robot moving on a 5 x 5 square table top. 

The robot is considered to take up the entire space of one grid element. It's movement is discrete and can only move from one grid element to an adjacent grid element. It cannot occupy more than one grid element at the same time.

```c#
                    N
   +------+------+------+------+------+
   |      |      |      |      |      |
   | 0,4  |      |      |      | 4,4  |
   +----------------------------------+
   |      |      |      |      |      |
   |      |      |      |      |      |
   +----------------------------------+ 
 W |      |      |      |      |      | E
   |      |      |      |      |      |
   +----------------------------------+
   |      |      |      |      |      |
   |      |      |      |      |      |
   +------+------+------+------+------+
   |      |      |      |      |      |
   | 0,0  |      |      |      | 4,0  |
   +------+------+------+------+------+
                    S
```

The library is an implementation of the `Toy Robot Simulator`. It is written in C# and targets .NET Standard. 


## Disclaimer 
The containing codebase is presented as an *example* of producing a nuget library; it is not an actual library intended for real use.

### Prerequisites

Requires Visual Studio installed with .NET Standard 2.1 and a Git client (i.e. SourceTree)


### Design overview
A basic component diagram was used as a guide for developing this library. 

![Component diagram](https://github.com/Argent78/LibToyBot/blob/master/doc/Component%20Diagram.png?raw=true)


### Building the library
This library can be built from the command line using the `dotnet` cli.

Assuming this repository was cloned to the local directory `C:\Git\LibToyBot`:

```
> cd C:\Git\LibToyBot

> dotnet build
```

### Packaging the library
To manually package the library, run the following command:

```
> cd C:\Git\LibToyBot

> dotnet pack
```

Note that this pack command will package a Debug build with a version number of 1.0.0. 

## Using the library

Add a nuget.config file to your project, in the same folder as your .csproj or .sln file:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="ToyBotFeed" value="https://pkgs.dev.azure.com/argent78/LibToyBot/_packaging/ToyBotFeed/nuget/v3/index.json" />
  </packageSources>
</configuration>
```

Then install the LibToyBot nuget package:

```
PM> Install-Package LibToyBot
```

Place a robot on the table at the specified co-ordinates, move it around and finish with a report command on the robot's position:

```c#
    public void PlaceAndMoveRobot()
    {
        Robot robot = GetRobot();
        robot.Action("PLACE 1,2,EAST");
        robot.Action("MOVE"); // 2,2,EAST
        robot.Action("LEFT"); // 2,2,NORTH
        robot.Action("MOVE"); // 2,3,NORTH
        robot.Action("MOVE"); // 2,4,NORTH
        IOutcome outcome = robot.Action("REPORT");
        Console.WriteLine($"Position: {outcome?.Message}"); // Print position & orientation
    }
```

If you need to get feedback on the outcome of each action, then use the return object from the `robot.Action()` method:

```c#
    public void PlaceAndMoveRobotWithOutcomes()
    {
        Robot robot = GetRobot();
        robot.Action("PLACE 4,2,EAST");
        IOutcome outcome = robot.Action("MOVE"); // Can't move, it will fall off the table
        if (outcome.Result == OutcomeStatus.Fail)
        {
            Console.WriteLine(outcome.Message);
        }
    }
```


How one gets a reference to the Robot instance depends on the framework being used.

### .NET Core
If consuming this library from a .NET Core project, add an additional reference to the `LibToyBot.DependencyInjection` nuget package:

```
PM> Install-Package LibToyBot.DependencyInjection
```

Then implement the `GetRobot()` method:

```C#
    private static Robot GetRobot()
    {
        return new ServiceCollection()
            .AddRobot()
            .BuildServiceProvider()
            .GetService<Robot>();
    }
```

See `sample\LibToyBot.Sample` for sample code.

### Alternatives to IoC
If your solution does not make use of an IoC container, then install the `LibToyBot.ToyBox` library:

```
PM> Install-Package LibToyBot.ToyBox
```

Then get a reference to the Robot from the ToyBox:

```C#
    private static Robot GetRobot()
    {
        return ToyBox.Robot;
    }
```

## Unit & Functional Tests

There are two test projects for this library. `LibToyBot.Test` is a suite of unit tests that were produced using a `Test Driven Development` approach. These tests are built using xUnit and NSubstitute, and test each component in isolation.

Functional tests are contained in the `LibToyBot.FunctionalTest` project. These tests do not use any mocking, and instead offer an end-to-end test of all components as if they were used from a real application.

These functional tests are documented using [Bddfy](https://github.com/TestStack/TestStack.BDDfy), with `stories` derived verbatim from the [requirements document](https://github.com/Argent78/LibToyBot/blob/master/doc/Toy%20Robot%20Simulator.docx).



### Running the tests

Test can be run using the `dotnet test` command:

```
> cd C:\Git\LibToyBot

> dotnet test
```

When run from the repository root, this will run all test projects, producing output similar to this:
```
Test Run Successful.
Total tests: 11
 Total time: 0.9655 Seconds

Test Run Successful.
Total tests: 144
     Passed: 144
 Total time: 0.9915 Seconds
 ```

Functional tests offer additional output when run as follows:

```
> cd C:\Git\LibToyBot\src\LibToyBot.FunctionalTest
> dotnet test --no-build --logger trx --results-directory _temp 
```

This will produce output with a `trx` results file in the `_temp` directory. This output can also be viewed via the [Azure DevOps pipeline](https://dev.azure.com/argent78/LibToyBot/_build?definitionId=1&_a=summary).


### Code Coverage
Code coverage reporting is provided by `Coverlet`, and produces a `Cobertura` xml result file.
To view test coverage, run the following command:

```
> cd C:\Git\LibToyBot\src\LibToyBot.FunctionalTest
> dotnet test --no-build /p:CollectCoverage=true
```

This will produce output similar to the following:

```
+-------------------------------+--------+--------+--------+
| Module                        | Line   | Branch | Method |
+-------------------------------+--------+--------+--------+
| LibToyBot.DependencyInjection | 100%   | 100%   | 100%   |
+-------------------------------+--------+--------+--------+
| LibToyBot                     | 93.38% | 80.43% | 91.07% |
+-------------------------------+--------+--------+--------+

+---------+--------+--------+--------+
|         | Line   | Branch | Method |
+---------+--------+--------+--------+
| Total   | 93.75% | 80.43% | 91.22% |
+---------+--------+--------+--------+
| Average | 96.69% | 90.21% | 95.53% |
+---------+--------+--------+--------+
```

## Continuous Integration

An [Azure DevOps pipeline](https://dev.azure.com/argent78/LibToyBot/_build?definitionId=1&_a=summary) is used to build, test and package this library.
Builds trigger automatically on commits to `develop` or `master` branch, as well as for pull-requests.

For `develop` or `master` branches, the pipeline will produce a nuget package.
Builds produced from the `develop` branch have the `-preview` suffix in their version string.

Nuget packages are published to an Azure artifacts feed. For a 'real' library, this would instead publish to Nuget.org.


## Branching Strategy

Releasable packages are built from the `master` branch. No direct commits occur against the `master` branch; commits to `master` only occur via pull-requests from the `develop` branch.

The `develop` branch is used for building & testing new features that are not yet releasable. All new features are implemented by branching from the `develop` branch. Direct commits to `develop` are allowed but discouraged, and usually reserved for small corrections or updates (like typos).

## Versioning

This library uses [SemVer](http://semver.org/) for versioning.


## Built With

* [Visual Studio Community Edition](https://visualstudio.microsoft.com/vs/community) - The development environment used
* [VS Code](https://code.visualstudio.com) - Azure Pipeline editor-of-choice
* [SourceTree](https://www.sourcetreeapp.com) - Source code management
* [draw.io](https://www.draw.io) - Web-based flowchart tool
* [asciiflow.com](https://asciiflow.com/) - ASCII grid-drawing tool
