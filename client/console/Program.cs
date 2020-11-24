using System;
using Newtonsoft.Json;
using System.Net;

namespace console
{
    class Program
    {

        public class Message
        {
            public string username;
            public string message;
        }
        public class Messages
        {
            public Message[] messages;
        }
        static void Main(string[] args)
        {
            System.Console.WriteLine("Client starting");

            WebClient wc = new WebClient();
            string json = wc.DownloadString("http://localhost:3000/chat/get");

            Messages readMessages = JsonConvert.DeserializeObject<Messages>(json);
            for (int i = 0; i < readMessages.messages.Length; i++)
            {
                System.Console.WriteLine(readMessages.messages[i].username);
                System.Console.WriteLine(readMessages.messages[i].message);
            }
        }
    }
}
