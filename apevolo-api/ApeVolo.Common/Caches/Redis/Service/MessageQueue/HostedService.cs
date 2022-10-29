using System;
using System.Threading;
using System.Threading.Tasks;
using ApeVolo.Common.Caches.Redis.Models;
using log4net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ApeVolo.Common.Caches.Redis.Service.MessageQueue;

public class HostedService : IHostedService, IDisposable
{
    //private readonly ILogger _logger;
    // private static readonly ILog Logger = LogManager.GetLogger(typeof(HostedService));
    private readonly IServiceProvider _provider;
    private readonly IOptions<RedisOptions> _options;

    public HostedService(IServiceProvider provider, IOptions<RedisOptions> options)
    {
        _provider = provider;
        _options = options;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var init = new InitCore();
        Task.Run(async () => { await init.FindInterfaceTypes(_provider, _options.Value); }, cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}