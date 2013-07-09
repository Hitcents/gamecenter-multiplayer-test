using System;
using System.IO;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.GameKit;
using MonoTouch.UIKit;

namespace GameCenterMultiplayer
{
    public class Player
    {
        public Player()
        {
            string id = GKLocalPlayer.LocalPlayer.PlayerID;
        }
    }
}

