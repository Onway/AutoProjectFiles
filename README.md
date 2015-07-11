AutoProjectFiles

A VSPackage for automatic including source files to project or excluding from project.

In some cases, project file will not checkin repository, and when checkout new files or deleted files, we need to manually include them to project or exclude from project.  
Let this VSPackage takes snapshot for some folders, so it knows which file is added or removed and auto updates the project file.  

After installed right click on project node in the Solution Explorer, there are two menu items call "Auto Project Files" and "Create Snapshot...".  
Codes wrote in vs2013, .NET 4.5 and just little tested in vs2013 community edition.
