#Hard code the VS 2022 path due to VS2017 is also installed in the agent
$VSInstall = "C:\Program Files\Microsoft Visual Studio\2022\Professional"
#$VSInstall = (Get-CimInstance MSFT_VSInstance).InstallLocation
if(-Not $VSInstall) {
    throw "Unable to locate Visual Studio installation"
}

$MSBuild = "$VSInstall\MSBuild\Current\Bin\MSBuild.exe"
$VSTest = "$VSInstall\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

cd Couchbase.Lite.Tests.WinUI

.\modify_packages.ps1 -Version $Env:VERSION

# Build and Test CBL debug CI nuget package

Remove-Item -Recurse -Force obj/ -ErrorAction Ignore
Remove-Item -Recurse -Force bin/ -ErrorAction Ignore

& $MSBuild --%  Couchbase.Lite.Tests.WinUI.sln /t:Restore /p:Configuration=Debug
& $MSBuild --%  Couchbase.Lite.Tests.WinUI.sln /p:Configuration=Debug /p:Platform=x64 /p:TargetFramework=net6.0-windows10.0.19041.0 /p:AppxBundle=Never /p:UapAppxPackageBuildMode=SideloadOnly /p:PackageCertificateKeyFile=Couchbase.Lite.Tests.WinUI_TemporaryKey --no-restore

& $VSTest /InIsolation bin\x64\Debug\net6.0-windows10.0.19041.0\Couchbase.Lite.Tests.WinUI.dll /platform:x64 /Logger:trx /diag:out\diagnostic.txt

# Build and Test CBL CI nuget package 

Remove-Item -Recurse -Force obj/ -ErrorAction Ignore
Remove-Item -Recurse -Force bin/ -ErrorAction Ignore

& $MSBuild --%  Couchbase.Lite.Tests.WinUI.sln /t:Restore /p:Configuration=Release
& $MSBuild --%  Couchbase.Lite.Tests.WinUI.sln /p:Configuration=Release /p:Platform=x64 /p:TargetFramework=net6.0-windows10.0.19041.0 /p:AppxBundle=Never /p:UapAppxPackageBuildMode=SideloadOnly /p:PackageCertificateKeyFile=Couchbase.Lite.Tests.WinUI_TemporaryKey

& $VSTest /InIsolation bin\x64\Release\net6.0-windows10.0.19041.0\Couchbase.Lite.Tests.WinUI.dll /platform:x64 /Logger:trx /diag:out\diagnostic.txt