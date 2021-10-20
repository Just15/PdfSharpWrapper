using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using System.Linq;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
[DotNetVerbosityMapping]
class Build : NukeBuild
{
    // Nuke Build ---------------------------------------------------------------------------------------------------------------
    //
    // https://blog.codingmilitia.com/2020/10/24/2020-10-24-setting-up-a-build-with-nuke/
    //
    // https://cfrenzel.com/publishing-nuget-nuke-appveyor/
    //
    // https://blog.dangl.me/archive/escalating-automation-the-nuclear-option/
    //
    // https://www.ariank.dev/create-a-github-release-with-nuke-build-automation-tool/

    // GitVersion ---------------------------------------------------------------------------------------------------------------
    //
    // https://gitversion.net/docs/learn/branching-strategies/gitflow/examples

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution(GenerateProjects = true)] readonly Solution Solution;
    [GitVersion] readonly GitVersion GitVersion;
    [GitRepository] readonly GitRepository GitRepository;

    [Parameter] string GitHubAuthenticationToken = "TODO: GitHubAuthenticationToken";
    [Parameter] readonly string NugetApiUrl = "https://api.nuget.org/v3/index.json"; // Default
    [Parameter] readonly string NugetApiKey;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion));
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .TriggeredBy(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());
        });

    Target Pack => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution.GetProject(Solution.PdfSharpWrapper))
                .SetOutputDirectory(ArtifactsDirectory)
                .SetConfiguration(Configuration)
                .SetIncludeSymbols(true)
                .EnableContinuousIntegrationBuild()
                .SetVersion(GitVersion.NuGetVersionV2));
        });

    Target Push => _ => _
        .DependsOn(Pack)
        .Requires(() => NugetApiUrl)
        .Requires(() => NugetApiKey)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {
            GlobFiles(ArtifactsDirectory, "*.nupkg")
                .NotEmpty()
                .Where(x => !x.EndsWith("symbols.nupkg"))
                .ForEach(x =>
                {
                    DotNetNuGetPush(s => s
                        .SetTargetPath(x)
                        .SetSource(NugetApiUrl)
                        .SetApiKey(NugetApiKey)
                    );
                });
        });
}
