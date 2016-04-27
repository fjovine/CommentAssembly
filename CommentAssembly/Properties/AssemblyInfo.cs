using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("CommentAssembly")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("HP")]
[assembly: AssemblyProduct("CommentAssembly")]
[assembly: AssemblyCopyright("Copyright © Francesco Iovine 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
// TODO [X] Activate deletion of the first row
// TODO [X] Save the closing time as parameter
// TODO [X] Save size and position of the window
// TODO [X] Do not show ID field in the list
// TODO [X] Reactivate checkbox on things to do list
// TODO [X] Save if the window should be topmost as parameter
// TODO [X] Description field not editable
// TODO [X] Insert a properties window
// TODO [X] When something has been done, add it  automatically to the compilation comment
// TODO [X] Solve the bug that stores multiple copies of the parameters
// TODO [X] The Add button should be made active only when
// TODO there is something in the todo textbox
// TODO [X] Do not leave a void row every line with the release number
// TODO [X] Delete the property SaveWindowsSize
// TODO [X] Delete "Save Windows ... " item in properties GUI
// TODO [X] Reactivate "Keep on top" option (Debug and Release versions)
// TODO [X] Reactivate "Close windows automatically" option (Debug and Release versions)
// TODO [X] Use the timeout provided in the properties
// TODO [X] When the timeout is not checked, do not show the timeout progress bar
// TODO [X] Todo thing test 1
// TODO [X] Todo thing test 2
// TODO [X] Todo thing test 3
// TODO [X] Todo thing test 4
// TODO [ ] Add a style to the window
// TODO [ ] Change the Properties checkbox bitmap with a down or up pointing arrow.
// ENDTODO
// TODO PARAM KeepOnTop=True
// TODO PARAM CloseWinAutomatically=True
// TODO PARAM ClosingTime=10
// TODO PARAM WinLocation=213;144;1080;558

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//In order to begin building localizable applications, set 
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page, 
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page, 
                                              // app, or any theme specific resource dictionaries)
)]


// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.171")]
[assembly: AssemblyFileVersion("1.0.0.171")]
//// 1.0.0.171  Compiled 27/04/2016 06.47.37
//// 1.0.0.170  Compiled 27/04/2016 06.46.24
//// 1.0.0.169  Compiled 27/04/2016 06.36.06
//// OK tested addition of things done
//// Done : Todo thing test 4
//// 1.0.0.168  Compiled 27/04/2016 06.35.28
//// Done : Todo thing test 1
//// Done : Todo thing test 2
//// Done : Todo thing test 3
//// 1.0.0.167  Compiled 27/04/2016 06.34.39
//// Corrected bug on addition of done things.
//// 1.0.0.166  Compiled 27/04/2016 06.33.17
//// Done : When something has been done, add it  automatically to the compilation comment
//// 1.0.0.165  Compiled 27/04/2016 06.26.49
//// Done : Activate deletion of the first row
//// Done : Save the closing time as parameter
//// Done : Save size and position of the window
//// Done : Do not show ID field in the list
//// Done : Reactivate checkbox on things to do list
//// Done : Save if the window should be topmost as parameter
//// Done : Description field not editable
//// Done : Insert a properties window
//// Done : Solve the bug that stores multiple copies of the parameters
//// Done : The Add button should be made active only when
//// there is something in the todo textbox
//// Done : Do not leave a void row every line with the release number
//// Done : Delete the property SaveWindowsSize
//// Done : Delete "Save Windows ... " item in properties GUI
//// Done : Reactivate "Keep on top" option (Debug and Release versions)
//// Done : Reactivate "Close windows automatically" option (Debug and Release versions)
//// Done : Use the timeout provided in the properties
//// Done : When the timeout is not checked, do not show the timeout progress bar
//// Done : When something has been done, add it  automatically to the compilation comment
//// 1.0.0.164  Compiled 27/04/2016 06.12.59
//// 1.0.0.163  Compiled 27/04/2016 05.43.25
//// Reactivated Keep On Top option on all versions.
//// 1.0.0.162  Compiled 27/04/2016 05.41.16
//// When the closing timeout is inactive, the timeout progress bar is collapsed
//// 1.0.0.161  Compiled 27/04/2016 05.40.18
//// 1.0.0.160  Compiled 27/04/2016 05.38.11
//// 1.0.0.159  Compiled 27/04/2016 05.37.43
//// 1.0.0.158  Compiled 27/04/2016 05.37.02
//// Used the timeout provided in the properties
//// 1.0.0.157  Compiled 27/04/2016 05.32.55
//// Reactivated "Close window" after timeout
//// 1.0.0.156  Compiled 27/04/2016 05.31.33
//// 1.0.0.155  Compiled 27/04/2016 05.31.06
//// 1.0.0.154  Compiled 27/04/2016 05.27.25
//// Deleted "Save Windows ..." item in GUI
//// 1.0.0.153  Compiled 27/04/2016 05.25.59
//// The ToDo Id has been deleted from the list
//// 1.0.0.152  Compiled 26/04/2016 19.57.56
//// Solved the problem of void lines between the versions
//// 1.0.0.151  Compiled 26/04/2016 19.56.34
//// 1.0.0.150  Compiled 26/04/2016 19.54.43
//// 1.0.0.149  Compiled 26/04/2016 19.34.51
//// 1.0.0.148  Compiled 26/04/2016 19.33.41
//// 1.0.0.147  Compiled 26/04/2016 19.32.50
//// 1.0.0.146  Compiled 26/04/2016 19.31.36
//// Saved window position
//// 1.0.0.145  Compiled 26/04/2016 19.30.44
//// 1.0.0.144  Compiled 26/04/2016 19.30.24
//// 1.0.0.143  Compiled 26/04/2016 19.27.39
//// 1.0.0.142  Compiled 26/04/2016 19.27.11
//// 1.0.0.141  Compiled 26/04/2016 19.26.08
//// 1.0.0.140  Compiled 26/04/2016 19.23.56
//// 1.0.0.139  Compiled 26/04/2016 19.23.00
//// 1.0.0.138  Compiled 26/04/2016 19.22.32
//// 1.0.0.137  Compiled 26/04/2016 19.22.09
//// 1.0.0.136  Compiled 26/04/2016 19.19.39
//// 1.0.0.135  Compiled 26/04/2016 18.36.39
//// 1.0.0.134  Compiled 26/04/2016 18.35.42
//// 1.0.0.133  Compiled 26/04/2016 18.30.54
//// 1.0.0.132  Compiled 26/04/2016 18.28.57
//// 1.0.0.131  Compiled 26/04/2016 18.27.53
//// Solved the bug that caused multiple storage of the todo parameters
//// 1.0.0.130  Compiled 26/04/2016 18.26.48
//// 1.0.0.129  Compiled 26/04/2016 18.23.29
//// 1.0.0.128  Compiled 26/04/2016 18.22.42

//// 1.0.0.127  Compiled 26/04/2016 18.21.16

//// 1.0.0.126  Compiled 26/04/2016 18.18.03

//// 1.0.0.125  Compiled 26/04/2016 18.16.54

//// 1.0.0.124  Compiled 15/04/2016 08.11.18

//// 1.0.0.123  Compiled 15/04/2016 08.10.04

//// 1.0.0.122  Compiled 15/04/2016 08.07.03

//// 1.0.0.121  Compiled 15/04/2016 07.07.08

//// 1.0.0.120  Compiled 15/04/2016 07.06.07

//// 1.0.0.119  Compiled 15/04/2016 07.05.24
//// Property panel active

//// 1.0.0.118  Compiled 15/04/2016 07.03.06

//// 1.0.0.117  Compiled 15/04/2016 07.00.53

//// 1.0.0.116  Compiled 15/04/2016 06.57.40

//// 1.0.0.115  Compiled 15/04/2016 06.56.13

//// 1.0.0.114  Compiled 15/04/2016 06.55.23

//// 1.0.0.113  Compiled 15/04/2016 06.50.33

//// 1.0.0.112  Compiled 15/04/2016 06.40.40

//// 1.0.0.111  Compiled 15/04/2016 06.39.56

//// 1.0.0.110  Compiled 15/04/2016 06.38.45

//// 1.0.0.109  Compiled 15/04/2016 06.37.36

//// 1.0.0.108  Compiled 15/04/2016 06.36.41

