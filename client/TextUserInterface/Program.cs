using Terminal.Gui;
using System;
using Mono.Terminal;

class Chat {
	class Box10x : View {
		public Box10x (int x, int y) : base (new Rect (x, y, 10, 10))
		{
		}

		public override void Redraw (Rect region)
		{
			Driver.SetAttribute (ColorScheme.Focus);

			for (int y = 0; y < 10; y++) {
				Move (0, y);
				for (int x = 0; x < 10; x++) {

					Driver.AddRune ((Rune)('0' + (x + y) % 10));
				}
			}

		}
	}

	class Filler : View {
		public Filler (Rect rect) : base (rect)
		{
		}

		public override void Redraw (Rect region)
		{
			Driver.SetAttribute (ColorScheme.Focus);
			var f = Frame;

			for (int y = 0; y < f.Width; y++) {
				Move (0, y);
				for (int x = 0; x < f.Height; x++) {
					Rune r;
					switch (x % 3) {
					case 0:
						r = '.';
						break;
					case 1:
						r = 'o';
						break;
					default:
						r = 'O';
						break;
					}
					Driver.AddRune (r);
				}
			}
		}
	}

	static void ShowEntries (View container)
	{

		// Application.MainLoop.AddTimeout (TimeSpan.FromMilliseconds (300), timer);
		var messageHistory = new ListView (new Rect (1, 6, 16, 4), new string [] {
			"First Message",
			"Next message",
			"Third Message and some more text"
		});

		var chatMessage = new TextField ("") {
			X = 0,
			Y = Pos.AnchorEnd() -1,
			Width = Dim.Fill(),
			Height = 3
		};

		container.Add (
			messageHistory,
			chatMessage
		);

	}

	static bool Quit ()
	{
		var n = MessageBox.Query (50, 7, "Quit Chat", "Quit?", "Yes", "No");
		return n == 0;
	}
	static void Main ()
	{
		//Application.UseSystemConsole = true;
		Application.Init ();

		var top = Application.Top;
		var tframe = top.Frame;

		var win = new Window ("NoWordChat"){
			X = 0,
			Y = 1,
			Width = Dim.Fill (),
			Height = Dim.Fill () - 1
		};					
		var menu = new MenuBar (new MenuBarItem [] {
			new MenuBarItem ("_Chat", new MenuItem [] {
				new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
			})
		});

		ShowEntries (win);


		top.Add (win, menu);
		top.Add (menu);
		Application.Run ();
	}
}