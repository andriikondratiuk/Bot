using System;
using System.Threading;

namespace SashaGrayBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var telegram = new TelegramBot();
            telegram.Start();
            while (true);           
        }
    }
}
