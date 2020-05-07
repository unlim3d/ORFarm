﻿using System;
using System.Dynamic;
using System.IO;
using System.Reflection;


public class FarmSettings
{
    private static string root = Directory.GetCurrentDirectory();
    public static string Root
    {
        get { return root;}
        set { root = value; }
    }

    public static void RootPath()
    {
        if (Directory.GetDirectories(Root, "NoDeadLine").Length > 0)
        {
            Root = Root;

        }
        else 
        {
            Root=Root.Substring(0, Root.LastIndexOf("\\"));
            RootPath();
        }
        
    }

    public static string WebServer
    {
        get
        {
            string tempDirectory = Directory.GetDirectories(Root, "web_server")[0];
            if (!Directory.Exists(tempDirectory))
            {
                Console.WriteLine("Kapec");

            }
            return tempDirectory;
        }
    }

    public static string Resources
    {
        get
        {
            string tempDirectory=Path.Combine(SitePath, "Resources");
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
            return tempDirectory;
        }
    }
    public static string SitePath
    {
        get
        {
            string tempDirectory = Path.Combine(WebServer, "Site");
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            return Directory.GetDirectories(WebServer, "Site")[0];
        }
    }

    public static string BaseDesignElements
    {
        get
        {
            string tempDirectory = Path.Combine(Resources, "BaseDesignElements");//Directory.GetDirectories(WebServer, "BaseDesignElements")[0];
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            return Directory.GetDirectories(Resources, "BaseDesignElements")[0];
        }
    }

    public static string DeadLineReportFolderSlaves
    {
        get
        {
            string path = Path.Combine(DeadLineReportFolder, "slaves");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return path;
            }

            return path;
        }
    }

    public static string DeadLineReportFolderWin
    {
        get
        {
            string path = Path.Combine(DeadLineReportFolder, "win");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return path;
            }

            return path;
        }
    }

    public static string DeadLineReportFolder
    {
        get
        {

            string pathRoot = SitePath;
            string path = Path.Combine(pathRoot, "Reports");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }

    public static string HardwareInfoCommandLine
    {
        get
        {
            string tempDirectory = Path.Combine(Resources, "OpenHardwareMonitorReport");
            if(!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
            return tempDirectory;
        }
    }

    public static string NetworkInfoCommandLine
    {
        get
        {
            string tempDirectory = Path.Combine(Resources, "NetworkSpeedTest");
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
            return tempDirectory;
        }
    }

    public static string FFMPEG
    {
        get { return Directory.GetDirectories(Root, "FFMPEG")[0] + "\\ffmpeg.exe"; }
    }
}
 
