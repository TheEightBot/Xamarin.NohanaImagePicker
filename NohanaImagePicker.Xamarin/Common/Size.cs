using System;
using CoreGraphics;
using UIKit;

namespace NohanaImagePicker.Xamarin.Common
{
    public struct Size
    {
        public static nfloat StatusBarHeight { 
            get 
            {
                if (UIApplication.SharedApplication.StatusBarHidden)  
                    return 0;  

                return UIApplication.SharedApplication.StatusBarFrame.Size.Height;    
            }
        }

        public static nfloat NavigationBarHeight(UIViewController viewController)
        {
            return viewController.NavigationController?.NavigationBar.Frame.Size.Height ?? (nfloat)0;
        }

        public static nfloat AppBarHeight(UIViewController viewController)
        {
            return StatusBarHeight + NavigationBarHeight(viewController);
        }

        public static nfloat Toolbarheight(UIViewController viewController)
        {
            var navigationController = viewController.NavigationController;

            if (navigationController == null) return (nfloat)0;

            if (!navigationController.ToolbarHidden) return (nfloat)0;

            return navigationController.Toolbar.Frame.Size.Height;
        }

        public static CGRect ScreenRectWithoutAppBar(UIViewController viewController)
        {
            var appBarHeight = Size.AppBarHeight(viewController);
            var toolbarHeight = Size.Toolbarheight(viewController);
            return new CGRect(
                x: 0,
                y: appBarHeight,
                width: UIScreen.MainScreen.Bounds.Width,
                height: UIScreen.MainScreen.Bounds.Height
            );
        }
    }
}
