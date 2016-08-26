﻿using System;
using Chatham.ServiceDiscovery.Abstractions;
using Consul;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chatham.ServiceDiscovery.Consul
{
    public static class ConsulServiceCollectionExtensions
    {
        public static IServiceCollection AddConsulServiceDiscovery(this IServiceCollection services, ConsulClientConfiguration config = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var consulConfig = new global::Consul.ConsulClientConfiguration();
            if (config?.Address != null)
            {
                consulConfig.Address = config.Address;
            }
            if (config?.Token != null)
            {
                consulConfig.Token = config.Token;
            }

            services.TryAdd(new ServiceDescriptor(typeof(IConsulClient), p => new ConsulClient(consulConfig), ServiceLifetime.Singleton));
            services.TryAdd(new ServiceDescriptor(typeof(IMemoryCache), p => new MemoryCache(new MemoryCacheOptions()), ServiceLifetime.Transient));

            services.TryAddTransient<ICacheClient, CacheClient>();
            services.TryAddSingleton<IServiceSubscriberFactory, ConsulServiceSubscriberFactory>();

            return services;
        }
    }
}
