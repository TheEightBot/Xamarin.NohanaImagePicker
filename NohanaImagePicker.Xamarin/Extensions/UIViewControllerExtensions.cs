using System;
using UIKit;
using NohanaImagePicker.Xamarin.Common;
using System.Linq;

namespace NohanaImagePicker.Xamarin.Extensions
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
    }
}
