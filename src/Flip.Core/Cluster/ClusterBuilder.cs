using Akka.Actor;
using Akka.Actor.Internal;
using Akka.Cluster;
using Akka.Configuration;
using Flip.Core.Utils;

namespace Flip.Core
{
    public class ClusterBuilder
    {
        private ActorSystemImpl _system;
        private Akka.Cluster.Cluster _cluster;
        private Config _config;

        public ClusterBuilder()
        {
           
        }

        public ClusterBuilder WithConfig(string path)
        {
            var loader = new ConfigLoader();
            _config = loader.Load(path);
            return this;
        }

        public Akka.Cluster.Cluster Build()
        {
            
            var actorSystem = ActorSystem.Create("flipcluster", _config);
            var cluster = Cluster.Get(actorSystem);
            return cluster;
        }

        
    }   
}