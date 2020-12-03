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
            public string messageText;
            public long time;
        }
        public class Messages
        {
            public Message[] messages;
        }
        // Only used for testing/example
        public class UserMessage {
            public string username;
            public string messageText;
        }
        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting Client");

            WebClient cli = new WebClient();

            System.Console.Write("Username: ");
            string user = System.Console.ReadLine();

            System.Console.WriteLine("");
            System.Console.WriteLine("Get old messages");
            System.Console.WriteLine("");

            string oldMessagesJson = cli.DownloadString("http://localhost:3000/chat/getFirst");
            Messages oldMessages = JsonConvert.DeserializeObject<Messages>(oldMessagesJson);

            for (int i = 0; i < oldMessages.messages.Length; i++)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(oldMessages.messages[i].time);

                System.Console.WriteLine($"{dateTimeOffset: oldMessages.messages[i].username}: {oldMessages.messages[i].messageText}");
            }

            while (true)
            {
                System.Console.Write("Message: ");
                string messageInput = System.Console.ReadLine();
                Message message = new Message();
                message.messageText = messageInput;
                message.username = user;
                string jsonSend = JsonConvert.SerializeObject(message);
                // Send test message to server
                System.Console.WriteLine("Sending data to server... ");
                cli.Headers[HttpRequestHeader.ContentType] = "application/json";
                try
                {
                cli.UploadString(new Uri("http://localhost:3000/chat/post"), "POST", jsonSend);
                }
                catch (WebException e)
                {
                    
                    throw e;
                }
            }


            /*
            // Working demo of GET and POST data
            // Can send the test string from userMessage
            System.Console.WriteLine("Client starting");
            
            System.Console.WriteLine("Getting data from server...");
            WebClient wc = new WebClient();
            string json = wc.DownloadString("http://localhost:3000/chat/getFirst");

            Messages readMessages = JsonConvert.DeserializeObject<Messages>(json);
            for (int i = 0; i < readMessages.messages.Length; i++)
            {
                System.Console.WriteLine(readMessages.messages[i].username);
                System.Console.WriteLine(readMessages.messages[i].messageText);
            }
            UserMessage userMessage = new UserMessage();
            userMessage.username = "20repsej";
            userMessage.messageText = "This is an example message";
            string jsonSend = JsonConvert.SerializeObject(userMessage);
            // Send test message to server
            System.Console.WriteLine("Sending data to server... ");
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            wc.UploadString(new Uri("http://localhost:3000/chat/post"), "POST", jsonSend);
            */
        }
    }
}
