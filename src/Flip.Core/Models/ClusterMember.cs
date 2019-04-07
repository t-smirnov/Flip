using System;

namespace Flip.Core.Models
{
    public class ClusterMember
    {
        public ClusterMember(string role, Uri address)
        {
            Role = role;
            Address = address;
        }
        public string Role { get; set; }
        
        public Uri Address { get; set; }
    }
}