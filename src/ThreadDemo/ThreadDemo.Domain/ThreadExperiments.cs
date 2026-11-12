using System.Runtime.InteropServices.Marshalling;

namespace ThreadDemo.Domain;

public class ThreadExperiments()
{

    public void RunSequential()
    {
        Work("A");
        Work("B");
    }

    public void RunThreads()
    {
        // lambda to assign a certain work to a thread in  his constructor
        Thread thread1 = new Thread(() => Work("A"));
        Thread thread2 = new Thread(() => Work("B"));

        Console.WriteLine("Starting threads ...");

        thread1.Start();
        thread2.Start();
        
        Console.WriteLine("This line is written by main therad");

        thread1.Join();//Waiting thread to finish
        thread2.Join();

        Console.WriteLine("All finished");

    }

    public void ShowThreadProperties()
    {
        Thread currentThread = Thread.CurrentThread;

        Console.WriteLine($"Thread ID: {currentThread.ManagedThreadId}");
        Console.WriteLine($"Thread Name: {currentThread.Name ?? "N/A"}");
        Console.WriteLine($"Is Background: {currentThread.IsBackground}");
        Console.WriteLine($"Priority: {currentThread.Priority}");
    }

    //Avere una risorsa condivisa condivisa su cui più thread possono agir in quanto possono esserci problemi sulla variabile condivisa
    // 1* test: thread liberi
    // 2* test: thread con lock
    public (int Expected, int Actual) RunCounter(bool useLock, int iterations =20_000)
    {
        int counter = 0;
        object gateLock=new();

        void Increment()
        {
            for(int i=0;i< iterations; i++)
            {
                if (useLock)
                {
                    lock(gateLock)
                    {
                        counter++;
                    }
                }
                else
                {
                   counter++; 
                }
            }
        }
        Thread t1 = new (Increment);
        Thread t2 = new (Increment);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        return (iterations * 2, counter);
    }

    

    // funizone per simulare del lavoro
    private static void Work(string name)
    {
        for (int i = 0; i <= 5; i++)
        {
            Console.WriteLine($"{name}: Thread: {Thread.CurrentThread.ManagedThreadId}, index: {i}");
            Thread.Sleep(100); // Simula un lavoro che richiede tempo
        }
    }


}
