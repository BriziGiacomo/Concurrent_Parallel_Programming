namespace SynchronizationDemo.Domain;

public class SemaphoreExperiment
{
    public void Run()
    {
        //Risorsa condivisa tra thread.
        // nel primo caso solo permesso di entrata, solo un lavorstore alla volta
        //primo parmetro: initialCount: numero di lavoratori che hanno inizialmente accesso
        //secondo parametro: maxCount: numero massimo di thread che possono entrare contemporaneamente
        var stampante = new SemaphoreSlim(1, 1);

        Thread[]lavoratori=new Thread[4];

        //acesso alla rsorsa condivisa
        for(int i=0; i<lavoratori.Length; i++)
        {
            int idLavoratore=i+1;
            lavoratori[i]=new Thread(()=>
            {
                Console.WriteLine($"ID: {idLavoratore} - vuole accedere alla stampante");
                stampante.Wait();

                Console.WriteLine($"--ID: {idLavoratore} - ottiene la risorsa");
                Thread.Sleep(1500);

                Console.WriteLine($"--ID: {idLavoratore} - rilascio della risorsa");  
                stampante.Release();
            });
        }


        foreach(Thread lavoratore in lavoratori)
        {
            lavoratore.Start();
        }

        foreach(Thread lavoratore in lavoratori)
        {
            lavoratore.Join();
        }

        Console.WriteLine("Tutti i lavoratori hanno terminatol'accesso alla stampante");
    }
}
