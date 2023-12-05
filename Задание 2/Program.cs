using System;
using System.Threading;

class Supermarket
{
    private static Semaphore _purchase = new Semaphore(0, int.MaxValue);
    private static Semaphore _cashier = new Semaphore(2, 2);
    private static int _revenue = 0;

    static void Main(string[] args)
    {
        for (int i = 1; i <= 10; i++)
        {
            Thread customerThread = new Thread(() =>
            {
                EnterSupermarket();
                Purchase();
                Pay();
                ExitSupermarket();
            });
            customerThread.Start();
        }
    }

    static void EnterSupermarket()
    {
        Console.WriteLine($"Покупатель {Thread.CurrentThread.ManagedThreadId} зашел в супермаркет");
        _purchase.Release();
        Thread.Sleep(1000);
    }

    static void Purchase()
    {
        Console.WriteLine($"Покупатель {Thread.CurrentThread.ManagedThreadId} делает покупку");
        Thread.Sleep(new Random().Next(1000, 3000));
    }

    static void Pay()
    {
        _cashier.WaitOne();
        Console.WriteLine($"Кассир обслуживает покупателя {Thread.CurrentThread.ManagedThreadId}");
        int cost = new Random().Next(10, 100);
        Console.WriteLine($"Покупатель {Thread.CurrentThread.ManagedThreadId} заплатил {cost} рублей");
        Interlocked.Add(ref _revenue, cost);
        Thread.Sleep(1000);
        _cashier.Release();
    }

    static void ExitSupermarket()
    {
        Console.WriteLine($"Покупатель {Thread.CurrentThread.ManagedThreadId} вышел из супермаркета");
        _purchase.WaitOne();
    }
}