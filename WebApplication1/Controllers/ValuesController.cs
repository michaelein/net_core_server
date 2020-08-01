using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Threading;

namespace WebApplication1.Controllers
{
    using StringList = List<string>;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public Dictionary<string, StringList> words = SingletonReadFile.Instance.dictionary;

        public class msgSimilar
        {
            public msgSimilar()
            {
                similar = new StringList();
            }
            public StringList similar { get; set; }
        }
        // GET http://localhost:8000/api/values/similar?word=apple
        [HttpGet("similar")]
        public ActionResult<string> Get(string word)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
           
            SingletonThreadSafe_stat.Instance.IncrementtotalRequestsCounter();
            msgSimilar msgToSend = new msgSimilar();
            StringList existing;
            string word_tmp= word;

            if (!words.TryGetValue(String.Concat(word.OrderBy(c => c)), out existing))
            {
                msgToSend.similar.Add("-1");
            }
            else
            {
                msgToSend.similar = new StringList(existing);
                if (msgToSend.similar.Contains(word_tmp))
                {
                    msgToSend.similar.Remove(word_tmp);
                }
            }
            watch.Stop();

            int elapsed =(int)watch.Elapsed.Ticks;
            SingletonThreadSafe_stat.Instance.AddReqTime(elapsed);
            return JsonConvert.SerializeObject(msgToSend);
        }

        public class msg_stats
        {

            public int totalWords { get; set; }
            public int totalRequests { get; set; }
            public int avgProcessingTimeNs { get; set; }
        }
        // GET http://localhost:8000/api/values/stats
        [HttpGet("stats")]
        public ActionResult<string> Get()
        {
            var stat = SingletonThreadSafe_stat.Instance;
            msg_stats msgToSend = new msg_stats();
            msgToSend.totalWords = stat.totalWords;
            msgToSend.totalRequests = stat.totalRequests;
            msgToSend.avgProcessingTimeNs = stat.avgProcessingTimeNs;
            return JsonConvert.SerializeObject(msgToSend);
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
