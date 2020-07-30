using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            public Dictionary<string, List<string>> dictionary_name;

            public void init()
            {
                string path = @"C:\Users\DESKTOP\Downloads\words_clean.txt";//C:\Users\DESKTOP\Downloads\words_clean.txt
                readText = File.ReadAllLines(path);
                dictionary_name = new Dictionary<string, List<string>>();
                foreach (string word in readText)
                {
                    List<string> existing;
                    string value = word;
                    if (!dictionary_name.TryGetValue(String.Concat(word.OrderBy(c => c)), out existing))
                    {
                        existing = new List<string>();
                        dictionary_name[String.Concat(word.OrderBy(c => c))] = existing;
                    }
                    existing.Add(value);
                }
            }
            private SingletonThreadSafe() { }
            public int g;
            public static SingletonThreadSafe Instance
            {
                get
                {
                    lock (_lock)
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
  
    public int g;
        public Startup(IConfiguration configuration)
        {
            var m=SingletonThreadSafe.Instance;
            m.init();
            var s = SingletonThreadSafe.Instance;
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
