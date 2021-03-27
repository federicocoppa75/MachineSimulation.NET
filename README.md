# MachineSimulation.NET
This project is an evolution of [**MachineSimulation.DX**](https://github.com/federicocoppa75/MachineSimulation.DX) and was boarn with these targets:
* switch from .NET Framework to .NET Core and .NET Standard;
* load machine data from server REST.

## Client
Application for load machine element models file (*.stl) and post it to data server. 
## Client.Machine
Application for load machine structure file (*.xml) made by [**MachineEditor**](https://github.com/federicocoppa75/MachineEditor#machineeditor) and post to data server or save as JSON file.

## Client.Tooling
Application for load machine tooling file (*.tooling) made by [**ToolingEditor**](https://github.com/federicocoppa75/MachineEditor#toolingeditor) ad post to data server or save as jTooling file (JSON format).

## Client.Tools
Application for load tools file (*.tools) made by [**ToolEditor**](https://github.com/federicocoppa75/MachineEditor#toolingeditor) ad post to data server or save ad JTools file (JSON format). 
## Machine.Data
Data model class library.
## Machine.Viewer
Application for view the machine with relative tooling, the data could be load from files made by the applications of this solution or by data server.

## Machine.ViewModels
ViewModels class library.S

## Machine.Views
Data view class library.

## Mesh.Data
Machine element models data model library.
## Server
Server REST for store and get machine data (structure, tooling, elements models). The data is stored by SQLite.

