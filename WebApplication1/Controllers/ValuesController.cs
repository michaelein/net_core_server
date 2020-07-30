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
  
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public Dictionary<string, List<string>> words = Startup.SingletonThreadSafe.Instance.dictionary;

        public class msg
        {
            public msg()
            {
                similar = new List<string>();
            }
            public List<string> similar { get; set; }
        }
        // GET http://localhost:8000/api/values/similar?word=apple
        [HttpGet("similar")]
        public ActionResult<string> Get(string word)
        {
            //var t = Startup.SingletonThreadSafe_stat.Instance;
            Startup.SingletonThreadSafe_stat.Instance.IncrementDoneCounter();
            //t.totalRequests
            // Interlocked.Increment(ref COUNTER);
            msg msgToSend = new msg(); 
            List<string> existing;
            string word_tmp= word;
            if (!words.TryGetValue(String.Concat(word.OrderBy(c => c)), out existing))
            {
                msgToSend.similar.Add("-1");
            }
            else
            {
                msgToSend.similar = new List<string>(existing);
                //p.similar=existing;
                if (msgToSend.similar.Contains(word_tmp))
                {
                    msgToSend.similar.Remove(word_tmp);
                }
            }        
            return JsonConvert.SerializeObject(msgToSend);
        }

        public class msg_stats
        {
            /*public msg_stats()
            {
                similar = new List<string>();
            }*/
            public int totalWords { get; set; }
            public int totalRequests { get; set; }
            public int avgProcessingTimeNs { get; set; }
        }
        // GET http://localhost:8000/api/values/stats
        [HttpGet("stats")]
        public ActionResult<string> Get()
        {
            var stat = Startup.SingletonThreadSafe_stat.Instance;

            msg_stats msgToSend = new msg_stats();
            msgToSend.totalRequests=stat.totalRequests;
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
