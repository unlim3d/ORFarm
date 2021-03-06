﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

public class Job
{
	public Job (string Rpath)
	{
		RenderPath = Rpath;
	}
	public int Id;
	public string RenderPath;
	public string[] ExistingFiles;
	public int[] FramesPreviewsOnServer;	 
	
	public string Renderer;
	public  int LastMovFramesCounter=0;
	public  int MinimumFrameRendered=999999999;
	public  int MaximumFrameRendered=-1111111;
	public string PreviewPath;
	public string JsonPath;
	 
	public string CollectPath = "";
	public string ServerPreviewMovFilePath;
	private string _RenderNameMask;
	public string RenderNameMask
	{
		set
		{
			PreviewPath = FarmSettings.SitePath + Id.ToString()+"//";
			ServerPreviewMovFilePath = FarmSettings.SitePath + Id.ToString()+"_"+ value + ".mov";
			_RenderNameMask =value;
			JsonPath = FarmSettings.SitePath + Id.ToString() + "_" + value + ".json";
			Program.Jobs.Add(this);
			Console.WriteLine("\nВсего Джоб Обнаружено: " + Program.Jobs.Count);
			Console.WriteLine("Новая джоба :" + _RenderNameMask);

		}
		get
		{
			return _RenderNameMask;
		}

	}

	public static void CheckJobName(string RenderFile)
	{



		if (File.Exists(RenderFile))
		{
			string SearchDirectory = RenderTask.GetDirectoryPathFromFile(RenderFile);
			Tools.ClearCurrentConsoleLine();
			Console.Write("Попытка найти новую джобу: " + SearchDirectory);

			if (Program.Jobs.Find(x => x.RenderPath == SearchDirectory) == null)
			{


				Job Joba = new Job(RenderTask.GetDirectoryPathFromFile(RenderFile));

				string str = Path.GetFileNameWithoutExtension(RenderFile);
				str = str.Substring(0, str.Length - 4);
				Joba.RenderNameMask = str;

				Program.Jobs.Add(Joba);


				Joba.ExistingFiles = Program.SearchFile(Joba.RenderPath, "*" + Joba.RenderNameMask + "*");

				Joba.Id = Program.Jobs.Count;
				SaveJobJson(Joba);
				TryParseOtherFrames(Joba);
			}
		}

	}




 static void TryParseOtherFrames(Job joba)
	{
			int z = 0;
			Console.WriteLine("\n Проверяем джобу номер: " + joba.Id.ToString() + " файлов в папке:  " + joba.ExistingFiles.Length+ ":   ");
			for (int j = 0; j < joba.ExistingFiles.Length; j++)
			{
				if (File.Exists(RenderTask.GetServerPreviewFileNameByOriginalFileName(joba.ExistingFiles[j])))
				{
				Console.Write("*");
				}
				else
				{
				 
				Thread newThread = new Thread(Program.RunOperation);
					Console.WriteLine("Запускаем поток: "+z++);

					Program.GenerateFromToFFmpegJpg(joba.ExistingFiles[j]);


				newThread.Start(Program.GenerateFromToFFmpegJpg(joba.ExistingFiles[j]));
				}
			
			 
			}
		CheckSequence(joba);
			 
				 
		  
	}

 
	 
	  static string GenerateMov(Job job)
	{
		string offset = " -start_number " + job.MinimumFrameRendered;


		// -vf scale = 320:-1 "- vf scale = 320:-1, "+ + GammaCorretion
		string tmp = offset + " -i  " + job.RenderNameMask+"%04d" + "-y " + job.ServerPreviewMovFilePath;
		Console.WriteLine("\nFFMPEG:  " + tmp + "\n");
		_ = Program.RunFFMpeg(tmp);
		return tmp;
	}
	 
