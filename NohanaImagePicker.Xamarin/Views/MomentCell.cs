using System;
using NohanaImagePicker.Xamarin.ViewControllers;
using UIKit;
using CoreGraphics;
namespace NohanaImagePicker.Xamarin.Views
{
    public class MomentCell : AlbumCell
    {
        NohanaImagePickerController.Config? config;

        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);
            var lineWidth = (nfloat)1 / UIScreen.MainScreen.Scale;
            var separatorColor = config?.Color.Separator ?? new UIColor(red: 0xbb / 0xff, green: 0xbb / 0xff, blue: 0xbb / 0xff, alpha: 1);
            separatorColor.SetFill();
            UIRectFill();
        }
    }
}
