﻿using System;
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
                System.Console.WriteLine($"{oldMessages.messages[i].username}: {oldMessages.messages[i].message}");
            }

            while (true)
            {
                string newMessage = System.Console.ReadLine();
                send(oldMessagesJson, user);
            }


            void send(string newMessage, string user)
            {

                newMessage = JsonConvert.SerializeObject(newMessage);
                System.Console.WriteLine(newMessage);
                cli.Headers[HttpRequestHeader.ContentType] = "application/json";
                cli.UploadString(new Uri("http://localhost:3000/chat/post"), "POST", newMessage);
            };

            /* Working demo of GET and POST data
            System.Console.WriteLine("Client starting");

            WebClient wc = new WebClient();
            string json = wc.DownloadString("http://localhost:3000/chat/getFirst");

            Messages readMessages = JsonConvert.DeserializeObject<Messages>(json);
            for (int i = 0; i < readMessages.messages.Length; i++)
            {
                System.Console.WriteLine(readMessages.messages[i].username);
                System.Console.WriteLine(readMessages.messages[i].message);
            }
            // Send test message to server
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            wc.UploadString(new Uri("http://localhost:3000/chat/post"), "POST", json);
            */
        }
    }
}
