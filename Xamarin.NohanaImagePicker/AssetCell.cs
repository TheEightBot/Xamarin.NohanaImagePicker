using System;
using Xamarin.NohanaImagePicker.Common;
using Xamarin.NohanaImagePicker;
using UIKit;
using static Xamarin.NohanaImagePicker.Common.NotificationInfo;

namespace Xamarin.NohanaImagePicker
{  
    
	public partial class AssetCell : UICollectionViewCell
    {

        public NohanaImagePickerController NohanaImagePickerController;
        public IAsset Asset;

        protected internal AssetCell(IntPtr handle) : base(handle)
        {
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            if (NohanaImagePickerController != null)
            {
                var droppedImage = NohanaImagePickerController.Conf.Image.DroppedSmall ?? UIImage.FromBundle("btn_select_m");
                var pickedImage = NohanaImagePickerController.Conf.Image.DroppedSmall ?? UIImage.FromBundle("btn_selected_m", NohanaImagePickerController.AssetBundle, null);

                pickButton.SetImage(droppedImage, new UIControlState());
                pickButton.SetImage(pickedImage, UIControlState.Selected); 
                
                pickButton.TouchUpInside -= PickButton_TouchUpInside;
                pickButton.TouchUpInside += PickButton_TouchUpInside;
            }
        }

        public void DidPushPickButton()
        { 
            if (pickButton.Selected)
            {
                if (NohanaImagePickerController.PickedAssetList.Drop(Asset)) 
                    pickButton.Selected = false; 
            }
			else
			{
				if (NohanaImagePickerController.PickedAssetList.Pick(Asset)) 
					pickButton.Selected = true; 
			}
            this.overlayView.Hidden = !pickButton.Selected;
        }

        public void Update(IAsset asset, NohanaImagePickerController nohanaImagePickerController)
        {
            this.Asset = asset;
            this.NohanaImagePickerController = nohanaImagePickerController;
            this.pickButton.Selected = nohanaImagePickerController.PickedAssetList.IsPicked(asset);
            this.overlayView.Hidden = !pickButton.Selected;
            this.pickButton.Hidden = !(nohanaImagePickerController.CanPickAsset(asset));

        }

        void PickButton_TouchUpInside(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("fires");
            DidPushPickButton();
        }
    }
}
