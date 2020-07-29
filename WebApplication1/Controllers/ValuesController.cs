using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections;

class readFile
{
    public string[] readText;
    public Dictionary<string, List<string>> dictionary_name;//= new Dictionary();

    public readFile()
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
        int t = 0;
    }
}
namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readFile myDictionary;
        public ValuesController()
        {
            myDictionary = new readFile();
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value12", "value2" };
        }

        // GET http://localhost:8000/api/values/similar?word=apple
        [HttpGet("similar")]
        public ActionResult<string> Get(string word)
        {
            msg p = new msg(); 
            List<string> existing;
            string word_tmp= word; 
            if (!myDictionary.dictionary_name.TryGetValue(String.Concat(word.OrderBy(c => c)), out existing))
            {
            }
                p.similar.Add(word);
            p.similar.Add(myDictionary.readText[0]);
            return JsonConvert.SerializeObject(p);
        }
        public class msg
        {
            public msg() {
                similar = new List<string>();
            }
            public List<string> similar { get; set; }
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
