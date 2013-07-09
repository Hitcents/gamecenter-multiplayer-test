using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.GameKit;
using System.Runtime.InteropServices;
using System.Text;

namespace GameCenterMultiplayer
{
	public partial class MainController : UIViewController
	{
        public Player player;
        GKMatch match;

		public MainController (IntPtr handle) : base (handle)
		{
		}

        public override bool ShouldAutorotate()
        {
            return true;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Landscape;
        }

        [Obsolete ("Deprecated in iOS6. Replace it with both GetSupportedInterfaceOrientations and PreferredInterfaceOrientationForPresentation")]
        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            return toInterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || toInterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _displayText.Text = string.Empty;

            _textEntry.EditingDidEndOnExit += (object sender, EventArgs e) => 
            {
                _textEntry.EndEditing(true);
            };

            _sendButton.TouchUpInside += (object sender, EventArgs e) => 
            {
                SendMessage();
            };

            _createMatch.TouchUpInside += (object sender, EventArgs e) => 
            {
                CreateMatch();
            };
        }

        void CreateMatch()
        {
            var matchRequest = new GKMatchRequest();
            matchRequest.MinPlayers = 2;
            matchRequest.MaxPlayers = 2;

            GKMatchmakerViewController matchMakerController = new GKMatchmakerViewController(matchRequest);

            PresentViewController (matchMakerController, true, null);

            matchMakerController.DidFindMatch += (sender, e) => 
            {
                matchMakerController.DismissViewController(true, null);

                match = e.Match;
                _createMatch.Enabled = false;

                match.DataReceived += (obj, args) => 
                {
                    string text = ToString(args.Data);

                    _displayText.Text += "\n" + text;
                };
                match.StateChanged += (obj, args) => 
                {
                    match.Disconnect();
                    var popup = new UIAlertView("Oops", "You have been disconnected from the match", null, "Okay");
                    popup.Show();
                    _createMatch.Enabled = true;
                    _displayText.Text = string.Empty;
                };
                match.Failed += (obj, args) => 
                {

                };
            };

            matchMakerController.DidFailWithError += (sender, e) => 
            {
                matchMakerController.DismissViewController(true, null);
            };
            matchMakerController.WasCancelled += (sender, e) => 
            {
                matchMakerController.DismissViewController(true, null);
            };
        }

        void SendMessage()
        {
            if(string.IsNullOrEmpty(_textEntry.Text))
            {
                var popup = new UIAlertView("Oops", "No text has been entered", null, "Okay");
                popup.Show();
            }
            else
            {
                var err = new NSError();
                _displayText.Text += "\n" + (GKLocalPlayer.LocalPlayer.Alias + ": " + _textEntry.Text);
                using (var data = ToData(GKLocalPlayer.LocalPlayer.Alias + ": " + _textEntry.Text))
                {
                    match.SendData(data, match.PlayersIDs, GKMatchSendDataMode.Reliable, err);
                }
                _textEntry.Text = string.Empty;
            }
        }

        NSData ToData(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            return NSData.FromArray(bytes);
        }

        string ToString(NSData data)
        {
            byte[] bytes = new byte[data.Length];

            Marshal.Copy(data.Bytes, bytes, 0, (int)data.Length);

            return Encoding.UTF8.GetString(bytes);
        }
	}
}
