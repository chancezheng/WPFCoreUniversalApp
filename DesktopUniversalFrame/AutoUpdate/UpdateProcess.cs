using DesktopUniversalFrame.ViewModel.MedicalViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace DesktopUniversalFrame.AutoUpdate
{
    /// <summary>
    /// 更新过程信息
    /// </summary>
    public class UpdateProcess
    {
        public bool CheckAppVersion()
        {
            bool isNeedUpdate = false;
            string httpAddress = MedicalReportViewModel.GetBinPath() + "\\AutoUpdate\\updateinfo.xml";
            string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ThreadPool.QueueUserWorkItem(q =>
            {
                var client = new WebClient();
                client.DownloadDataCompleted += (obj, e) =>
                {
                    try
                    {
                        MemoryStream ms = new MemoryStream(e.Result);

                        UpdateInfo updateInfo = new UpdateInfo();
                        XDocument document = XDocument.Load(ms);
                        XElement rootElement = document.Element("UpdateInfo");
                        updateInfo.AppName = rootElement.Element("AppName").Value;
                        updateInfo.AppVersion = rootElement.Element("AppVersion") == null || string.IsNullOrEmpty(rootElement.Element("AppVersion").Value) ? null : new Version(rootElement.Element("AppVersion").Value);
                        updateInfo.RequiredMinVersion = rootElement.Element("RequiredMinVersion") == null || string.IsNullOrEmpty(rootElement.Element("RequiredMinVersion").Value) ? null : new Version(rootElement.Element("RequiredMinVersion").Value);
                        updateInfo.Description = rootElement.Element("Description").Value;
                        updateInfo.MD5 = Guid.NewGuid();

                        ms.Close();
                        isNeedUpdate = StartUpdate(updateInfo);
                        if (isNeedUpdate)
                        {
                            //启动更新程序
                            Process.Start(Directory.GetCurrentDirectory() + "\\AppUpdate\\netcoreapp3.1\\AutoUpdatedApp.exe");

                            foreach (var process in Process.GetProcessesByName(AppExeName))
                            {
                                process.Kill();
                            }

                            isNeedUpdate = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                    }
                };
                client.DownloadDataAsync(new Uri(httpAddress, UriKind.Absolute));
            });

            return isNeedUpdate;
        }

        private bool StartUpdate(UpdateInfo updateInfo)
        {
            if (updateInfo.RequiredMinVersion != null && CurrentVersion < updateInfo.RequiredMinVersion)
                return false; //当前版本比需要的最低版本小，不更新
            if (CurrentVersion >= updateInfo.AppVersion)
                return false; //当前已经是最新版本，不更新

            return true;
        }



        /// <summary>
        /// 获得当前应用软件的版本
        /// </summary>
        private Version CurrentVersion
        {
            get { return new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion); }
        }

        /// <summary>
        /// 应用程序名称
        /// </summary>
        private string AppExeName
        {
            get { return Assembly.GetEntryAssembly().Location.Substring(Assembly.GetEntryAssembly().Location.LastIndexOf(Path.DirectorySeparatorChar) + 1).Replace(".exe", "").Replace(".dll", ""); }
        }

        /// <summary>
        /// 获得当前应用程序的根目录
        /// </summary>
        private string CurrentAppDirectory
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }
    }
}
