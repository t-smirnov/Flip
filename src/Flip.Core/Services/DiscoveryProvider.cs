using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using dotnet_etcd;
using Etcdserverpb;
using Flip.Core.Models;
using Google.Protobuf;
using Newtonsoft.Json;

namespace Flip.Core.Services
{
    public interface IDiscoveryProvider
    {
        Task<List<Address>> GetClusterMembers();

        Task<bool> AddMember(Address address);

        Task<bool> RemoveMember(Address address);
        Task<bool> Status();
    }

    public class EtcdDiscoveryService : IDiscoveryProvider
    {
        private EtcdClient _client;
        private const string SEEDS = "seeds";

        public EtcdDiscoveryService(EtcdClient client)
        {
            _client = client;
        }


        public async Task<List<Address>> GetClusterMembers()
        {
            var response = await _client.GetRangeAsync(SEEDS);

            if (response.Count == 0)
                return new List<Address>();
            
            var value = response.Kvs.Select(kv => kv.Value).First();

            var str = Encoding.UTF8.GetString(value.ToByteArray());
            var members = JsonConvert.DeserializeObject<List<Address>>(str);
            return members;
        }

        public async Task<bool> RemoveAllMembers()
        {
            var members = await GetClusterMembers();

            if (!members.Any()) return true;
            var response = _client.PutAsync(SEEDS, "[]");
            return true;
        }

        public async Task<bool> AddMember(Address address)
        {
            var members = await GetClusterMembers();
            
            members.Add(address);

            var str = JsonConvert.SerializeObject(members);            
            var response = await _client.PutAsync(SEEDS,str);
            return true;
        }

        public async Task<bool> RemoveMember(Address address)
        {
            var members = await GetClusterMembers();

            members.Remove(address);

            var str = JsonConvert.SerializeObject(members);
            var response = await _client.PutAsync(SEEDS, str);
            return true;
        }

        public async Task<bool> Status()
        {
            try
            {
                var request = new StatusRequest();
                var response = await _client.StatusASync(request);
                return response != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            
        }
    }
}