using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using Dutchskull.Aspire.PolyRepo.Interfaces;
using LibGit2Sharp;

namespace Dutchskull.Aspire.PolyRepo;

public class ProcessCommandExecutor : IProcessCommandExecutor
{
    public int BuildDotNetProject(string resolvedProjectPath, string[]? args = null)
    {
        StringBuilder arguments = new("build ");
        arguments.Append(resolvedProjectPath);
        if (args != null && args.Length > 0)
        {
            arguments.Append(' ');
            arguments.Append(string.Join(' ', args));
        }

        BufferedCommandResult result = RunBuffered("dotnet", arguments.ToString()).GetAwaiter().GetResult();
        Console.WriteLine(result.StandardOutput);
        if (result.ExitCode != 0)
        {
            string err = result.StandardError;
            Console.WriteLine(err);
            throw new Exception($"Process dotnet {arguments} failed with exit code {result.ExitCode}: {err}");
        }

        Console.WriteLine($"Process dotnet {arguments} finished successfully.");
        return result.ExitCode;
    }

    public int NpmInstall(string resolvedRepositoryPath)
    {
        string arguments = $"/C cd \"{resolvedRepositoryPath}\" && npm i";
        BufferedCommandResult result = RunBuffered("cmd.exe", arguments).GetAwaiter().GetResult();
        Console.WriteLine(result.StandardOutput);
        if (result.ExitCode != 0)
        {
            Console.WriteLine(result.StandardError);
            throw new Exception($"Process cmd.exe {arguments} failed with exit code {result.ExitCode}: {result.StandardError}");
        }

        Console.WriteLine($"Process cmd.exe {arguments} finished successfully.");
        return result.ExitCode;
    }


    private static async Task<BufferedCommandResult> RunBuffered(string fileName, string arguments)
    {
        StringBuilder stdOutBuilder = new();
        StringBuilder stdErrBuilder = new();

        Command cmd = Cli
            .Wrap(fileName)
            .WithArguments(arguments)
            .WithValidation(CommandResultValidation.None)
            .WithStandardOutputPipe(PipeTarget.Merge(
                PipeTarget.ToStringBuilder(stdOutBuilder),
                PipeTarget.ToStream(Console.OpenStandardOutput())))
            .WithStandardErrorPipe(PipeTarget.Merge(
                PipeTarget.ToStringBuilder(stdErrBuilder),
                PipeTarget.ToStream(Console.OpenStandardError())));

        return await cmd.ExecuteBufferedAsync();
    }
}
