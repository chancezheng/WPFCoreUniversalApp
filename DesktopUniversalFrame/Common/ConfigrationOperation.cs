using DesktopUniversalFrame.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DesktopUniversalFrame.Common
{
    /// <summary>
    /// 配置文件读写
    /// </summary>
    public class ConfigrationOperation
    {
        private Configuration ConfigurationObject;

        /// <summary>
        /// 根据路径获取配置文件
        /// </summary>
        /// <param name="configPath"></param>
        public ConfigrationOperation(string configPath)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configPath;
            ConfigurationObject = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        /// <summary>
        /// 根据键值获取配置文件value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetUserConfiguration(string key)
        {
            string value = ConfigurationObject.AppSettings.Settings[key].Value ?? string.Empty;
            return value;
        }

        /// <summary>
        /// 写入配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetUserConfiguration(string key, string value)
        {
            if (!ConfigurationObject.AppSettings.Settings.AllKeys.Contains(key))
                ConfigurationObject.AppSettings.Settings.Add(key, value);
            else
                ConfigurationObject.AppSettings.Settings[key].Value = value;
            ConfigurationObject.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
