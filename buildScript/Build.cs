using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.PublishTarget);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    Target Clean => _ => _
        .Before(RestorePackages)
        .Executes(() =>
        {
            GlobDirectories(Solution.Directory, "**/src/bin", "**/src/obj").ForEach(DeleteDirectory);
            //DotNetTasks.DotNetClean(a=>a.SetProject(Solution));
        });

    Target RestorePackages => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(a => a.SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(RestorePackages)
        .Executes(() =>
        {
            DotNetTasks.DotNetBuild(a => a.SetProjectFile(Solution).EnableNoRestore());
        });

    Target RunUnitTests => _ => _
                                .DependsOn(Compile)
                               .Executes(() =>
                               {
                                   //var testProjects = Solution.Projects
                                   //                           .Where(a => a.Name.EndsWith("tests.unit", StringComparison.OrdinalIgnoreCase))
                                   //                           .ToList();

                                   //foreach (var testProject in testProjects)
                                   //{
                                   //    DotNetTasks.DotNetTest(a=>a.SetProjectFile(testProject)
                                   //                               .EnableNoBuild()
                                   //                               .EnableNoRestore());
                                   //}
                               });


    Target PublishTarget => _ => _
                           .DependsOn(RunUnitTests)
                           .Executes(() =>
                           {
                               var publishSettings = new DotNetPublishSettings();
                               DotNetTasks.DotNetPublish(s => publishSettings
                                                                                          .SetProject(Solution));
                           });
}
