using System;
using UIKit;

namespace NohanaImagePicker.Xamarin.Views
{
    public class AssetCell : UICollectionViewCell
    {
        public UIImageView ImageView { get; set; }
        public UIButton PickButton { get; set; }
        public UIView OverlayView { get; set; }
    }
}
