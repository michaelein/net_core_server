using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApplication1
{
    public class Startup
    {
        public class SingletonThreadSafe
        {
            private static SingletonThreadSafe _instance;
            private static readonly object _lock = new object();
            public string[] readText;
            public Dictionary<string, List<string>> dictionary;

          
            private SingletonThreadSafe()
            {
                string path = @"C:\Users\DESKTOP\Downloads\words_clean.txt";
                readText = File.ReadAllLines(path);
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
        public class SingletonThreadSafe_stat
        {
            private static SingletonThreadSafe_stat _instance;
            private static readonly object _lock = new object();
            public int totalWords ;
            private int _totalRequests =0;
            public int avgProcessingTimeNs =0;
            //private int _doneCounter;
            public int totalRequests { get { return _totalRequests; } }
            public int IncrementtotalRequestsCounter() { return Interlocked.Increment(ref _totalRequests); }
            public int AddReqTime(int timeToAdd) { return Interlocked.Add(ref _totalRequests, timeToAdd); }
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

        public Startup(IConfiguration configuration)
        {
            var Instance_dictionary = SingletonThreadSafe.Instance;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
