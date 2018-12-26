using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitDrawer.GitCore
{
    public class GitWorker
    {
        string UserName;
        string RepositoryName;

        ProcessStartInfo gitInfo = new ProcessStartInfo();

        public GitWorker(string gitInstalledDirectory)
        {
            gitInfo.CreateNoWindow = true;
            gitInfo.RedirectStandardError = true;
            gitInfo.RedirectStandardOutput = true;
            gitInfo.FileName = gitInstalledDirectory + @"\bin\git.exe";
        }

        public string GitCommand(string repositoryPath, string command)
        {
            Process gitProcess = new Process();
            gitInfo.Arguments = command; // such as "fetch orign"
            gitInfo.WorkingDirectory = repositoryPath;
            gitInfo.UseShellExecute = false;

            gitProcess.StartInfo = gitInfo;
            gitProcess.Start();

            string stderr_str = gitProcess.StandardError.ReadToEnd();  // pick up STDERR
            string stdout_str = gitProcess.StandardOutput.ReadToEnd(); // pick up STDOUT

            gitProcess.WaitForExit();
            gitProcess.Close();

            return stderr_str;
        }
    }


}