//// 1.0.0.107  Compiled 15/04/2016 06.35.32

//// 1.0.0.106  Compiled 15/04/2016 06.33.07

//// 1.0.0.105  Compiled 15/04/2016 06.21.44

//// 1.0.0.104  Compiled 15/04/2016 06.18.35
//// Il checkbox "fatto" è stato riattivato

//// 1.0.0.103  Compiled 15/04/2016 06.13.34

//// 1.0.0.102  Compiled 15/04/2016 06.05.58
//// Attivata la cancellazione della prima riga

//// 1.0.0.101  Compiled 15/04/2016 05.59.41

//// 1.0.0.100  Compiled 14/04/2016 18.56.57

//// 1.0.0.99  Compiled 14/04/2016 18.56.31

//// 1.0.0.98  Compiled 14/04/2016 18.54.11

//// 1.0.0.97  Compiled 14/04/2016 18.52.33

//// 1.0.0.96  Compiled 14/04/2016 18.51.08

//// 1.0.0.95  Compiled 14/04/2016 18.22.19

//// 1.0.0.94  Compiled 14/04/2016 17.56.38

//// 1.0.0.93  Compiled 14/04/2016 17.52.56

//// 1.0.0.92  Compiled 14/04/2016 06.46.25

//// 1.0.0.91  Compiled 14/04/2016 06.45.34

//// 1.0.0.90  Compiled 14/04/2016 06.45.04

//// 1.0.0.89  Compiled 14/04/2016 06.41.59
//// The main windows closure on timeout is only on release mode

//// 1.0.0.88  Compiled 14/04/2016 06.41.15

//// 1.0.0.87  Compiled 14/04/2016 06.40.48

//// 1.0.0.86  Compiled 14/04/2016 06.39.33

//// 1.0.0.85  Compiled 14/04/2016 06.35.00

//// 1.0.0.84  Compiled 14/04/2016 06.34.14

//// 1.0.0.83  Compiled 14/04/2016 06.32.49

//// 1.0.0.82  Compiled 14/04/2016 06.19.17

//// 1.0.0.81  Compiled 14/04/2016 06.17.41

//// 1.0.0.80  Compiled 14/04/2016 06.14.31

//// 1.0.0.79  Compiled 14/04/2016 06.14.08

//// 1.0.0.78  Compiled 14/04/2016 06.13.31

//// 1.0.0.77  Compiled 14/04/2016 06.12.48

//// 1.0.0.76  Compiled 14/04/2016 06.00.19

//// 1.0.0.75  Compiled 14/04/2016 05.55.58
//// TheToDoList is private now

//// 1.0.0.74  Compiled 14/04/2016 05.40.05

//// 1.0.0.73  Compiled 14/04/2016 05.38.47

//// 1.0.0.72  Compiled 14/04/2016 05.37.48

//// 1.0.0.71  Compiled 4/13/2016 10:38:19 PM

//// 1.0.0.70  Compiled 4/13/2016 10:37:35 PM

//// 1.0.0.69  Compiled 13/04/2016 18.42.21

//// 1.0.0.68  Compiled 13/04/2016 18.35.59

//// 1.0.0.67  Compiled 13/04/2016 18.34.08

//// 1.0.0.66  Compiled 13/04/2016 18.33.32

//// 1.0.0.65  Compiled 13/04/2016 18.23.36

//// 1.0.0.64  Compiled 13/04/2016 18.22.38

//// 1.0.0.63  Compiled 13/04/2016 18.20.31

//// 1.0.0.62  Compiled 13/04/2016 18.19.26

//// 1.0.0.61  Compiled 13/04/2016 18.09.57

//// 1.0.0.60  Compiled 13/04/2016 18.01.11

//// 1.0.0.59  Compiled 13/04/2016 18.00.13

//// 1.0.0.58  Compiled 13/04/2016 17.53.38

//// 1.0.0.57  Compiled 13/04/2016 17.52.23

//// 1.0.0.56  Compiled 13/04/2016 06.47.37

//// 1.0.0.55  Compiled 13/04/2016 06.47.05

//// 1.0.0.54  Compiled 13/04/2016 06.45.48

//// 1.0.0.53  Compiled 13/04/2016 06.40.48

