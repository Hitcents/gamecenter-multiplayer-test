// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace GameCenterMultiplayer
{
	[Register ("MainController")]
	partial class MainController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton _createMatch { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView _displayText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton _sendButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField _textEntry { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_displayText != null) {
				_displayText.Dispose ();
				_displayText = null;
			}

			if (_textEntry != null) {
				_textEntry.Dispose ();
				_textEntry = null;
			}

			if (_sendButton != null) {
				_sendButton.Dispose ();
				_sendButton = null;
			}

			if (_createMatch != null) {
				_createMatch.Dispose ();
				_createMatch = null;
			}
		}
	}
}
