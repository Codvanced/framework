#r @"packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.ProcessHelper
open Fake.Testing

Log "fake started..."

let artifactsDir = "./artifacts"
let buildDir = artifactsDir + "/build/"
let testDir = artifactsDir + "/tests/"
let deployDir = artifactsDir + "/deploy/"

let version = "3.0.0"

Target "Clean" (fun _ ->
    killMSBuild()
    CleanDirs [buildDir; testDir; deployDir]
)

Target "SetVersions" (fun _ ->
    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Abstraction/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Abstraction"
         Attribute.Description "IOC.FW.Abstraction"
         Attribute.Product "Abstraction"
         Attribute.Guid "5b3d5261-8415-433c-a27d-b22ca4e991b7"
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Configuration/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Configuration"
         Attribute.Description "IOC.FW.Configuration"
         Attribute.Product "Configuration"
         Attribute.Guid "28c6b3ed-c9b5-417c-8e99-c24b450de618"
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.ContainerManager.DryIoc.Mvc/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.ContainerManager.DryIoc.Mvc"
         Attribute.Description "IOC.FW.ContainerManager.DryIoc.Mvc"
         Attribute.Product "ContainerManager DryIoc Mvc"
         Attribute.Guid "1e10e9fa-c756-465f-b106-a985940da8cb"
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.ContainerManager.SimpleInjector.WebApi/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.ContainerManager.SimpleInjector.WebApi"
         Attribute.Description "IOC.FW.ContainerManager.SimpleInjector.WebApi"
         Attribute.Product "ContainerManager SimpleInjector WebApi"
         Attribute.Guid "41a971f9-06d1-4e50-ab3a-7fedb7fc4710"
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Cryptography/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Cryptography"
         Attribute.Description "IOC.FW.Cryptography"
         Attribute.Product "Cryptography"
         Attribute.Guid "9ca72a18-42db-4181-b011-36226861af35"
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Data/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Data"
         Attribute.Description "IOC.FW.Data"
         Attribute.Product "Data"
         Attribute.Guid "4e0a9c6c-3368-4b83-92f5-6fda2679c434"
         Attribute.Version version
         Attribute.FileVersion version]
    
    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Expressions/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Expressions"
         Attribute.Description "IOC.FW.Expressions"
         Attribute.Product "Expressions"
         Attribute.Guid "10ba2aa7-0828-4dd2-8c22-a123255e207c"
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Extensions/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Extensions"
         Attribute.Description "IOC.FW.Extensions"
         Attribute.Product "Extensions"
         Attribute.Guid "68100b12-1020-42ca-bea6-3e0cd9ddd040"
         Attribute.Version version
         Attribute.FileVersion version]
    
    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.FTP/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.FTP"
         Attribute.Description "IOC.FW.FTP"
         Attribute.Product "FTP"
         Attribute.Guid "6671d840-ca72-4f9a-9624-7e3ffa97f520"
         Attribute.Version version
         Attribute.FileVersion version]
    
    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.ImageTransformation/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.ImageTransformation"
         Attribute.Description "IOC.FW.ImageTransformation"
         Attribute.Product "Transformation"
         Attribute.Guid "370ff938-8cdb-4a93-8e48-c85d52808e03"
         Attribute.Version version
         Attribute.FileVersion version]
    
    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Logging/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Logging"
         Attribute.Description "IOC.FW.Logging"
         Attribute.Product "Logging"
         Attribute.Guid "6089782a-ea12-4f14-9d24-bcab9f9f0163"
         Attribute.Version version
         Attribute.FileVersion version]
    
    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.POP3/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.POP3"
         Attribute.Description "IOC.FW.POP3"
         Attribute.Product "POP3"
         Attribute.Guid "8cf26fb8-dfc9-485f-a997-fddaa5282aae"
         Attribute.Version version
         Attribute.FileVersion version]
    
    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Repository/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Repository"
         Attribute.Description "IOC.FW.Repository"
         Attribute.Product "Repository"
         Attribute.Guid "a2718b96-3f93-4d29-8942-e841f9eb5457"
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Repository.EF6/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Repository.EF6"
         Attribute.Description "IOC.FW.Repository.EF6"
         Attribute.Product "Repository Entity Framework 6"
         Attribute.Guid "5ce716f2-e7c2-4149-abf8-4ad319bf96d8"
         Attribute.Version version
         Attribute.FileVersion version]
    
    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Validation/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Validation"
         Attribute.Description "IOC.FW.Validation"
         Attribute.Product "Validation"
         Attribute.Guid "7a23f19f-96ee-44c7-81d9-3cdd4cb7b9f8"
         Attribute.Version version
         Attribute.FileVersion version]
    
    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Web/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Web"
         Attribute.Description "IOC.FW.Web"
         Attribute.Product "Web"
         Attribute.Guid "92fd7376-973c-4e0d-8739-1a42ab860018"
         Attribute.Version version
         Attribute.FileVersion version]
    
    CreateCSharpAssemblyInfo "./Source/CSharp/IoCFramework/IOC.FW.Web.MVC/Properties/AssemblyInfo.cs"
        [Attribute.Title "IOC.FW.Web.MVC"
         Attribute.Description "IOC.FW.Web.MVC"
         Attribute.Product "Web MVC"
         Attribute.Guid "7398f722-b0e4-405e-881d-7277074abc18"
         Attribute.Version version
         Attribute.FileVersion version]
)

Target "BuildApp" (fun _ ->
    !! @"./Source/CSharp/IoCFramework/**/*.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
    !! @"./Tests/CSharp/IoC.FW.Test/**/*.csproj"
      |> MSBuildRelease testDir "Build"
      |> Log "AppBuild-Output: "
)

Target "xUnitTest" (fun _ ->
    !! (testDir + @"/*Tests*.dll")
        |> xUnit2 (fun p ->
            {p with
                ShadowCopy = false;
                HtmlOutputPath = Some (testDir @@ "framework-tests.html"); })
)

"Clean"
  ==> "SetVersions"
  ==> "BuildApp"
  ==> "BuildTest"
  ==> "xUnitTest"
  
RunTargetOrDefault "xUnitTest"