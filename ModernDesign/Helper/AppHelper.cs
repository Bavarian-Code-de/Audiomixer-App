using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSApp.Helper
{
    public class AppHelper
    {
        public List<AppProcessIdHelper> getAllInstalledApps(List<string> allApps, List<AppProcessIdHelper> _apps)
        {
            Process[] processes = Process.GetProcesses();
            allApps.Clear();
            foreach (Process p in processes)
            {
                if (!String.IsNullOrEmpty(p.MainWindowTitle))
                {

                    //applist.Items.Add(p.ProcessName);
                    string fileDescription = FileVersionInfo.GetVersionInfo(p.MainModule.FileName).FileDescription;
                    _apps.Add(new AppProcessIdHelper { pid = p.Id, appname = fileDescription });
                    if (fileDescription != "")
                        allApps.Add(fileDescription);
                }
            }
            return _apps;
            //    string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            //    using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            //    {
            //        foreach (string subkey_name in key.GetSubKeyNames())
            //        {
            //            using (RegistryKey subkey = key.OpenSubKey(subkey_name))
            //            {
            //                try
            //                {
            //                    if (subkey.GetValue("DisplayName") != null)
            //                    {
            //                        if (!allApps.Contains(subkey.GetValue("DisplayName").ToString()))
            //                        {
            //                            string appname = subkey.GetValue("DisplayName").ToString();
            //                            if (appname.Contains("Visual") || appname.Contains(".NET") || appname.Contains("vs_") || appname.Contains("SDK") || appname.Contains("Runtime") || appname.Contains(".Net") || appname.Contains("Update")) { }
            //                            else
            //                            {
            //                                allApps.Add(appname);
            //                            }
            //                        }

            //                    }
            //                }
            //                catch (NullReferenceException e) { }
            //            }
            //        }
            //    }
            //    string registry_key2 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            //    using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key2))
            //    {
            //        if (key != null)
            //        {
            //            foreach (string subkey_name in key.GetSubKeyNames())
            //            {
            //                using (RegistryKey subkey = key.OpenSubKey(subkey_name))
            //                {
            //                    try
            //                    {
            //                        if (subkey.GetValue("DisplayName") != null)
            //                        {
            //                            if (!allApps.Contains(subkey.GetValue("DisplayName").ToString()))
            //                            {
            //                                string appname = subkey.GetValue("DisplayName").ToString();
            //                                if (appname.Contains("Visual") || appname.Contains(".NET") || appname.Contains("vs_") || appname.Contains("SDK") || appname.Contains("Runtime") || appname.Contains(".Net") || appname.Contains("Update")) { }
            //                                else
            //                                {
            //                                    allApps.Add(appname);
            //                                }
            //                            }

            //                        }
            //                    }
            //                    catch (NullReferenceException e) { }
            //                }
            //            }
            //        }
            //    }
            //    string registry_key3 = @"Software\Valve\Steam\Apps";
            //    using (Microsoft.Win32.RegistryKey key = Registry.CurrentUser.OpenSubKey(registry_key3))
            //    {
            //        foreach (string subkey_name in key.GetSubKeyNames())
            //        {
            //            string[] test = key.GetSubKeyNames();
            //            try
            //            {
            //                using (RegistryKey subkey = key.OpenSubKey(subkey_name))
            //                {
            //                    if (subkey.GetValue("Name") != null)
            //                    {
            //                        if (!allApps.Contains(subkey.GetValue("Name").ToString()))
            //                        {
            //                            string appname = subkey.GetValue("Name").ToString();
            //                            if (appname.Contains("Visual") || appname.Contains(".NET") || appname.Contains("vs_") || appname.Contains("SDK") || appname.Contains("Runtime") || appname.Contains(".Net") || appname.Contains("Update")) { }
            //                            else
            //                            {
            //                                allApps.Add(appname);
            //                            }
            //                        }

            //                    }
            //                }
            //            }
            //            catch (NullReferenceException e) { }
            //        }
            //    }
            //    string registry_key4 = @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store";
            //    using (Microsoft.Win32.RegistryKey key = Registry.CurrentUser.OpenSubKey(registry_key4))
            //    {
            //        foreach (string subkey_name in key.GetValueNames())
            //        {
            //            string exename = subkey_name.Split('\\').Last();
            //            allApps.Add(exename);
            //            //using (RegistryKey subkey = key.OpenSubKey(subkey_name))
            //            //{
            //            //    try
            //            //    {
            //            //        if (subkey.GetValue("Name") != null)
            //            //        {
            //            //            if (!allApps.Contains(subkey.GetValue("Name").ToString()))
            //            //            {
            //            //                string appname = subkey.GetValue("Name").ToString();
            //            //                if (appname.Contains("Visual") || appname.Contains(".NET") || appname.Contains("vs_") || appname.Contains("SDK") || appname.Contains("Runtime") || appname.Contains(".Net") || appname.Contains("Update")) { }
            //            //                else
            //            //                {
            //            //                    allApps.Add(appname);
            //            //                }
            //            //            }

            //            //        }
            //            //    }
            //            //    catch (NullReferenceException e) { }
            //            //}
            //        }
            //    }
            //    allApps.Sort();
            //    readProcesses(_apps);
            //}
            //public void readProcesses(List<AppProcessIdHelper> _apps)
            //{
            //    Process[] processCollection = Process.GetProcesses();
            //    foreach (Process p in processCollection)
            //    {
            //        if (AudioHelper.GetApplicationVolume(p.Id) != null)
            //        {
            //            _apps.Add(new AppProcessIdHelper { pid = p.Id, appname = p.ProcessName });
            //        }
            //    }
            //}
        }
    }
}
