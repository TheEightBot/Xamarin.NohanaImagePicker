using System;
using Xamarin.NohanaImagePicker.Common;
using Xamarin.NohanaImagePicker;
using UIKit;
using static Xamarin.NohanaImagePicker.Common.NotificationInfo;

namespace Xamarin.NohanaImagePicker
{  
    
	public partial class AssetCell : UICollectionViewCell
    {

        public NohanaImagePickerController nohanaImagePickerController;
        public IAsset Asset;

        protected internal AssetCell(IntPtr handle) : base(handle)
        {
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            if (nohanaImagePickerController != null)
            {
                var droppedImage = nohanaImagePickerController.Conf.Image.DroppedSmall ?? UIImage.FromBundle("btn_select_m", nohanaImagePickerController.AssetBundle, null);
                var pickedImage = nohanaImagePickerController.Conf.Image.DroppedSmall ?? UIImage.FromBundle("btn_selected_m", nohanaImagePickerController.AssetBundle, null);

                pickButton.SetImage(droppedImage, new UIControlState());
                pickButton.SetImage(pickedImage, UIControlState.Selected); 
            }
        }

        public void DidPushPickButton(UIButton sender)
        { 
            if (pickButton.Selected)
            {
                if (nohanaImagePickerController.PickedAssetList.Drop(Asset)) 
                    pickButton.Selected = false; 
            }
			else
			{
				if (nohanaImagePickerController.PickedAssetList.Pick(Asset)) 
					pickButton.Selected = false; 
			}
            this.overlayView.Hidden = !pickButton.Selected;
        }

        public void Update(IAsset asset, NohanaImagePickerController nohanaImagePickerController)
        {
            this.Asset = asset;
            this.nohanaImagePickerController = nohanaImagePickerController;
            this.pickButton.Selected = nohanaImagePickerController.PickedAssetList.IsPicked(asset);
            this.overlayView.Hidden = !pickButton.Selected;
            this.pickButton.Hidden = !(nohanaImagePickerController.CanPickAsset(asset));

        }
    }
}
