# AutoProjectFiles
Visual Studio 扩展程序  
用于多人开发的项目且在不提交项目文件的情况下，自动将新增的源码文件加入到项目中或者将删除的文件从项目中移除。

## 使用
下载[AutoProjectFiles](https://github.com/Onway/AutoProjectFiles/releases/download/v1.2/AutoProjectFiles.vsix)  
安装后在资源管理器中右键项目名称节点，会看到“自动更新项目”和“创建快照...”两个选项。  
“创建快照”是对项目中指定的源码目录建立一份已有文件列表，以便后续知道新增或者删除的文件。  
“自动更新项目”之后，将会对新增或删除文件更新至项目，同时刷新文件快照列表。

## vs版本 
代码是vs2013写的，生成的插件在vs2013和vs2015都能使用。

## BUGS
* 手动新增文件后提交至代码仓库，文件被其他用户删除后，又重新拉取了更新，会导致项目文件没有更新