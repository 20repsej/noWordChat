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
      System.Diagnostics.Debug.WriteLine("This is a log");
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

      var inputWin = new SearchWindow()
      {
        X = 0,
        Y = Pos.AnchorEnd() - 3,
        Width = Dim.Fill()
      };

      // Message input field
      var inputMessage = new TextField("")
      {
        Width = Dim.Fill() - 10
      };

      // Username input field
      var inputUsername = new TextField("")
      {
        X = Pos.Right(inputMessage),
        Width = 10
      };

      inputWin.Enter_Pressed += () =>
            {
              Program P = new Program();
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

      Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(10), x =>
      {
        Program P = new Program();
        P.getMessages();
        win.SetNeedsDisplay();
        return true;
      });

      Program P = new Program();
      chat c = new chat();

      oldMessages.SetSource(P.getFirstOldMessages());

      Application.Run();
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
    public class Program
    {
      public List<String> oldMessagesListTest = new List<String>();
      public List<string> getFirstOldMessages()
      {

        WebClient cli = new WebClient();

        try
        {
          string oldMessagesJson = cli.DownloadString("http://localhost:3000/chat/getFirst");
          Messages oldMessages = JsonConvert.DeserializeObject<Messages>(oldMessagesJson);

          for (int i = 0; i < oldMessages.messages.Length; i++)
          {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(oldMessages.messages[i].time);

            oldMessagesListTest.Add(dateTimeOffset + " " + oldMessages.messages[i].username + ": " + oldMessages.messages[i].messageText);
          }

          return oldMessagesListTest;
        }
        catch (System.Exception)
        {
          return oldMessagesListTest;
          throw;
        }
      }
      public List<string> getMessages()
      {

        WebClient cli = new WebClient();

        // Send message to server
        cli.Headers[HttpRequestHeader.ContentType] = "application/json";
        try
        {
          System.Console.WriteLine("Start");
          string answer = cli.UploadString(new Uri("http://localhost:3000/chat/get"), "POST", "testTime");
          Messages oldMessages = JsonConvert.DeserializeObject<Messages>(answer);

          for (int i = 0; i < oldMessages.messages.Length; i++)
          {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(oldMessages.messages[i].time);

            oldMessagesListTest.Add(dateTimeOffset + " " + oldMessages.messages[i].username + ": " + oldMessages.messages[i].messageText);
          }

          System.Console.WriteLine("Testing!");
          return oldMessagesListTest;
        }
        catch (System.Exception)
        {
          return oldMessagesListTest;
          throw;
        }
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