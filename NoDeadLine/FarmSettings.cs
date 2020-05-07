using System;
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
        if (Directory.GetFiles(Root, "server.js").Length > 0)
        {
            Root = Root;

        }
        else 
        {
            Root=Root.Substring(0, Root.LastIndexOf("\\"));
            RootPath();
        }
        
    }
    public static string SitePath
    {
        get
        {
            string tempDirectory = Path.Combine(Root, "Site");
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            return Directory.GetDirectories(Root, "Site")[0];
        }
    }

    public static string BaseDesignElements
    {
        get
        {
            string tempDirectory = Directory.GetDirectories(Root, "BaseDesignElements")[0];
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            return Directory.GetDirectories(Root, "BaseDesignElements")[0];
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

            string pathRoot = FarmSettings.Root;
            string path = Path.Combine(pathRoot, "reports");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }

    public static string HardwareInfoCommandLine
    {
        get { return Directory.GetDirectories(FarmSettings.Root, "Resources")[0] + "\\OpenHardwareMonitorReport"; }
    }

    public static string NetworkInfoCommandLine
    {
        get { return Directory.GetDirectories(FarmSettings.Root, "Resources")[0] + "\\NetworkSpeedTest"; }
    }

    public static string FFMPEG
    {
        get { return Directory.GetDirectories(Root, "FFMPEG")[0] + "\\ffmpeg.exe"; }
    }
}
 
