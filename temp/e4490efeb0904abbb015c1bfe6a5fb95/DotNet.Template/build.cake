 #21din nuget:?package=Cake.Kudu.Client
 
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
string framework       = Argument("framework", "netcoreapp2.0");
string webAppPath      = Argument("web_app_path", "./DotNet.Template.Api/DotNet.Template.Api.csproj");
string migratorAppPath = Argument($"migrator_app_path", "./DotNet.Template.Migrator/DotNet.Template.Migrator.csproj");
string baseUri         = Argument("azure_uri", ""); //https://{yoursite}.scm.azurewebsites.net
string userName        = Argument("azure_uname", "");
string password        = Argument("azure_pass","");

string webAppOutputPath= "./dist-webapp";

Task(StageDefault)
.IsDependentOn(StagePublish);

Task(StagePublish)
.IsDependentOn(StageMigrate)
.Does(()=> 
{
    IKuduClient kuduClient = KuduClient(baseUri, userName, password);
    kuduClient.ZipDeployDirectory(webAppOutputPath);
});

Task(StageMigrate)
.IsDependentOn(StagePack)
.Does(()=>
{
    if(!string.IsNullOrEmpty(migratorAppPath))
    {
        var settings = new DotNetCoreRunSettings
        {
            Framework = framework,
            Configuration = config
        };
        DotNetCoreRun(migratorAppPath, "", settings);
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
                            OutputDirectory = webAppOutputPath,
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
            throw new Exception(StageTest + " FAIL");
        }
    }
});

Task(StageBuild)
.IsDependentOn(StageRestore)
.Does(()=>
{
    DotNetCoreBuild(".", new DotNetCoreBuildSettings()
                        {
                            Configuration = config,
                            ArgumentCustomization = args => args.Append("--no-restore"),
                        });
});

Task(StageRestore)
.IsDependentOn(StageClean)
.Does(()=>
{
    DotNetCoreRestore();
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
    
    var distDirectories = GetDirectories("./**/dist*");

    foreach(var directory in distDirectories)
    {
        Console.WriteLine(directory);
        DeleteDirectory(directory, new DeleteDirectorySettings {
            Force = true,
            Recursive  = true
        });
    }
    
});


RunTarget(StageDefault);