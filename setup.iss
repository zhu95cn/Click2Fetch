[Setup]
AppName=Click2Fetch
AppVersion=1.0.0
AppPublisher=Click2Fetch
DefaultDirName={autopf}\Click2Fetch
DefaultGroupName=Click2Fetch
OutputDir=..\publish
OutputBaseFilename=Click2Fetch-Setup-win-x64
Compression=lzma2
SolidCompression=yes
PrivilegesRequired=lowest
SetupIconFile=src\Click2Fetch\icon.jpeg

[Files]
Source: "publish\win-x64-installer\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\Click2Fetch"; Filename: "{app}\Click2Fetch.exe"
Name: "{group}\Uninstall Click2Fetch"; Filename: "{uninstallexe}"
Name: "{autodesktop}\Click2Fetch"; Filename: "{app}\Click2Fetch.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Create desktop shortcut"; GroupDescription: "Additional icons:"

[Run]
Filename: "{app}\Click2Fetch.exe"; Description: "Launch Click2Fetch"; Flags: nowait postinstall skipifsilent
