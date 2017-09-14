using System;
using CoreGraphics;
using Foundation;
using NohanaImagePicker.Xamarin.ViewControllers;
using NohanaImagePicker.Xamarin.Views;
using UIKit;
using AVFoundation;
using AVKit;

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

        public static CGRect ExpandingAnimationFromCellRect(AssetListViewController fromVC, AssetCell fromCell)
        {
            var origin = new CGPoint(
                x: fromCell.Frame.X,
                y: fromCell.Frame.Y - fromVC.CollectionView.ContentOffset.Y
            );
            return new CGRect(origin, fromCell.Frame.Size); 
        }

        public static CGRect ExpandingAnimationToCellRect(UIViewController fromVC, CGSize toSize)
        { 
            return AVFoundation.AVUtilities.WithAspectRatio(ScreenRectWithoutAppBar(fromVC), toSize); 
        }

        public static CGRect ContractingAnimationToCellRect(AssetListViewController toVC, AssetCell toCell)
        {
            var origin = new CGPoint(
                x: toCell.Frame.X,
                y: toCell.Frame.Y - toVC.CollectionView.ContentOffset.Y
            );
            return new CGRect(origin, toCell.Frame.Size);
        }

        public static CGRect ContractingAnimationFromCellRect(AssetDetailListViewController fromVC, AssetDetailCell fromCell, CGSize contractingImageSize)
        { 
            var rect = AVFoundation.AVUtilities.WithAspectRatio(fromCell.ImageView.Frame, contractingImageSize);
            rect.Y += Size.AppBarHeight(fromVC);
            rect.X -= fromCell.ScrollView.ContentOffset.X;
            rect.Y -= fromCell.ScrollView.ContentOffset.Y;
            return rect;
        }
    }
}
