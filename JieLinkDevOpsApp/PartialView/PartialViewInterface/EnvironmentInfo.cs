﻿using PartialViewInterface.Models;
using PartialViewInterface.Utils;
using Quartz;
using Quartz.Impl;
using System.Configuration;

namespace PartialViewInterface
{
    public static class EnvironmentInfo
    {
        static EnvironmentInfo()
        {
            if (DbConnEntity == null)
            {
                DbConnEntity = new DbConnEntity();
            }

            ConfigHelper.AddAppConfig("DataArchiveJob", "0 0 0 * * ?");
            ConfigHelper.AddAppConfig("ReportProjectInfoJob", "0 0 1 * * ?");
            ConfigHelper.AddAppConfig("AutoArchive", "0");
            ConfigHelper.AddAppConfig("AutoArchiveMonth", "3");
            ConfigHelper.AddAppConfig("AutoStartCorectString", "{\"AutoStartFlag\":\"false\",\"LoopTime\":\"30\"}");
            ConfigHelper.AddAppConfig("AutoStartSyncString", "{\"autoStartFlag\":false,\"loopTime\":5,\"day\":1,\"limit\":100,\"versionCheck\":false}");

        }

        public static string ProjectNo { get; set; }

        public static string RemoteAccount { get; set; }

        public static string RemotePassword { get; set; }

        public static string ContactName { get; set; }

        public static string ContactPhone { get; set; }

        /// <summary>
        /// 服务端URL
        /// </summary>
        public static string ServerUrl = GetValue("ServerUrl", "");

        /// <summary>
        /// 当前版本
        /// </summary>
        public static string CurrentVersion { get; set; }

        /// <summary>
        /// 数据库备份路径
        /// </summary>
        public static string TaskBackUpPath { get; set; }

        /// <summary>
        /// 中心数据库连接对象
        /// </summary>
        public static DbConnEntity DbConnEntity = JsonHelper.DeserializeObject<DbConnEntity>(GetValue("ConnectionString", ""));


        public static string ConnectionString
        {
            get
            {
                if (DbConnEntity == null) return "";
                return $"Data Source={DbConnEntity.Ip};port={DbConnEntity.Port};User ID={DbConnEntity.UserName};Password={DbConnEntity.Password};Initial Catalog={DbConnEntity.DbName};Pooling=true;";
            }
        }

        /// <summary>
        /// 启动软件时是否启动矫正车位数线程对象
        /// </summary>
        public static AutoStartCorectEntity AutoStartCorectEntity = JsonHelper.DeserializeObject<AutoStartCorectEntity>(GetValue("AutoStartCorectString", "{\"AutoStartFlag\":\"false\",\"LoopTime\":\"30\"}"));

        /// <summary>
        /// 启动软件时是否启动同步线程对象
        /// </summary>
        public static AutoStartSyncEntity AutoStartSyncEntity = JsonHelper.DeserializeObject<AutoStartSyncEntity>(GetValue("AutoStartSyncString", "{\"autoStartFlag\":false,\"loopTime\":5,\"day\":1,\"limit\":100,\"versionCheck\":false}"));

        /// <summary>
        /// 启用自动归档
        /// </summary>
        public static bool IsAutoArchive = GetValue("AutoArchive", "0") == "1";

        /// <summary>
        /// 自动归档的月份
        /// </summary>
        public static int AutoArchiveMonth = int.Parse(GetValue("AutoArchiveMonth", "3"));

        /// <summary>
        /// 定时任务全局实例
        /// </summary>
        public static IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public static string GetValue(string key, string value = "")
        {
            return ConfigurationManager.AppSettings[key] ?? value;
        }
    }
}
