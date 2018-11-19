# FileProcessor
This application has been build in JetBrain Rider on MacOS system.
It's using .NET Framework 4.6 and C#6. 

The application can be open in the Visual Studio straight away. 
However the Unit Test Framework it uses is NUnit3, which is the only one that supports in Rider IDE.
Visual Studio needs NUnit3 plugin from NuGet to run it. 

My instance of Visual Studio 2017 failed to install NUnit3, so the tests has only been ran on Rider IDE not Visual Studio.
I might expect some different behaviour in FileReaderTest when trying to access test files from Test project directory. 
