**CommentAssembly**

**The problem**

If you develop software tools alone, composed by many executables or deployed on many computers for instance in a laboratory, keeping track of 
which version is being used becomes extremely important. In some situations, though, the development is in real time, or almost in real time, so if you 
pass your day releasing many executable, chasing a bug or trying to make something work when the same application runs on many computers in the intranet 
can be difficult. Sometimes a problem resides in the fact that not all systems run the same last version.

For this reason I use to show always the version number in the caption bar of the main window, but this is only half of the problem. If you compile with a slight
modification that makes the difference and you forgot to update manually your version number, then two recent versions seem equal but they are not.

**The solution**

This application is aimed at solving this kind of problems: every time you launch a new compilation, a dialog box will pop up where you can add a brief 
comment about the changes implemented in this version. The revision number of the Microsoft Version (rightmost, least significant number) will be automatically 
increased so every executable will be distinguishable from any other.

All this information is stored as comments into the AssemblyInfo.cs that is always part of a standard Visual Studio solution and therefore becomes a useful
documentation file.

It is a WPF appication targeted at the .NET platform that integrates into Visual Studio as launched as a pre-buil event so it is mandatorily
launched whenever a new compilation takes place.

**Installation**

In order to install **CommentAssembly**

1. Download the executable from here if you do not want to compile it
2. Copy it in the Properties folder of your project
3. Double click on the Properties file
4. Select the Build events tab. 
5. Insert in the Pre-build event command line the following "$(ProjectDir)Properties\CommentAssembly.exe" "$(ProjectDir)
6. Close the properties file.

![MainScreen](./doc/img02.png)