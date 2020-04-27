using System;
using System.IO;
using System.Security.Permissions;
using System.Threading;

class Threads
{
   public static void TestThreads()
    {
        AutoResetEvent mainEvent = new AutoResetEvent(false);
        int workerThreads;
        int portThreads;

        ThreadPool.GetMaxThreads(out workerThreads, out portThreads);
        Console.WriteLine("\nMaximum worker threads: \t{0}" +
            "\nMaximum completion port threads: {1}",
            workerThreads, portThreads);

        ThreadPool.GetAvailableThreads(out workerThreads,
            out portThreads);
        Console.WriteLine("\nAvailable worker threads: \t{0}" +
            "\nAvailable completion port threads: {1}\n",
            workerThreads, portThreads);

        _ = ThreadPool.QueueUserWorkItem(new
            WaitCallback(ThreadPoolTest.WorkItemMethod), mainEvent);

        // Since ThreadPool threads are background threads, 
        // wait for the work item to signal before ending Main.
        _ = mainEvent.WaitOne(5000, false);
    }
}

class ThreadPoolTest
{
    // Maintains state information to be passed to EndWriteCallback.
    // This information allows the callback to end the asynchronous
    // write operation and signal when it is finished.
    class State
    {
        public FileStream fStream;
        public AutoResetEvent autoEvent;

        public State(FileStream fStream, AutoResetEvent autoEvent)
        {
            this.fStream = fStream;
            this.autoEvent = autoEvent;
        }
    }

    ThreadPoolTest() { }

    public static void WorkItemMethod(object mainEvent)
    {
        Console.WriteLine("\nStarting WorkItem.\n");
        AutoResetEvent autoEvent = new AutoResetEvent(false);

        // Create some data.
        const int ArraySize = 10000;
        const int BufferSize = 1000;
        byte[] byteArray = new Byte[ArraySize];
        new Random().NextBytes(byteArray);

        string p= System.IO.Directory.GetCurrentDirectory()+ "Test1.dat";
        FileStream fileWriter1 =
            new FileStream(p, FileMode.Create,
            FileAccess.ReadWrite, FileShare.ReadWrite,
            BufferSize, true);
        FileStream fileWriter2 =
            new FileStream(p.Insert(p.Length-4,"_"), FileMode.Create,
            FileAccess.ReadWrite, FileShare.ReadWrite,
            BufferSize, true);
        State stateInfo1 = new State(fileWriter1, autoEvent);
        State stateInfo2 = new State(fileWriter2, autoEvent);

        // Asynchronously write to the files.
        _ = fileWriter1.BeginWrite(byteArray, 0, byteArray.Length,
            new AsyncCallback(EndWriteCallback), stateInfo1);
        _ = fileWriter2.BeginWrite(byteArray, 0, byteArray.Length,
            new AsyncCallback(EndWriteCallback), stateInfo2);

        // Wait for the callbacks to signal.
        _ = autoEvent.WaitOne();
        _ = autoEvent.WaitOne();

        fileWriter1.Close();
        fileWriter2.Close();
        Console.WriteLine("\nEnding WorkItem.\n");

        // Signal Main that the work item is finished.
        _ = ((AutoResetEvent)mainEvent).Set();
    }

    static void EndWriteCallback(IAsyncResult asyncResult)
    {
        Console.WriteLine("Starting EndWriteCallback.");

        State stateInfo = (State)asyncResult.AsyncState;
        int workerThreads;
        int portThreads;
        try
        {
            ThreadPool.GetAvailableThreads(out workerThreads,
                out portThreads);
            Console.WriteLine("\nAvailable worker threads: \t{0}" +
                "\nAvailable completion port threads: {1}\n",
                workerThreads, portThreads);

            stateInfo.fStream.EndWrite(asyncResult);

            // Sleep so the other thread has a chance to run
            // before the current thread ends.
            Thread.Sleep(1500);
        }
        finally
        {
            // Signal that the current thread is finished.
            _ = stateInfo.autoEvent.Set();
            Console.WriteLine("Ending EndWriteCallback.");
        }
    }
}