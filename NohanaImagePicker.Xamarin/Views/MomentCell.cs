using System;
using NohanaImagePicker.Xamarin.ViewControllers;
using UIKit;
using CoreGraphics;
namespace NohanaImagePicker.Xamarin.Views
{
	//DONE
	public class MomentCell : AlbumCell
    {
        public NohanaImagePickerController.Config? Config;

        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);
            var lineWidth = (nfloat)1 / UIScreen.MainScreen.Scale;
            var separatorColor = Config?.Color.Separator ?? new UIColor(red: 0xbb / 0xff, green: 0xbb / 0xff, blue: 0xbb / 0xff, alpha: 1);
            separatorColor.SetFill();
            UIKit.UIGraphics.RectFill(new CGRect(x: 16, y: Frame.Size.Height - lineWidth, width: Frame.Size.Width, height: lineWidth));

        }
    }
}
