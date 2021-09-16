using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundWorkerQueue.Models
{
    public class LongRunningService : BackgroundService
    {
        private readonly BackgroundWorkerQueues queue;

        public LongRunningService(BackgroundWorkerQueues queue)
        {
            this.queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await queue.DequeueAsync(stoppingToken);

                await workItem(stoppingToken);
            }
        }
    }
}
