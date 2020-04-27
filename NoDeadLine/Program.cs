﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Timers;
using System.Diagnostics;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Buffers;

class Program
{
    
    public const string DeadLineReportFolder =@"c:\DeadlineRepository10\reports\slaves\";
    public static string FFMPEG = @"c:\DeadlineRepository10\Node\FFMPEG\ffmpeg.exe ";
    public static string NodeWWW = @"c:\DeadlineRepository10\Node\Site\";
    public static List<RenderTask> tasks;
    public static float Counts = 0;
    public static int ChangedCountFiles = 0;
    public static List<Report> Reports;
    public static List<string> Operations;
    public static List<Thread> _threads;
    public static List<Job> Jobs;
   static int iteration = 10;
         static void Main(string[] args)
    {
        Jobs = new List<Job>();
        //  _threads = new List<Thread>();
      //      Threads.TestThreads();
        int c = ThreadPool.ThreadCount;
        Console.WriteLine("ThreadsNumber: " + c);
        ThreadPool.SetMaxThreads (3200000,999);
         ThreadPool.SetMinThreads (10000,10000);
        c = ThreadPool.ThreadCount;
        Console.WriteLine("ThreadsNumber: " + c);
        Operations = new List<string>();
        Reports = new List<Report>();
        // DeletePreviousRender();
        ListenerCommand.StartThread();
         tasks = new List<RenderTask>();
        while (true)
        { 
            Report. CreaterReports();
           

             
                
            
         
            while (Operations.Count > 0)
            {
                Thread newThread = new Thread(RunOperation);
                newThread.Start();
                Thread.Sleep(200); 
            }
           
            Console.ForegroundColor = ConsoleColor.Red;
             
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n");
            Console.WriteLine("\nСледующая проверка через: \n");
            for (int i = 0; i < iteration; i++)
            {
                Thread.Sleep(1000);
               Tools.ClearCurrentConsoleLine();
                Console.Write((iteration - i).ToString() + " ");
               
            }
             
            
           
        }  
    }

    public static void RunOperation(object data)
    {
        
          
            RunFFMpeg(data.ToString());
           
    }
   
    static void DeletePreviousRender()
    {
        string[] strs = SearchFile(NodeWWW, "");
        for (int i = 0; i < strs.Length; i++)
        {
            File.Delete(strs[i]);
        }
        Console.WriteLine("Удалено: " + (strs.Length).ToString()+ " Файлов" ); ;
    }
   
   
   
    public static string[] SearchFile(string patch, string pattern)
    {
        /*флаг SearchOption.AllDirectories означает искать во всех вложенных папках*/
        string[] ResultSearch = Directory.GetFiles(patch, pattern, SearchOption.AllDirectories);
        //возвращаем список найденных файлов соответствующих условию поиска 
       
       
            return ResultSearch;
    }
   

    public static void SaveJpegPreview(RenderTask task, string tmp)
    {
         
       
     
        
        
    }
    public static string GenerateFromToFFmpegJpg(string OriginalFilename)
    {
      

        string tmp = " -i  " + OriginalFilename + " -s 640:360 " + "-y " + RenderTask.GetServerPreviewFileNameByOriginalFileName(OriginalFilename);
        Console.WriteLine("\nFFMPEG:  " + tmp + "\n");
        return tmp;
    }
 
    public static string RunFFMpeg(string parametres)
    {
        ChangedCountFiles++;
        ProcessStartInfo oInfo = new ProcessStartInfo(FFMPEG, parametres);
        oInfo.UseShellExecute = false;
        oInfo.CreateNoWindow = true;

        //so we are going to redirect the output and error so that we can parse the return
        oInfo.RedirectStandardOutput = true;
        oInfo.RedirectStandardError = true;

        //Create the output and streamreader to get the output
        string output = null; StreamReader srOutput = null;

        //try the process
        try
        {
            //run the process
            Process proc = System.Diagnostics.Process.Start(oInfo);

            //proc.WaitForExit();

            //get the output
            srOutput = proc.StandardError;

            //now put it in a string
            output = srOutput.ReadToEnd();

            proc.Close();
        }
        catch (Exception)
        {
            output = string.Empty;
            Console.WriteLine("Косяк бляяя:  ");
        }
        finally
        {
            //now, if we succeded, close out the streamreader
            if (srOutput != null)
            {
                srOutput.Close();
                srOutput.Dispose();
            }
        }
        return output;
    }
}
