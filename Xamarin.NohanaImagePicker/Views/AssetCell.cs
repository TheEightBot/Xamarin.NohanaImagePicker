using System;
using Xamarin.NohanaImagePicker.Common;
using Xamarin.NohanaImagePicker.ViewControllers;
using UIKit;
using static Xamarin.NohanaImagePicker.Common.NotificationInfo;

namespace Xamarin.NohanaImagePicker.Views
{  
    
	public class AssetCell : UICollectionViewCell
    {
        public UIImageView ImageView { get; set; }
        public UIButton PickButton { get; set; }
        public UIView OverlayView { get; set; }

        public NohanaImagePickerController nohanaImagePickerController;
        public IAsset Asset;

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            if (nohanaImagePickerController != null)
            {
                var droppedImage = nohanaImagePickerController.Conf.Image.DroppedSmall ?? UIImage.FromBundle("btn_select_m", nohanaImagePickerController.AssetBundle, null);
                var pickedImage = nohanaImagePickerController.Conf.Image.DroppedSmall ?? UIImage.FromBundle("btn_selected_m", nohanaImagePickerController.AssetBundle, null);

                PickButton.SetImage(droppedImage, new UIControlState());
                PickButton.SetImage(pickedImage, UIControlState.Selected); 
            }
        }

        public void DidPushPickButton(UIButton sender)
        { 
            if (PickButton.Selected)
            {
                if (nohanaImagePickerController.PickedAssetList.Drop(Asset)) 
                    PickButton.Selected = false; 
            }
			else
			{
				if (nohanaImagePickerController.PickedAssetList.Pick(Asset)) 
					PickButton.Selected = false; 
			}
            this.OverlayView.Hidden = !PickButton.Selected;
        }

        public void Update(IAsset asset, NohanaImagePickerController nohanaImagePickerController)
        {
            this.Asset = asset;
            this.nohanaImagePickerController = nohanaImagePickerController;
            this.PickButton.Selected = nohanaImagePickerController.PickedAssetList.IsPicked(asset);
            this.OverlayView.Hidden = !PickButton.Selected;
            this.PickButton.Hidden = !(nohanaImagePickerController.CanPickAsset(asset));

        }
    }
}
