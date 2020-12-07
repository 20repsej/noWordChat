using Terminal.Gui;
using System;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;

namespace noWordChat
{

    class chat
    {

        public List<String> oldMessagesList = new List<String>();
        static void Main()
        {
            Application.Init();
            var top = Application.Top;

            // Creates the top-level window to show
            var win = new Window("noWordChat")
            {
                X = 0,
                Y = 0,

                // Make a full window
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            top.Add(win);

            // Shows old messages
            var oldMessages = new ListView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                AllowsMultipleSelection = false
            };

            // Message input field
            var inputMessage = new TextField("")
            {
                X = 0,
                Y = Pos.AnchorEnd() - 1,
                Width = Dim.Fill() - 12
            };

            // Username input field
            var inputUsername = new TextField("")
            {
                X = Pos.Right(inputMessage),
                Y = Pos.AnchorEnd() - 1,
                Width = Dim.Fill() - 10
            };

            win.Add(

                oldMessages,
                inputMessage,
                inputUsername

            );

            oldMessagesList = getFirstOldMessages();
            oldMessages.SetSource(oldMessagesList);

            Application.Run();
        }
        public List<string> getFirstOldMessages()
        {

            WebClient cli = new WebClient();

            string oldMessagesJson = cli.DownloadString("http://localhost:3000/chat/getFirst");
            Messages oldMessages = JsonConvert.DeserializeObject<Messages>(oldMessagesJson);

            for (int i = 0; i < oldMessages.messages.Length; i++)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(oldMessages.messages[i].time);

                oldMessagesList.Add($"{dateTimeOffset: oldMessages.messages[i].username}: {oldMessages.messages[i].messageText}");
            }


            return oldMessagesList;
        }
        public string getMessagesFromServer()
        {
            WebClient cli = new WebClient();

            string oldMessagesJson = cli.DownloadString("http://localhost:3000/chat/getFirst");
            Messages oldMessages = JsonConvert.DeserializeObject<Messages>(oldMessagesJson);


            return "haha yes";
        }
        public void uploadToServer(string userMessage, string username)
        {
            WebClient cli = new WebClient();

            // Convert before upload
            Message message = new Message();
            message.messageText = userMessage;
            message.username = username;
            string jsonSend = JsonConvert.SerializeObject(message);

            // Send message to server
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
        public class UserMessage
        {
            public string username;
            public string messageText;
        }
    }
}