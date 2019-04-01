using System;
using System.Security.Claims;
using Flip.Core;

namespace Flip.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var cluster = new ClusterBuilder()
                .WithConfig("config.hocon")
                .Build();


            Console.ReadKey();
        }
    }
}