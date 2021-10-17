using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
[DotNetVerbosityMapping]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode



    // -----------------------------------------------------------------------------------------------------------------------
    // -----------------------------------------------------------------------------------------------------------------------
    // Articles --------------------------------------------------------------------------------------------------------------
    // -----------------------------------------------------------------------------------------------------------------------
    //
    // https://blog.codingmilitia.com/2020/10/24/2020-10-24-setting-up-a-build-with-nuke/
    //
    // https://blog.dangl.me/archive/escalating-automation-the-nuclear-option/
    //
    // https://www.ariank.dev/create-a-github-release-with-nuke-build-automation-tool/
    //
    //
    // -----------------------------------------------------------------------------------------------------------------------


    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution(GenerateProjects = true)] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    [Parameter] string GitHubAuthenticationToken = "TODO: GitHubAuthenticationToken";
    [Parameter] string NugetApiUrl = "https://api.nuget.org/v3/index.json";
    [Parameter] string NugetApiKey = "TODO: NugetApiKey";

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
                .EnableNoRestore());
            //.SetAssemblyVersion(GitVersion.AssemblySemVer)
            //.SetFileVersion(GitVersion.AssemblySemFileVer)
            //.SetInformationalVersion(GitVersion.InformationalVersion))
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
        .TriggeredBy(Test)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution.GetProject(Solution.PdfSharpWrapper))
                .SetOutputDirectory(ArtifactsDirectory)
                .SetConfiguration(Configuration)
                .SetIncludeSymbols(true)
                .EnableContinuousIntegrationBuild());
        });

    // Push
    // https://cfrenzel.com/publishing-nuget-nuke-appveyor/
}