//// 1.0.0.52  Compiled 13/04/2016 06.34.21

//// 1.0.0.51  Compiled 13/04/2016 06.33.53

//// 1.0.0.50  Compiled 13/04/2016 06.31.02

//// 1.0.0.49  Compiled 13/04/2016 06.29.55

//// 1.0.0.48  Compiled 13/04/2016 06.27.03

//// 1.0.0.47  Compiled 13/04/2016 06.24.30

//// 1.0.0.46  Compiled 13/04/2016 06.20.35

//// 1.0.0.45  Compiled 13/04/2016 06.15.56
//// Added todo decode and encode

//// 1.0.0.44  Compiled 13/04/2016 06.13.38

//// 1.0.0.43  Compiled 13/04/2016 05.58.53

//// 1.0.0.42  Compiled 13/04/2016 05.54.56

//// 1.0.0.41  Compiled 12/04/2016 06.17.30

//// 1.0.0.40  Compiled 12/04/2016 06.16.16

//// 1.0.0.39  Compiled 12/04/2016 06.12.10

//// 1.0.0.38  Compiled 12/04/2016 06.11.35

//// 1.0.0.37  Compiled 12/04/2016 06.10.35

//// 1.0.0.36  Compiled 12/04/2016 06.09.13
//// Added ToDo list

//// 1.0.0.35  Compiled 12/04/2016 06.08.12

//// 1.0.0.34  Compiled 24/03/2016 18.48.40
//// Inserted a progress bar to indicate the timeout clicking.

//// 1.0.0.33  Compiled 24/03/2016 18.47.26

//// 1.0.0.32  Compiled 24/03/2016 18.47.05

//// 1.0.0.31  Compiled 24/03/2016 18.37.42
//// The recent history scrollbars are activated

//// 1.0.0.30  Compiled 24/03/2016 18.36.11

//// 1.0.0.29  Compiled 24/03/2016 18.35.49

//// 1.0.0.28  Compiled 24/03/2016 18.34.47

//// 1.0.0.27  Compiled 24/03/2016 18.34.22

//// 1.0.0.26  Compiled 24/03/2016 18.33.58

//// 1.0.0.25  Compiled 24/03/2016 18.33.09

//// 1.0.0.24  Compiled 24/03/2016 18.31.30
//// If the mouse has been clicked on the window, the countdown is blocked

//// 1.0.0.23  Compiled 24/03/2016 18.29.11
//// If a key has been hit, the countdown is blocked

//// 1.0.0.22  Compiled 24/03/2016 18.28.11
//// k

//// 1.0.0.21  Compiled 24/03/2016 18.27.39

//// 1.0.0.20  Compiled 24/03/2016 18.27.09

//// 1.0.0.19  Compiled 24/03/2016 18.24.55
//// The window is kept on top of the others

//// 1.0.0.18  Compiled 24/03/2016 18.24.06

//// 1.0.0.17  Compiled 24/03/2016 18.23.45

//// 1.0.0.16  Compiled 24/03/2016 18.11.24

//// 1.0.0.15  Compiled 24/03/2016 18.01.55

//// 1.0.0.14  Compiled 24/03/2016 18.00.55

//// 1.0.0.13  Compiled 24/03/2016 17.59.08
//// Added status bar to the bottom of the main window

//// 1.0.0.12  Compiled 24/03/2016 17.56.48

//// 1.0.0.11  Compiled 24/03/2016 17.08.18

//// 1.0.0.10  Compiled 24/03/2016 17.07.36

//// 1.0.0.9  Compiled 24/03/2016 17.06.10

//// 1.0.0.8  Compiled 24/03/2016 16.52.03
//// 1.0.0.7  Compiled 24/03/2016 16.44.26
//// The project name is on the caption bar
//// 1.0.0.6  Compiled 21.03.2016 14:13:59
//// 1.0.0.5  Compiled 21.03.2016 14:03:07
//// First usable version
//// 1.0.0.4  Compiled 21.03.2016 14:02:46
//// 1.0.0.3  Compiled 21.03.2016 13:55:15
//// 1.0.0.2  Compiled 21.03.2016 13:53:00
//// 1.0.0.1  Compiled 21.03.2016 13:51:23
////
