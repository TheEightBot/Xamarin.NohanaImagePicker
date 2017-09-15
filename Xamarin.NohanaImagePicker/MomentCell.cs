using System;
using Xamarin.NohanaImagePicker;
using UIKit;
using CoreGraphics;
namespace Xamarin.NohanaImagePicker
{
	//DONE
	public partial class MomentCell : AlbumCell
    {
        public NohanaImagePickerController.Config? Config;

        public MomentCell(IntPtr handle) : base(handle)
        {
        }

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
