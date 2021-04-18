using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;

namespace niftyBot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient _client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken stoppingToken)
        {
            _client = new HttpClient();
            return base.StartAsync(stoppingToken);
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _client.Dispose();
            return base.StopAsync(stoppingToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await _client.GetAsync("https://www.google.com/");
                
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Google is up!");
                }
                else
                {
                    _logger.LogInformation("Google is down!");
                }

                await Task.Delay(60*1000, stoppingToken);
            }
        }
    }
}
