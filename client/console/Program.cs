using System;
using Newtonsoft.Json;
using System.Net;

namespace console
{
    class Program
    {

        public class message
        {
            public string username;
            public string userMessage;
        }
        public class Messages
        {
            public Messages[] messages;
        }
        static void Main(string[] args)
        {
            WebClient wc = new WebClient();
            string json = wc.DownloadString("http://10.254.144.84:3000/chat/get");

            Messages allMessages = JsonConvert.DeserializeObject<Messages>(json);

            for (int i = 0; i < allMessages.messages.Length; i++)
                System.Console.WriteLine("Data" + allMessages.messages[i]);
            {
                
            }
        }
    }
}
