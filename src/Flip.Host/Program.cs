using System;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Actor.Internal;
using Akka.Cluster;
using dotnet_etcd;
using Etcdserverpb;
using Flip.Core.Services;
using Flip.Host.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Yaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Cluster = Akka.Cluster.Cluster;

namespace Flip.Host
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddYamlFile("config.yml")
                .Build();

            var host = new HostBuilder()
                .ConfigureServices(services => BuildServices(services, config))
                .Build();
            await host.StartAsync();

        }

        private static void BuildServices(IServiceCollection services, IConfigurationRoot config)
        {
            var host = config.GetSection("etcd:host").Value;
            var port = int.Parse(config.GetSection("etcd:port").Value);
            services.AddSingleton(factory => new EtcdClient(host, port));
            services.AddSingleton<IDiscoveryProvider, EtcdDiscoveryService>();
        }


       

        
    }
}