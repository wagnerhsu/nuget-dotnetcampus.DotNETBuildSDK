﻿using System;
using System.IO;
using dotnetCampus.DotNETBuild.Context;

namespace dotnetCampus.DotNETBuild.Utils
{
    public static class Log
    {
        public static LogLevel LogLevel { set; get; } = LogLevel.Info;

        public static void Debug(string message)
        {
            if (LogLevel < LogLevel.Debug)
            {
                return;
            }

            Console.WriteLine(message);
            FileLog?.WriteLine($"[Debug] {message}");
        }

        public static void Warning(string message)
        {
            if (LogLevel < LogLevel.Warning)
            {
                return;
            }

            Console.WriteLine(message);
            FileLog?.WriteLine($"[Warning] {message}");
        }

        public static void Info(string message)
        {
            if (LogLevel < LogLevel.Info)
            {
                return;
            }

            Console.WriteLine(message);
            FileLog?.WriteLine($"[Info] {message}");
        }

        public static void Error(string message)
        {
            var foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = foregroundColor;
            FileLog?.WriteLine($"[Error] {message}");
        }

        public static FileLog FileLog { set; get; }

        public static void InitFileLog()
        {
            var appConfigurator = AppConfigurator.GetAppConfigurator();
            var logConfiguration = appConfigurator.Of<LogConfiguration>();

            var folder = Path.GetFullPath(logConfiguration.BuildLogDirectory);
            Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, $"DotNETBuild {DateTime.Now:yyMMddhhmmss}.txt");

            if (!string.IsNullOrEmpty(logConfiguration.BuildLogFile))
            {
                file = logConfiguration.BuildLogFile;
            }
            else
            {
                logConfiguration.BuildLogFile = file;
            }

            var fileLog =
                new FileLog(new FileInfo(file));
            Log.Debug($"日志文件 {fileLog.LogFile.FullName}");

            FileLog = fileLog;
        }
    }
}