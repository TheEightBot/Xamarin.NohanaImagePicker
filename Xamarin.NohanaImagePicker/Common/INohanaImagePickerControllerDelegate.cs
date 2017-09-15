using System;
using System.Collections.Generic;
using Foundation;
using Xamarin.NohanaImagePicker;
using Photos;
using UIKit;

namespace Xamarin.NohanaImagePicker.Common
{
    public interface INohanaImagePickerControllerDelegate
    {
		//func nohanaImagePickerDidCancel(_ picker: NohanaImagePickerController) 
		void NohanaImagePickerDidCancel(NohanaImagePickerController picker);

		// func nohanaImagePicker(_ picker: NohanaImagePickerController, didFinishPickingPhotoKitAssets pickedAssts :[PHAsset])
		void NahonaImagesPicked(NohanaImagePickerController picker, List<PHAsset> pickedAssts);

		//@objc optional func nohanaImagePicker(_ picker: NohanaImagePickerController, willPickPhotoKitAsset asset: PHAsset, pickedAssetsCount: Int) -> Bool
		bool NahonaImagePickerWillPick(NohanaImagePickerController picker, PHAsset asset, int pickedAssetsCount);
		
        //@objc optional func nohanaImagePicker(_ picker: NohanaImagePickerController, didPickPhotoKitAsset asset: PHAsset, pickedAssetsCount: Int)
		void NahonaImagePickerDidPick(NohanaImagePickerController picker, PHAsset asset, int pickedAssetCount);
	     
    	//@objc optional func nohanaImagePicker(_ picker: NohanaImagePickerController, willDropPhotoKitAsset asset: PHAsset, pickedAssetsCount: Int) -> Bool
        bool NohanaImagePickerWillDrop(NohanaImagePickerController picker, PHAsset asset, int pickedAssetCount);

    	//@objc optional func nohanaImagePicker(_ picker: NohanaImagePickerController, didDropPhotoKitAsset asset: PHAsset, pickedAssetsCount: Int)
        void NahonaImagePickerDidDrop(NohanaImagePickerController picker, PHAsset asset, int pickedAssetCount);

    	//@objc optional func nohanaImagePicker(_ picker: NohanaImagePickerController, didSelectPhotoKitAsset asset: PHAsset)
        void NahonaImagePickerDidSelect(NohanaImagePickerController picker, PHAsset asset);

    	//@objc optional func nohanaImagePicker(_ picker: NohanaImagePickerController, didSelectPhotoKitAssetList assetList: PHAssetCollection)
        void NahonaImagePickerDidSelect(NohanaImagePickerController picker, PHAssetCollection assetList);

    	//@objc optional func nohanaImagePickerDidSelectMoment(_ picker: NohanaImagePickerController) -> Void
        void NohanaImagePickerDidSelectMoment(NohanaImagePickerController picker);

    	//@objc optional func nohanaImagePicker(_ picker: NohanaImagePickerController, assetListViewController: UICollectionViewController, cell: UICollectionViewCell, indexPath: IndexPath, photoKitAsset: PHAsset) -> UICollectionViewCell
        UICollectionViewCell NohanaImagePickerList(NohanaImagePickerController picker, UICollectionViewController assetListViewController, UICollectionViewCell cell, NSIndexPath indexPath, PHAsset photoKitAsset);

    	//@objc optional func nohanaImagePicker(_ picker: NohanaImagePickerController, assetDetailListViewController: UICollectionViewController, cell: UICollectionViewCell, indexPath: IndexPath, photoKitAsset: PHAsset) -> UICollectionViewCell
        UICollectionViewCell NohanaImagePickerDetailList(NohanaImagePickerController picker, UICollectionViewController assetDetailListViewController, UICollectionViewCell cell, NSIndexPath indexPath, PHAsset photoKitAsset);

    	//@objc optional func nohanaImagePicker(_ picker: NohanaImagePickerController, assetDetailListViewController: UICollectionViewController, didChangeAssetDetailPage indexPath: IndexPath, photoKitAsset: PHAsset)
        UICollectionViewCell NohanaImagePickerDidChange(NohanaImagePickerController picker, UICollectionViewController assetDetailListViewController, NSIndexPath indexPath, PHAsset photoKitAsset);

    }
}
