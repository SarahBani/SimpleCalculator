using Build.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Build
{
    public class PipelineExecuter
    {

        #region Properties

        private const string _solutionName = "ProductMadness";
        private const string _buildProjectName = "Build";
        private const string _webAPIProjectName = "WebAPI";
        private const string _unitTestProjectName = "UnitTest";
        private const string _workspace = "Workspace";
        private const int ConsoleLineLength = 70;
        private const int ConsolePadLeftLength = 20;

        private string _solutionPath;
        private string _workspacePath;
        private string _versionWorkspacePath;
        private string _webApiProjectPath;
        private string _testProjectPath;

        /// <summary>
        /// a list of steps which should be executed
        /// </summary>
        private IList<PipelineStep> _pipelineSteps;
        protected string CurrentBuildVersion => Settings.Default.BuildVersion;

        #endregion /Properties

        #region Constructors

        public PipelineExecuter(IList<PipelineStep> steps)
        {
            this._pipelineSteps = steps;
            SetPaths();
        }

        #endregion /Constructors

        #region Methods

        private void SetPaths()
        {
            this._solutionPath = GetSolutionPath();
            this._workspacePath = GetWorkSpacePath();
            this._webApiProjectPath = $"{this._solutionPath}\\{_webAPIProjectName}";
            this._testProjectPath = $"{this._solutionPath}\\{_unitTestProjectName}";
        }

        public void RunPipeline()
        {
            ConsoleWriteLine("Start Pipline");
            GenerateNewVersion();
            ConsoleWriteLine("New Version Generated");
            this._versionWorkspacePath = $"{this._workspacePath}\\Version-{this.CurrentBuildVersion}";
            SetWorkSpace();
            ConsoleWriteLine("New version directory in workspace created");

            bool isPipelineSuccessful = true;
            foreach (var step in this._pipelineSteps)
            {
                string operationName = step.Operation.ToString();
                ConsoleWriteLine($"{operationName}ing Version {this.CurrentBuildVersion}. Please wait...");
                bool isSuccessful = RunOperation(step.Operation);
                Console.WriteLine();
                ConsoleWriteLine($"{operationName} was {(isSuccessful ? "Successful" : "Failed") }!");
                if (step.IsRequired && !isSuccessful)
                {
                    isPipelineSuccessful = false;
                    break;
                }
            }
            ConsoleWriteLine($"Pipline {(isPipelineSuccessful ? "finished successfully" : "encountered a problem") }!");
            ConsoleWriteLine("End Pipline");
            Console.ReadLine();
        }

        private void GenerateNewVersion()
        {
            string version = Settings.Default.BuildVersion;
            int buildNumber = int.Parse(version.Substring(version.LastIndexOf('.') + 1));
            buildNumber++;
            version = version.Substring(0, version.LastIndexOf('.') + 1) + buildNumber;
            Settings.Default.BuildVersion = version;
            Settings.Default.Save();
        }

        private string GetSolutionPath()
        {
            var directory = Directory.GetParent(Environment.CurrentDirectory);
            while (!directory.FullName.EndsWith(_solutionName))
            {
                directory = directory.Parent;
            }
            return $"{directory.FullName}";
        }

        private string GetWorkSpacePath()
        {
            var directory = Directory.GetParent(Environment.CurrentDirectory);
            while (!directory.FullName.EndsWith(_buildProjectName))
            {
                directory = directory.Parent;
            }
            return $"{directory.FullName}\\{_workspace}";
        }

        private void SetWorkSpace()
        {
            if (!Directory.Exists(this._workspacePath))
            {
                Directory.CreateDirectory(this._workspacePath);
            }
            if (!Directory.Exists(this._versionWorkspacePath))
            {
                Directory.CreateDirectory(this._versionWorkspacePath);
            }
        }

        public string GetChangePathScript(string path)
        {
            return $"Set-Location -Path {path} -PassThru;";
        }

        private string GetOperationScript(PipelineOperation operation) =>
            operation switch
            {
                PipelineOperation.Build => GetChangePathScript(this._webApiProjectPath) +
                    $"dotnet build -c Release -o {this._versionWorkspacePath}\\Build -p:FileVersion={this.CurrentBuildVersion};",
                PipelineOperation.Test => GetChangePathScript(this._testProjectPath) +
                    $"dotnet test --verbosity m --logger \"trx;LogFileName={this._workspacePath}\\TestResults\\Version-{this.CurrentBuildVersion}.trx\";",
                PipelineOperation.Deploy => GetChangePathScript(this._webApiProjectPath) +
                    $"dotnet publish WebAPI.csproj -c Release -o {this._versionWorkspacePath}\\Published -p:FileVersion={this.CurrentBuildVersion};",
                _ => string.Empty
            };

        private bool RunOperation(PipelineOperation operation)
        {
            string script = GetOperationScript(operation);
            script += "$lastexitcode"; // returns the status of executing the last command
            var results = PowerShellHelper.GetExecutionResult(script);
            return results.Last().Equals("0"); // 0 for successful command
        }

        public void ConsoleWriteLine(string text)
        {
            Console.WriteLine((new string('-', ConsolePadLeftLength) + text).PadRight(ConsoleLineLength, '-'));
        }

        #endregion /Methods

    }
}