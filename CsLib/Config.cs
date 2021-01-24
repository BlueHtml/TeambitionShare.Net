using System;
using System.IO;
using System.Linq;

namespace CsLib
{
    public static class Config
    {
        public static bool IsConfigured => _phpCodes?.Length > 1;

        static string[] _phpCodes;
        static readonly string _envPath = Environment.GetEnvironmentVariable("CONFIG_PATH");
        static readonly string _path = string.IsNullOrWhiteSpace(_envPath) ? Path.Combine(AppContext.BaseDirectory, "config") : _envPath;

        static Config()
        {
            Directory.CreateDirectory(_path);
            string sys = "sys.cfg.php";
            if (!File.Exists(Path.Combine(_path, sys)))
            {
                SaveConfig(sys, @"<?php
return [
    'debug' => false, //调试模式
];");
            }
        }

        public static string[] GetPhpCodes()
        {
            if (!IsConfigured)
            {
                LoadConfig();
            }
            return _phpCodes;
        }

        public static bool SaveConfig(string fileName, string phpCode)
        {
            File.WriteAllText(Path.Combine(_path, fileName), phpCode);
            LoadConfig();
            return true;
        }

        static void LoadConfig()
        {
            _phpCodes = Directory.GetFiles(_path, "*.php")
                .Select(p =>
                {
                    string php = File.ReadAllText(p);
                    if (php.StartsWith("<?php", StringComparison.OrdinalIgnoreCase))
                    {
                        return "#" + php;
                    }
                    return php;
                })
                .ToArray();
        }
    }
}
