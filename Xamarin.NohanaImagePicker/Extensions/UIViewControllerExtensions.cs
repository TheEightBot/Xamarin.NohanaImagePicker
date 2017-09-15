using System;
using UIKit;
using Xamarin.NohanaImagePicker.Common;
using System.Linq;
using Foundation;
using Xamarin.NohanaImagePicker;

namespace Xamarin.NohanaImagePicker.Extensions
{
    public static class UIViewControllerExtensions
    {
        public static void UpdateVisibilityOfActivityIndicator(this UIViewController vc, UIView activityIndicator)
		{
            if (vc is Common.IActivityIndicatable) return;
                
            if ((vc as Common.IActivityIndicatable).IsProgressing())
			{
				if (!vc.View.Subviews.Contains(activityIndicator))
					vc.View.AddSubview(activityIndicator);
			}
			else
			{
				activityIndicator.RemoveFromSuperview();
			}
		}

		public static void UpdateVisibilityOfEmptyIndicator(this UIViewController vc, UIView emptyIndicator)
		{
            if (vc is Common.IEmptyIndicatable) return;

            if ((vc as Common.IEmptyIndicatable).IsEmpty())
			{

				if (!vc.View.Subviews.Contains(emptyIndicator))
					vc.View.AddSubview(emptyIndicator);
			}
			else
			{
				emptyIndicator.RemoveFromSuperview();
			} 
		}

		public static void SetUpToolbarItems(this UIViewController vc)
		{
			var leftSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, null, null);
			var rightSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, null, null);

			var infoAttributes = new UITextAttributes();
			infoAttributes.Font = UIFont.SystemFontOfSize(14);
			infoAttributes.TextColor = UIColor.Black;

			var infoButton = new UIBarButtonItem(string.Empty, UIBarButtonItemStyle.Plain, null, null);
			infoButton.Enabled = false;
			infoButton.SetTitleTextAttributes(infoAttributes, new UIControlState());

			var buttons = new UIBarButtonItem[] { leftSpace, rightSpace, infoButton };
			vc.SetToolbarItems(buttons, false);
		}

        public static void SetToolbarTitle(this UIViewController vc, NohanaImagePickerController nohanaImagePickerController)
        {
            var count = vc.ToolbarItems.Length;
            if (count >= 2)
            {
                if (vc.ToolbarItems[1] != null)
                {
                    UIBarButtonItem infoButton = vc.ToolbarItems[1];
                    infoButton.TintColor = UIColor.Black;
                    if (nohanaImagePickerController.MaximumNumberOfSelection == 0)
                    {
                        var title = NSString.LocalizedFormat(
                            (nohanaImagePickerController.Conf.Strings.ToolbarTitleNoLimit ?? NSString.LocalizedFormat($"Selected Items: "))
                                + $"{nohanaImagePickerController.PickedAssetList?.Count ?? 0}");
                                
                        infoButton.Title = title;
                    }
                    else
                    {
                        var title = NSString.LocalizedFormat(
                            (nohanaImagePickerController.Conf.Strings.ToolbarTitleHasLimit ?? NSString.LocalizedFormat($"Selected Items: "))
                                + $"{nohanaImagePickerController.PickedAssetList?.Count ?? 0} / {nohanaImagePickerController.MaximumNumberOfSelection}");
                            
						infoButton.Title = title;
                    }
                }
            }
        }


        public static void AddPickPhotoKitAssetNotificationObservers(this UIViewController vc)
        { 
            NSNotificationCenter.DefaultCenter.AddObserver(NotificationInfo.Asset.PhotoKit.DidPick, (notification) => DidPickPhotoKitAsset(vc, notification));
            NSNotificationCenter.DefaultCenter.AddObserver(NotificationInfo.Asset.PhotoKit.DidDrop, (notification) => DidDropPhotoKitAsset(vc, notification));
        }

        public static void DidPickPhotoKitAsset(UIViewController vc, NSNotification notification)
        {
            SetToolbarTitleFromNotification(vc, notification);
        }

		public static void DidDropPhotoKitAsset(UIViewController vc, NSNotification notification)
		{
            SetToolbarTitleFromNotification(vc, notification);
		}

        static void SetToolbarTitleFromNotification(UIViewController vc, NSNotification notification)
        {
            var nohanaImagePicker = notification.Object as NohanaImagePickerController;
            if (nohanaImagePicker == null)
                return;
                
            vc.SetToolbarTitle(nohanaImagePicker);
        }
    }
}
