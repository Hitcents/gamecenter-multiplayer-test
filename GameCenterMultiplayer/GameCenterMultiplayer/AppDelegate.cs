using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.GameKit;

namespace GameCenterMultiplayer
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        bool gameCenterAuthenticationComplete = false;
        string currentPlayerID;
        MainController viewController;

        // class-level declarations
        public override UIWindow Window
        {
            get;
            set;
        }
        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation(UIApplication application)
        {
        }
        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
        }

        /// This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
        }

        /// This method is called when the application is about to terminate. Save data, if needed. 
        public override void WillTerminate(UIApplication application)
        {
        }

        public override void FinishedLaunching(UIApplication application)
        {
           viewController = new MainController(Handle);

           if (!isGameCenterAPIAvailable ())
                gameCenterAuthenticationComplete = false;

            else 
            {
                GKLocalPlayer.LocalPlayer.AuthenticateHandler = (ui, error) => 
                {
                    if(ui != null)
                    {
                        viewController.PresentViewController(ui,true,null);
                    }
                    else if(error != null)
                    {
                        new UIAlertView ("Error", error.ToString(), null, "OK", null).Show();
                    }
                    else if(GKLocalPlayer.LocalPlayer.Authenticated)
                    {
                        this.gameCenterAuthenticationComplete = true;

                        //Switching Users
                        if(currentPlayerID != null || currentPlayerID != GKLocalPlayer.LocalPlayer.PlayerID)
                        {
                            currentPlayerID = GKLocalPlayer.LocalPlayer.PlayerID;
                            viewController.player = new Player();
                        }
                    }
                    else
                    {
                        this.gameCenterAuthenticationComplete = false;
                    }

                };
            }
            
        }

        private bool isGameCenterAPIAvailable()
        {
            return UIDevice.CurrentDevice.CheckSystemVersion (4, 1);
        }
    }
}

