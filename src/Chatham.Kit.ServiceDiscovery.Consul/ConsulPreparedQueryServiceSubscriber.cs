﻿using Consul;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chatham.Kit.ServiceDiscovery.Abstractions;

namespace Chatham.Kit.ServiceDiscovery.Consul
{
    public class ConsulPreparedQueryServiceSubscriber : IServiceSubscriber
    {
        private readonly IConsulClient _client;
        private readonly string _queryName;

        public ConsulPreparedQueryServiceSubscriber(IConsulClient client, string queryName)
        {
            _client = client;
            _queryName = queryName;
        }

        public async Task<List<Endpoint>> Endpoints(CancellationToken ct = default(CancellationToken))
        {
            var servicesQuery = await
               _client.PreparedQuery.Execute(_queryName, ct)
                   .ConfigureAwait(false);

            return servicesQuery.Response.Nodes.Select(service => service.ToEndpoint()).ToList();
        }

        public void Dispose() {}
    }
}
