using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryIndexator
{
    public class FileSystemIndexer
    {
        public static int cFiles = 0;
        public static int cFolders = 0;
        private readonly static object _locker = new object();
        private readonly static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(3, 3);
        public static ConcurrentDictionary<string, ICollection<string>> Index { get; set; } =
            new ConcurrentDictionary<string, ICollection<string>>(10, 100_000);
        public static void IndexDirectory(DirectoryInfo directory)
        {
            _semaphoreSlim.Wait();
            try
            {
                var files = directory.GetFiles();
                cFolders++;

                foreach (var file in files)
                {
                    try
                    {
                        if (!Index.ContainsKey(file.Name))
                        {
                            lock (_locker)
                            {
                                
                                Index[file.Name] = new List<string>();
                                Index[file.Name].Add(file.FullName);
                            }
                        }
                        else
                        {
                            lock (_locker)
                            {
                                
                                Index[file.Name].Add(file.FullName);
                            }
                        }
                        cFiles++;
                    }
                    catch (Exception ex)
                    {

                    }
                }


                var subDirectories = directory.GetDirectories();
                if (subDirectories.Any())
                {
                    foreach (var dir in subDirectories)
                    {
                        try
                        {
                            ThreadPool.QueueUserWorkItem(work => IndexDirectory(dir));
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _semaphoreSlim.Release();
            }                
        }
    }
}
