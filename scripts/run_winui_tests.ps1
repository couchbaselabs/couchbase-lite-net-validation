#Hard code the VS 2022 path due to VS2017 is also installed in the agent
$VSInstall = (& 'C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe' -latest -requires Microsoft.NetCore.Component.SDK -requires Microsoft.NetCore.Component.Runtime.8.0 -property resolvedInstallationPath)
if(-Not $VSInstall) {
    throw "Unable to locate Visual Studio installation"
}

$MSBuild = "$VSInstall\MSBuild\Current\Bin\MSBuild.exe"
$VSTest = "$VSInstall\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

# Build and Test CBL debug CI nuget package

Remove-Item -Recurse -Force obj/ -ErrorAction Ignore
Remove-Item -Recurse -Force bin/ -ErrorAction Ignore

& $MSBuild --%  Couchbase.Lite.Tests.WinUI.sln /t:Restore /p:Configuration=Debug
& $MSBuild --%  Couchbase.Lite.Tests.WinUI.sln /p:Configuration=Debug /p:Platform=x64 /p:TargetFramework=net8.0-windows10.0.19041.0 /p:AppxBundle=Never /p:UapAppxPackageBuildMode=SideloadOnly /p:PackageCertificateKeyFile=Couchbase.Lite.Tests.WinUI_TemporaryKey --no-restore

& $VSTest /InIsolation bin\x64\Debug\net8.0-windows10.0.19041.0\Couchbase.Lite.Tests.WinUI.dll /platform:x64 /Logger:trx /diag:out\diagnostic.txt

# Build and Test CBL CI nuget package 

Remove-Item -Recurse -Force obj/ -ErrorAction Ignore
Remove-Item -Recurse -Force bin/ -ErrorAction Ignore

& $MSBuild --%  Couchbase.Lite.Tests.WinUI.sln /t:Restore /p:Configuration=Release
& $MSBuild --%  Couchbase.Lite.Tests.WinUI.sln /p:Configuration=Release /p:Platform=x64 /p:TargetFramework=net8.0-windows10.0.19041.0 /p:AppxBundle=Never /p:UapAppxPackageBuildMode=SideloadOnly /p:PackageCertificateKeyFile=Couchbase.Lite.Tests.WinUI_TemporaryKey

& $VSTest /InIsolation bin\x64\Release\net8.0-windows10.0.19041.0\Couchbase.Lite.Tests.WinUI.dll /platform:x64 /Logger:trx /diag:out\diagnostic.txt