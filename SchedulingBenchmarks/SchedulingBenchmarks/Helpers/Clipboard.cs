using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SchedulingBenchmarks
{
    public static class Clipboard
    {
        private static readonly OSPlatform _OSPlatform;

        static Clipboard()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) _OSPlatform = OSPlatform.Windows;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) _OSPlatform = OSPlatform.OSX;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) _OSPlatform = OSPlatform.Linux;
        }

        public static void Copy(string val)
        {
            if (_OSPlatform == OSPlatform.Windows)
            {
                val = val.Replace(Environment.NewLine, $" & echo ");
                Bat($"(echo {val}) | clip");
            }

            if (_OSPlatform == OSPlatform.OSX)
            {
                Bash($"echo \"{val}\" | pbcopy");
            }
        }

        private static string Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            string result = Run("/bin/bash", $"-c \"{escapedArgs}\"");
            return result;
        }

        private static string Bat(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            string result = Run("cmd.exe", $"/c \"{escapedArgs}\"");
            return result;
        }

        private static string Run(string filename, string arguments)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                }
            };

            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
