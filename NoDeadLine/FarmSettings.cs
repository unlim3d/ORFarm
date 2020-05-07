using System;
using System.IO;


 

    public class FarmSettings


    {
        public static string Root
    {
        get
        {
            string rootPath= Path.Combine("..", Directory.GetCurrentDirectory());
            rootPath.Replace("\\NoDeadLine", "");
            return rootPath;
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
                return  Directory.GetDirectories(Root, "Site")[0];
            }
        }
        public static string BaseDesignElements
        {
            get
            {
                return Directory.GetDirectories(Root, "BaseDesignElements")[0];
            }
        }
    }
 
