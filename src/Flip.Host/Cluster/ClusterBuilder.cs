using Akka.Actor;
using Akka.Cluster;
using Akka.Configuration;

namespace Flip.Host
{
    public class ClusterBuilder
    {
        private ActorSystem _system;
        private Cluster _cluster;
        private Config _config;
        private string _name;

        public ClusterBuilder WithConfig(string path)
        {
            _name = "cluster";
            _config = new ConfigLoader().Load(path);
            
            return this;
        }

        public ClusterBuilder WithDiscovery(string etcdHost)
        {
            
            
            return this;
        }

        public Cluster Build()
        {
            _system = ActorSystem.Create(_name, _config);            
            _cluster = Cluster.Get(_system);            
            return _cluster;
        }

        
    }   
}