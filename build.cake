 #addin nuget:?package=Cake.Kudu.Client
 
 // STAGE NAMES

string CheckEnvVarStage = "Check Env Var";
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
string baseUri         = Argument("azure_uri", ""); //https://{yoursite}.scm.azurewebsites.net
string userName        = Argument("azure_uname", "");
string password        = Argument("azure_pass","");
string branchName      = Argument("branchName","");
string selectedEnv     = string.Empty;

var BranchEnvironmentPairs = new Dictionary<string, string>()
{
    {"master","prod" }
};

string[] AutoReleaseEnv = new []
{
    "prod"
};


string outputPath       = "./dist";

Task(StageDefault)
.IsDependentOn(StagePublish);

Task(StagePublish)
.IsDependentOn(StagePack)
.Does(()=> 
{
    if(!AutoReleaseEnv.Contains(selectedEnv))
    {
        Console.WriteLine($"Auto Release is deactive for this environment [{selectedEnv}]. [BranchName : {branchName}]");
        return;
    }

    IKuduClient kuduClient = KuduClient(baseUri, userName, password);
    kuduClient.ZipDeployDirectory(outputPath);
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
.IsDependentOn(CheckEnvVarStage)
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

Task(CheckEnvVarStage)
.Does(()=>
{
    if(string.IsNullOrEmpty(branchName))
    {
        throw new Exception("Branch Name should be provided");
    }
    Console.WriteLine("Branch Name = " + branchName);

    if(BranchEnvironmentPairs.ContainsKey(branchName))
    {
        selectedEnv = BranchEnvironmentPairs[branchName];
    }

    Console.WriteLine("Selected Env = " + selectedEnv);

});

RunTarget(StageDefault);