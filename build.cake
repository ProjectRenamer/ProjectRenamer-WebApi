 #addin nuget:?package=Cake.Kudu.Client
 
 // STAGE NAMES

string StageClean       = "Clean";
string StageRestore     = "Restore";
string StageBuild       = "Build";
string StageTest        = "Test";
string StagePack        = "Pack";
string StageMigrate     = "Migrate";
string StagePublish     = "Publish";
string StageDefault     = "RESULT";

// RUN OPERATION
// VARIABLES

string config          = Argument("config", "Release");
string slnPath         = Argument("sln_path", "./ProjectRenamer.WebApi.sln");
string webAppPath      = Argument("web_app_path", "./ProjectRenamer.Api/ProjectRenamer.Api.csproj");
string migratorAppPath = Argument($"migrator_app_path", FindMigratorAppDll());
string baseUri         = Argument("azure_uri", ""); //https://{yoursite}.scm.azurewebsites.net
string userName        = Argument("azure_uname", "");
string password        = Argument("azure_pass","");

string outputPath       = "./dist";

public string FindMigratorAppDll()
{
    var migrators = GetFiles("./**/" + config + "/**/*.Migrator.dll");
    var file = migrators.FirstOrDefault();
    return file?.ToString() ?? string.Empty;
}

Task(StageDefault)
.IsDependentOn(StagePublish);

Task(StagePublish)
.IsDependentOn(StageMigrate)
.Does(()=> 
{
    IKuduClient kuduClient = KuduClient(baseUri, userName, password);
    kuduClient.ZipDeployDirectory(outputPath);
});

Task(StageMigrate)
.IsDependentOn(StagePack)
.Does(()=>
{
    if(!string.IsNullOrEmpty(migratorAppPath))
    {
        DotNetCoreExecute(migratorAppPath);
    }
    else
    {
        Console.WriteLine("Migrator not found");    
    }
});

Task(StagePack)
.IsDependentOn(StageTest)
.Does(()=>
{
    DotNetCorePublish(webAppPath,
                      new DotNetCorePublishSettings()
                      {
                            Configuration = config,
                            OutputDirectory = outputPath,
                            ArgumentCustomization = args => args.Append("--no-restore"),
                      });
});

Task(StageTest)
.IsDependentOn(StageBuild)
.Does(()=>
{
    var projects = GetFiles("./**/*Test.csproj");
    bool success = true;
    foreach(var project in projects)
    {
        Information("Testing project " + project);
        try
        {
            DotNetCoreTest(project.ToString(),
                            new DotNetCoreTestSettings()
                            {
                                Configuration = config,
                                NoBuild = true,
                                ArgumentCustomization = args => args.Append("--no-restore"),
                            });
        }
        catch(Exception e)
        {
            success = false;
            Console.WriteLine(e);
        }
        
        if(!success)
        {
            throw new Exception(StageBuild + " FAIL");
        }
    }
});

Task(StageBuild)
.IsDependentOn(StageRestore)
.Does(()=>
{
    DotNetCoreBuild(slnPath, new DotNetCoreBuildSettings()
                        {
                            Configuration = config,
                            ArgumentCustomization = args => args.Append("--no-restore"),
                        });
});

Task(StageRestore)
.IsDependentOn(StageClean)
.Does(()=>
{
    DotNetCoreRestore(slnPath);
});


Task(StageClean)
.Does(()=>
{
    var objDirectories = GetDirectories("./**/obj/*");

    foreach(var directory in objDirectories)
    {
        Console.WriteLine(directory);
        DeleteDirectory(directory, new DeleteDirectorySettings
        {
            Force = true,
            Recursive  = true
        });
    }
    
    var binDirectories = GetDirectories("./**/bin/*");

    foreach(var directory in binDirectories)
    {
        Console.WriteLine(directory);
        DeleteDirectory(directory, new DeleteDirectorySettings {
            Force = true,
            Recursive  = true
        });
    }
    
});


RunTarget(StageDefault);