	  static void CheckSequence(Job job)
	{
		int SequenceCounter = 0;
		foreach (var item in job.ExistingFiles)
		{
			int framenumber = RenderTask.GetFrameNumberFromFileName(item);
			if (job.MaximumFrameRendered < framenumber) job.MaximumFrameRendered = framenumber;
			if (job.MinimumFrameRendered > framenumber) job.MinimumFrameRendered = framenumber;
		}
		
		string Frame = "";
		for (int j = job.MinimumFrameRendered; j <=job.MaximumFrameRendered; j++)
		{
			Frame = j.ToString();
			if (Frame.Length == 1) Frame = "000" + Frame;
			if (Frame.Length == 2) Frame = "00" + Frame;
			if (Frame.Length == 3) Frame = "0" + Frame;
			Frame = RenderTask.GetServerPreviewFileNameByOriginalFileName( job.RenderNameMask + Frame + ".jpg");
			if (File.Exists(Frame)) SequenceCounter++;
			else
			{

				if (SequenceCounter > 1)
				{
					if (job.LastMovFramesCounter != j)
					{
						Console.WriteLine("\nЕбушки воробушки, охуенный мов рендерим: " + job.MinimumFrameRendered.ToString() + "-" + job.MaximumFrameRendered.ToString());

						_ = GenerateMovFile(job, Frame, job.MinimumFrameRendered, j);
						job.LastMovFramesCounter = j;
					}
					else
					{
						Console.WriteLine("\nПропускаем MOV, так как число файлов не изменилось\n");
					}
				}

				break;
			}
			if (j == job.MaximumFrameRendered)
				_ = GenerateMovFile(job, Frame, job.MinimumFrameRendered, j);

		}

	}
	
	 static void SaveJobJson(Job job)
	{
		string output = JsonConvert.SerializeObject(job);
		File.WriteAllText(Path.Combine(FarmSettings.JobsDirectory, job.RenderNameMask + ".json"), output);
		Console.ForegroundColor = ConsoleColor.DarkGreen;
		Console.WriteLine("\nЗаписываем JsonJob: " + job.RenderNameMask);
		Console.ForegroundColor = ConsoleColor.White;
	}
	 

	  static string GenerateMovFile(Job job,string path,int startFrame,int CountOfFrames)
	{
		string filemask = Path.GetFileNameWithoutExtension(path);
		filemask = filemask.Substring(0, filemask.Length - 4);
		string output = RenderTask.GetServerPreviewFileNameByOriginalFileName( filemask);
		output = (output.Substring(0, output.Length - 3))+".mov ";
		path = path.Substring(0, output.Length -5) + "%04d.jpg";
		path = " -i " + path + " " + output;
		string offset = " -start_number " + job.MinimumFrameRendered;


		// -vf scale = 320:-1 "- vf scale = 320:-1, "+ + GammaCorretion
		string tmp = offset + path   + " -y ";
		Program.RunFFMpeg(tmp);

		


		return null;
	}

	public static string FindVrayRGBColorRenderMask(string path)
	{

		string[] strs = Directory.GetDirectories(path);
		List<string> checkedstr= new List<string>();
		for (int i = 0; i < strs.Length; i++)
		{
			if (!strs[i].Contains("System Volume Information") && (!strs[i].Contains("RECYCLE.BIN")) && (!strs[i].Contains("web_server"))) 
				checkedstr.Add(strs[i]);
			 
			 
			 
		 
		}

		List<string> UniquePaths = new List<string>();
		for (int i = 0; i < checkedstr.Count; i++)
		{
			string[] str = Directory.GetFiles(checkedstr[i], "*RGB_color.*", SearchOption.AllDirectories);

			if(str.Length!=0)
			for (int j = 0; j < str.Length; j++)
			{
				string temp = str[j].Substring(0, str[j].Length - 8);
				if (!UniquePaths.Contains(temp))
				{
					UniquePaths.Add(temp);
					CheckJobName(str[j]);
				}
			}
		}
	
		
		 


		
		
		 
	 

		return null;
	}
	  static string DeleteAllSybmols(string str)
	{
		string temps="";
		for (int i = 0; i < str.Length; i++)
		{
			if ((str.Substring(i, 1) == "0")) temps += str[i];
			if ((str.Substring(i, 1) == "1")) temps += str[i];
			if ((str.Substring(i, 1) == "2")) temps += str[i];
			if ((str.Substring(i, 1) == "3")) temps += str[i];
			if ((str.Substring(i, 1) == "4")) temps += str[i];
			if ((str.Substring(i, 1) == "5")) temps += str[i];
			if ((str.Substring(i, 1) == "6")) temps += str[i];
			if ((str.Substring(i, 1) == "7")) temps += str[i];
			if ((str.Substring(i, 1) == "8")) temps += str[i];
			if ((str.Substring(i, 1) == "9")) temps += str[i];
		}
 
		
		return temps;
	}
	
	  static int GetJobIdByFileName(string rendername)
	{
		if(Program.Jobs!=null)
		for (int i = 0; i < Program.Jobs.Count; i++)
		{
			if (Program.Jobs[i].RenderPath == rendername) return i;
		}
		return -1;
	}
}
