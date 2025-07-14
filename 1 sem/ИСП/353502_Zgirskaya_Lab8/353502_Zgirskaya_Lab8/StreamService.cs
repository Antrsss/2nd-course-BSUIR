using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _353502_Zgirskaya_Lab8
{
    internal class StreamService<T>
    {
        private readonly SemaphoreSlim _writeSemaphore = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _copySemaphore = new SemaphoreSlim(0, 1);

        public async Task WriteToStreamAsync(Stream stream, IEnumerable<T> data, IProgress<string> progress)
        {
            progress?.Report($"Enter to Writing to stream started in thread {Thread.CurrentThread.ManagedThreadId}");
            await _writeSemaphore.WaitAsync();
            try
            {
                progress?.Report($"Writing to stream started in thread {Thread.CurrentThread.ManagedThreadId}");

                using (var writer = new StreamWriter(stream, leaveOpen: true))
                {
                    foreach (var item in data)
                    {
                        Thread.Sleep(500);
                        //await Task.Delay(500);
                        string jsonData = JsonConvert.SerializeObject(item);
                        await writer.WriteLineAsync(jsonData);
                        progress?.Report($"Element written to stream {Thread.CurrentThread.ManagedThreadId}");
                    }
                }

                progress?.Report($"Writing to stream ended in thread {Thread.CurrentThread.ManagedThreadId}");
            }
            finally
            {
                _copySemaphore.Release();
                _writeSemaphore.Release();
            }
        }

        public async Task CopyFromStreamAsync(Stream stream, string filename, IProgress<string> progress)
        {
            progress?.Report($"Enter to Copying from stream started in thread {Thread.CurrentThread.ManagedThreadId}");
            await _copySemaphore.WaitAsync();
            try
            {
                progress?.Report($"Copying from stream started in thread {Thread.CurrentThread.ManagedThreadId}");

                using (var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                progress?.Report($"Copying from stream ended in thread {Thread.CurrentThread.ManagedThreadId}");
            }
            finally
            {
                _copySemaphore.Release();
            }
        }

        public async Task<int> GetStatisticsAsync(string filename, Func<T, bool> filter)
        {
            int count = 0;

            using (var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fileStream))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    T item = JsonConvert.DeserializeObject<T>(line);

                    if (item != null && filter.Invoke(item))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
