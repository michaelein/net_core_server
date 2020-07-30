﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace WebApplication1
{
    public class SingletonThreadSafe_stat
    {
        private static SingletonThreadSafe_stat _instance;
        private static readonly object _lock = new object();
        public int totalWords;
        private int _totalRequests = 0;
        //public int avgProcessingTimeNs = 0;
        public int globalTime = 0;


        public int totalRequests { get { return _totalRequests; } }
        public int IncrementtotalRequestsCounter() { return Interlocked.Increment(ref _totalRequests); }
        public int AddReqTime(int timeToAdd) { return Interlocked.Add(ref globalTime, timeToAdd); }
        public int avgProcessingTimeNs
        {
            get
            {    
                    return globalTime / _totalRequests * 100;
            }
            
        }

        //public int AddReqTime_(int timeToAdd) { return Interlocked. Add(ref globalTime, timeToAdd); }

        //Interlocked.Add(ref Program._value, 1);
        //  public int IncrementDoneCounter_() { return Interlocked.(ref _totalRequests); }

        /*public void ThreadSafeMethod(string parameter1)
        {
            totalRequests++;

        }*/
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
        public string[] readText;
        public Dictionary<string, List<string>> dictionary;


        private SingletonThreadSafe()
        {
            var path1= Directory.GetCurrentDirectory();
            path1 += @"\words_clean.txt";
            readText = File.ReadAllLines(path1);
            dictionary = new Dictionary<string, List<string>>();
            foreach (string word in readText)
            {
                List<string> existing;
                string value = word;
                if (!dictionary.TryGetValue(String.Concat(word.OrderBy(c => c)), out existing))
                {
                    existing = new List<string>();
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