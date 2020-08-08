﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Windows;
using NitroxModel.Logger;

namespace NitroxLauncher
{
    public static class AppHelper
    {
        public static readonly string ProgramFileDirectory = Environment.ExpandEnvironmentVariables("%ProgramW6432%");

        private static bool IsAppRunningInAdmin()
        {
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void RestartAsAdmin()
        {
            if (!IsAppRunningInAdmin())
            {
                MessageBoxResult result = MessageBox.Show(
                    "Nitrox launcher should be executed with administrator permissions in order to properly patch Subnautica while in Program Files directory, do you want to restart ?",
                    "Nitrox needs permissions",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.Yes,
                    MessageBoxOptions.DefaultDesktopOnly
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Setting up start info of the new process of the same application
                        // Using operating shell and setting the ProcessStartInfo.Verb to “runas” will let it run as admin
                        ProcessStartInfo processStartInfo = new ProcessStartInfo(Assembly.GetEntryAssembly().CodeBase)
                        {
                            UseShellExecute = true,
                            Verb = "runas"
                        };

                        // Start the application as new process
                        Process.Start(processStartInfo);
                        Environment.Exit(1);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error while trying to instance an admin process of the launcher, aborting");
                    }
                }

                //We might exit the application if the user says no ?
            }
            else
            {
                Log.Info("Can't restart the launcher as admin, we already have the perms");
            }
        }
    }

}
