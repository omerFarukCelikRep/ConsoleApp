using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class MyClass
    {
        SemaphoreSlim sync = new SemaphoreSlim(1, 1);
        Dictionary<string, string> messages = new Dictionary<string, string>();

        public async Task<string> WaitForAck(string origUuid)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            while (timer.ElapsedMilliseconds < (10 * 1000))
            {
                if (await this.sync.WaitAsync(TimeSpan.FromSeconds(7)))
                {
                    try
                    {
                        if (this.messages.TryGetValue(origUuid, out var value))
                        {
                            return value;
                        }
                    }
                    finally
                    {
                        this.sync.Release();
                    }
                }

                await Task.Delay(1000);
            }

            return null;
        }

        public async Task PushAck(string origUuid, string value)
        {
            await this.sync.WaitAsync();

            try
            {
                this.messages.Add(origUuid, value);
            }
            finally
            {
                this.sync.Release();
            }
        }


    }
}
