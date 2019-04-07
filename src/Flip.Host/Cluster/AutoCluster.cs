using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster;
using Flip.Core.Services;
using Flip.Host.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Flip.Host
{
    public class AutoCluster
    {
        private readonly IDiscoveryProvider _discovery;
        private readonly Cluster _cluster;
        

        public AutoCluster(IDiscoveryProvider discovery, ClusterBuilder builder)
        {
            _discovery = discovery;
            
            _cluster = builder.WithConfig("config.hocon").Build();
        }
        
        public async Task<SetupResult> Join()
        {
            var isOk = await _discovery.Status();
            if (!isOk)
                throw new Exception("Discovery service is unavailable");
            
            var members = await _discovery.GetClusterMembers();
            if (members.Any())
            {
                _cluster.JoinSeedNodes(members);
            }
            else
            {
                var self = _cluster.SelfAddress;
                await _discovery.AddMember(self);
                _cluster.JoinSeedNodes(ImmutableList.Create(self));
            }
            
            return new SetupResult();
        }
        
        public async Task<LeaveResult> Leave()
        {
            var self = _cluster.SelfAddress;

            var ok = await _discovery.RemoveMember(self);
            if (ok)
            {
                await _cluster.LeaveAsync(CancellationToken.None);
            }
            
            return new LeaveResult();
        }
        
        
        public ActorSystem System { get; set; }
    }
}