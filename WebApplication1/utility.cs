using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Threading;


namespace WebApplication1
{
    using StringList = List<string>;
    public class SingletonThreadSafe_stat
    {
        const int betterRange = 100;
        const int NsFactor = 10000;
        private static SingletonThreadSafe_stat _instance;
        private static readonly object _lock = new object();
        public int totalWords;
        private int _totalRequests = 0;
        private int globalTime = 0;
        public int totalRequests { get { return _totalRequests; } }
        //https://stackoverflow.com/questions/13181740/c-sharp-thread-safe-fastest-counter
        public int IncrementtotalRequestsCounter() { return Interlocked.Increment(ref _totalRequests); }
        public int AddReqTime(int timeToAdd) { return Interlocked.Add(ref globalTime, timeToAdd / betterRange); }
        public int avgProcessingTimeNs
        {
            get
            {    
                    return globalTime / _totalRequests * NsFactor;
            }   
        }
        private SingletonThreadSafe_stat()
        {
            totalWords = SingletonReadFile.Instance.dictionary.Count();
        }
        public static SingletonThreadSafe_stat Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SingletonThreadSafe_stat();
                    }
                    return _instance;
                }
            }
        }
    }
    public class SingletonReadFile
    {
        private static SingletonReadFile _instance;
        private static readonly object _lock = new object();
        public string[] ReadText;
        public Dictionary<string, StringList> dictionary;
        private SingletonReadFile()
        {
            var Path1= Directory.GetCurrentDirectory();
            Path1 += @"/words_clean.txt";
            try
            {
                ReadText = File.ReadAllLines(Path1);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"[Data File Missing] {e}");
                throw new FileNotFoundException(@"[data.txt not in c:\temp directory]", e);
            }
            dictionary = new Dictionary<string, StringList>();
            foreach (string word in ReadText)
            {
                StringList existing;
                string value = word;
                if (!dictionary.TryGetValue(String.Concat(word.OrderBy(c => c)), out existing))
                {
                    existing = new StringList();
                    dictionary[String.Concat(word.OrderBy(c => c))] = existing;
                }
                existing.Add(value);
            }
        }
        public static SingletonReadFile Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SingletonReadFile();
                }
                return _instance;
            }
        }
    }
}
