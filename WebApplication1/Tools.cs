﻿using System;
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
        private static SingletonThreadSafe_stat _instance;
        private static readonly object _lock = new object();
        public int totalWords;
        private int _totalRequests = 0;
        private int globalTime = 0;
        public int totalRequests { get { return _totalRequests; } }
        //https://stackoverflow.com/questions/13181740/c-sharp-thread-safe-fastest-counter
        public int IncrementtotalRequestsCounter() { return Interlocked.Increment(ref _totalRequests); }
        public int AddReqTime(int timeToAdd) { return Interlocked.Add(ref globalTime, timeToAdd); }
        public int avgProcessingTimeNs
        {
            get
            {    
                    return globalTime / _totalRequests * 100;
            }   
        }

        private SingletonThreadSafe_stat()
        {
            totalWords = SingletonThreadSafe.Instance.dictionary.Count();
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
    public class SingletonThreadSafe
    {
        private static SingletonThreadSafe _instance;
        private static readonly object _lock = new object();
        public string[] ReadText;
        public Dictionary<string, StringList> dictionary;


        private SingletonThreadSafe()
        {
            var path1= Directory.GetCurrentDirectory();
            path1 += @"/words_clean.txt";
            try
            {
                ReadText = File.ReadAllLines(path1);
            }
            catch
            {
                Console.WriteLine("No file is found-words_clean");
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
        public static SingletonThreadSafe Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SingletonThreadSafe();
                }
                return _instance;

            }
        }
    }
}
