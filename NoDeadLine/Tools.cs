using System;
 
public class Tools
{
	public static int coreCount = 0;


    public static string FindToStrEnd(string str, int shift)
    {

        int t = str.IndexOf('\n');
        if (t != -1) return str.Substring(0, t + shift);

        else
            return str;
    }
    public static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }
}
	
