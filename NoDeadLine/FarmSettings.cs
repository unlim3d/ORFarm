using System;
using System.IO;


 

    public class FarmSettings


    {
        public static string Root
    {
        get
        {
            return Path.Combine("..",Directory.GetCurrentDirectory());
        }
    }
        public static string SitePath
        {
            get
            {
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
 
