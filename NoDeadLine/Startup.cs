﻿using System;
using System.Globalization;
using System.IO;
 
 

public class StartUp
{

    public static void GetStarted()
    {
        CopyAddDirectoryImage();
        GetNetworkPoolInfo();

    }

private static void GetNetworkPoolInfo()
    {
        
    }

private static void CopyAddDirectoryImage()
    {

        if (File.Exists(Path.Combine(FarmSettings.BaseDesignElements, "Orfarm_Add_Directory.jpg")))
        {
            
            File.Copy(Path.Combine(FarmSettings.BaseDesignElements, "Orfarm_Add_Directory.jpg"), (Path.Combine(FarmSettings.SitePath, "Add_Directory0000.jpg")),true);
        }   
        else
        {
            Console.WriteLine("Нет файла Orfarm_Add_Directory.jpg, Создаем пустышку");
            File.Create(Path.Combine(FarmSettings.SitePath, "Add_Directory0000.jpg") );
        }
      
    }	 
}

