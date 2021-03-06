﻿using Terminal.Gui;
using System;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;

namespace noWordChat
{
    class chat
    {

        // Insert server ip here
        public string serverIp = "http://localhost:3000";
        public List<String> oldMessagesList = new List<String>();
        static void Main()
        {

            Program P = new Program();
            chat c = new chat();



            // Creates the top-level window to show
            var win = new Window("noWordChat")
            {
                X = 0,
                Y = 0,

                // Make a full window
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };



            // Shows old messages
            var oldMessages = new ListView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),

                AllowsMultipleSelection = false,


            };


            List<String> chatWindowMessages = P.getMessages(0);
            chatWindowMessages.Reverse();

            oldMessages.SetSource(chatWindowMessages);


            System.Diagnostics.Debug.WriteLine("This is a log");
            Application.Init();
            var top = Application.Top;
            top.Add(win);

            var inputWin = new SearchWindow()
            {
                X = 0,
                Y = Pos.AnchorEnd() - 3,
                Width = Dim.Fill()
            };

            var inputUsername = new TextField("")
            {
                Width = 10
            };

            // Message input field
            var inputMessage = new TextField("")
            {
                Width = Dim.Fill(),
                X = Pos.Right(inputUsername) + 2
            };
            inputWin.Enter_Pressed += () =>
                  {
                      P.uploadToServer(inputMessage.Text.ToString(), inputUsername.Text.ToString());
                      inputMessage.Text = "";
                      win.SetNeedsDisplay();
                  };

            inputWin.Add(
              inputMessage,
              inputUsername
            );

            win.Add(

                oldMessages,
                inputWin

            );


            // Main loop of the app - refreshes message window
            int secondsPerRefresh = 5;
            bool colorChange = false;
            Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(secondsPerRefresh), x =>
            {
                // Change color of inputWin every 5s because reason.
                if (colorChange)
                {
                    inputWin.ColorScheme = new ColorScheme()
                    {
                        Focus = Terminal.Gui.Attribute.Make(Color.BrightGreen, Color.DarkGray),
                        Normal = Terminal.Gui.Attribute.Make(Color.Green, Color.BrightYellow),
                        HotFocus = Terminal.Gui.Attribute.Make(Color.BrightBlue, Color.Brown),
                        HotNormal = Terminal.Gui.Attribute.Make(Color.Red, Color.BrightRed),
                    };
                    colorChange = false;
                }
                else
                {
                    inputWin.ColorScheme = new ColorScheme()
                    {
                        Focus = Terminal.Gui.Attribute.Make(Color.BrightGreen, Color.Red),
                        Normal = Terminal.Gui.Attribute.Make(Color.Blue, Color.BrightGreen),
                        HotFocus = Terminal.Gui.Attribute.Make(Color.BrighCyan, Color.DarkGray),
                        HotNormal = Terminal.Gui.Attribute.Make(Color.Red, Color.BrightRed),
                    };
                    colorChange = true;
                }
                List<String> newMessages = P.getMessages(DateTimeOffset.Now.ToUnixTimeMilliseconds() - secondsPerRefresh * 1000);
                newMessages.Reverse();
                chatWindowMessages.InsertRange(0, newMessages);
                win.SetNeedsDisplay();
                return true;
            });


            inputUsername.Text = "Guest";
            Application.Run();
        }

        // One message
        public class Message
        {
            public string username;
            public string messageText;
            public long time;
        }

        // All the messages
        public class Messages
        {
            public Message[] messages;
        }

        public class Program
        {
            public List<String> oldMessagesListTest = new List<String>();

            public List<string> getMessages(long inTime)
            {


                chat c = new chat();

                WebClient cli = new WebClient();

                // Send message to server
                cli.Headers[HttpRequestHeader.ContentType] = "application/json";

                try
                {
                    string time = @"{""time"":""" + inTime + @"""}";
                    //System.Console.WriteLine(time);
                    string answer = cli.UploadString(new Uri(c.serverIp + "/chat/get"), "POST", time);
                    Messages oldMessages = JsonConvert.DeserializeObject<Messages>(answer);

                    for (int i = 0; i < oldMessages.messages.Length; i++)
                    {
                        // gets message time and converts it to local timezone
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(oldMessages.messages[i].time + long.Parse(TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalMilliseconds.ToString()));

                        c.oldMessagesList.Add(dateTimeOffset + " " + oldMessages.messages[i].username + ": " + oldMessages.messages[i].messageText);
                    }

                    return c.oldMessagesList;
                }
                catch (System.Exception)
                {
                    return c.oldMessagesList;
                    throw;
                }
            }

            // Uploads message to server
            public void uploadToServer(string userMessage, string username)
            {
                
                WebClient cli = new WebClient();

                chat c = new chat();
                // Convert before upload
                Message message = new Message();
                message.messageText = userMessage;
                message.username = username;
                string jsonSend = JsonConvert.SerializeObject(message);

                // Send message to server
                cli.Headers[HttpRequestHeader.ContentType] = "application/json";
                try
                {
                    cli.UploadString(new Uri(c.serverIp + "/chat/post"), "POST", jsonSend);
                }
                catch (WebException e)
                {

                    throw e;
                }
            }

            public void onEnterPress(KeyEvent e)
            {
                if (e.Key == Key.Enter)
                {
                    return;
                }
                else
                {
                    return;
                }
            }
        }
    }
    class SearchWindow : Window
    {
        public Action Enter_Pressed;

        public SearchWindow() : base("Message")
        {
        }
        public override bool ProcessKey(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.Enter)
            {
                if (Enter_Pressed != null)
                {
                    Enter_Pressed.Invoke();
                    return true;
                }
            }
            return base.ProcessKey(keyEvent);
        }
    }

}
