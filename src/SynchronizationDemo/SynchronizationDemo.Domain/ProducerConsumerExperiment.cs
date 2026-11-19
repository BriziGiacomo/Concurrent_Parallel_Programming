public class ProducerConsumerExperiment
{
    private const int BufferSize=4;//grandezza della tavola con i piatti
    private const int NProducers=2;
    private const int NConsumers=6;

    public void Run()
    {
        //struttura dati utile
        Queue<int> buffer = new ();

        var spaziDisponibili = new SemaphoreSlim(BufferSize, BufferSize);
        var elementiDisponibili = new SemaphoreSlim(0, BufferSize);
        var accessoTavola = new SemaphoreSlim(1, 1);//acesso mutualmente esclusivo alla tavola de ciccio con un panino dai 7 cereali; non due , non tre, ma ben 5


        Thread[] producers = new Thread[NProducers];
        Thread[] consumers = new Thread[NConsumers];

        int nextId=0;

        void ConsumeSomeProduct(int id)
        {
            //Finchè il thread non viene fermato o killato(scala-muro, fullbox, finestrella pump head-shot,full-max, + griddy commemorativo)
            while(true)
            {
                elementiDisponibili.Wait();
               
            }
        }

        void ProduceSomeProduct(int id)
        {
            //Finchè il thread non viene fermato o killato(scala-muro, fullbox, finestrella pump head-shot,full-max, + griddy commemorativo)
            while(true)
            {
                int foodItem= Interlocked.Increment(ref nextId);

                spaziDisponibili.Wait();
                try{
                    accessoTavola.Wait();
                    buffer.Enqueue(foodItem);
                    Console.WriteLine($"ProducerId: {id} produced foodItem: {foodItem}, BufferCount: {buffer.Count}");
                    accessoTavola.Release();
                }
                catch
                {
                    spaziDisponibili.Release();
                }

                elementiDisponibili.Release();
                Thread.Sleep(Random.Shared.Next(100, 250));
            }
        }

        for(int i=0; i<NConsumers; i++)
        {
            int id=i+1;
            consumers[i]=new Thread(()=> ConsumeSomeProduct(i));

            consumers[i].Start();
        }

        for(int i=0; i<NProducers; i++)
        {
            int id=i+1;
            producers[i]=new Thread(()=> ProduceSomeProduct(i));

            producers[i].Start();
        }


        foreach (Thread consumatore in producers)
        {
            consumatore.Join();
        }

        foreach (Thread produttore in consumers)
        {
            produttore.Join();
        }
    }   

}       