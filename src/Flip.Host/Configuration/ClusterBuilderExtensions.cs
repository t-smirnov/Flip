namespace Flip.Host.Configuration
{
    public static class ClusterBuilderExtensions
    {
        public static ClusterBuilder WithEtcd(this ClusterBuilder builder, string host)
        {
            return builder;
        }
    }
}