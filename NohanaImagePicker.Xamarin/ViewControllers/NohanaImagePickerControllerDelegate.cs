using System;
using UIKit;
using Photos;
using Foundation;
using NohanaImagePicker.Xamarin.Common;
using System.Collections.Generic;

namespace NohanaImagePicker.Xamarin.ViewControllers
{

    public enum MediaType
    { 
        Any = 0, Photo, Video
    }

    public class NohanaImagePickerController : UIViewController, INohanaImagePickerControllerDelegate
    {
        public int MaximumNumberOfSelection = 21; // set 0 to no limit
		public int NumberOfColumnsInPortrait = 4;
        public int NumberOfColumnsInLandscape = 7;

        public INohanaImagePickerControllerDelegate pickerDelegate;

        public bool ShouldShowMoment = true;

        public bool ShouldShowEmptyAlbum = false;

        public bool ToolbarHidden = false; 

        public bool CanPickAsset(Action asset)
        {
            return true;
        }
		
		public Config Conf { get; set; } = new Config();

        NSBundle _assetBundle;
        public NSBundle AssetBundle
        { 
            get {
                if (_assetBundle == null)
                {
                    var bundle = new NSBundle();
                    var path = bundle.PathForResource("NohanaImagePicker", ofType: "bundle");
                    if (path != null)
                    {
                        _assetBundle = new NSBundle(path);
                    }
                }

                return _assetBundle;
            }
        }

        public PickedAssetList PickedAssetList;
        public MediaType MediaType;
        public bool EnableExpandingPhotoAnimation = false;
        public List<PHAssetCollectionSubtype> AssetCollectionSubtypes;

        public NohanaImagePickerController() : base(nibName: null, bundle: null)
        { 
            AssetCollectionSubtypes = new List<PHAssetCollectionSubtype>
            { 
                PHAssetCollectionSubtype.AlbumRegular, 
				PHAssetCollectionSubtype.AlbumSyncedEvent, 
                PHAssetCollectionSubtype.AlbumSyncedFaces, 
                PHAssetCollectionSubtype.AlbumSyncedAlbum, 
                PHAssetCollectionSubtype.AlbumImported, 
                PHAssetCollectionSubtype.AlbumMyPhotoStream, 
                PHAssetCollectionSubtype.AlbumCloudShared, 
                PHAssetCollectionSubtype.SmartAlbumGeneric, 
                PHAssetCollectionSubtype.SmartAlbumFavorites, 
                PHAssetCollectionSubtype.SmartAlbumRecentlyAdded, 
                PHAssetCollectionSubtype.SmartAlbumUserLibrary
            };

            MediaType = MediaType.Photo;
            PickedAssetList = new PickedAssetList();
            EnableExpandingPhotoAnimation = true;
            this.PickedAssetList.NohanaImagePickerController = this;

        }

        public NohanaImagePickerController(NSCoder aDecoder) : base(aDecoder)
        {
            throw new NotImplementedException();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var storyboard = UIStoryboard.FromName("NohanaImagePicker", AssetBundle);
            var viewControllerId = 
                EnableExpandingPhotoAnimation
                    ? "EnableAnimationNavigationController"
                    : "DisableAnimationNavigationController";

            var navigationController = storyboard.InstantiateViewController(viewControllerId) as UINavigationController;
            if (navigationController != null)
            {
                AddChildViewController(navigationController);
                View.AddSubview(navigationController.View);
                navigationController.DidMoveToParentViewController(this);

                var albumListViewController = navigationController.TopViewController as AlbumListViewController;
                if (albumListViewController != null)
                { 
                    albumListViewController
                }



            }
        } 


        #region INohanaImagePickerControllerDelegate


        public void NohanaImagePickerDidCancel(NohanaImagePickerController picker)
		{
			throw new NotImplementedException();
		}

		public void NahonaImagePicker(NohanaImagePickerController picker, List<PHAsset> pickedAssts)
		{
			throw new NotImplementedException();
		}

		public bool NahonaImagePickerWillPick(NohanaImagePickerController picker, PHAsset asset, int pickedAssetsCount)
		{
			throw new NotImplementedException();
		}

		public void NahonaImagePickerDidPick(NohanaImagePickerController picker, PHAsset asset, int pickedAssetCount)
		{
			throw new NotImplementedException();
		}

		public bool NahonaImagePickerWillDrop(NohanaImagePickerController picker, PHAsset asset, int pickedAssetCount)
		{
			throw new NotImplementedException();
		}

		public void NahonaImagePickerDidDrop(NohanaImagePickerController picker, PHAsset asset, int pickedAssetCount)
		{
			throw new NotImplementedException();
		}

		public void NahonaImagePickerDidSelect(NohanaImagePickerController picker, PHAsset asset)
		{
			throw new NotImplementedException();
		}

		public void NahonaImagePickerDidSelect(NohanaImagePickerController picker, PHAssetCollection assetList)
		{
			throw new NotImplementedException();
		}

		public void NohanaImagePickerDidSelectMoment(NohanaImagePickerController picker)
		{
			throw new NotImplementedException();
		}

		public UICollectionViewCell NohanaImagePickerList(NohanaImagePickerController picker, UICollectionViewController assetListViewController, UICollectionViewCell cell, NSIndexPath indexPath, PHAsset photoKitAsset)
		{
			throw new NotImplementedException();
		}

		public UICollectionViewCell NohanaImagePickerDetailList(NohanaImagePickerController picker, UICollectionViewController assetDetailListViewController, UICollectionViewCell cell, NSIndexPath indexPath, PHAsset photoKitAsset)
		{
			throw new NotImplementedException();
		}

		public UICollectionViewCell NohanaImagePickerDidChange(NohanaImagePickerController picker, UICollectionViewController assetDetailListViewController, UICollectionViewCell cell, NSIndexPath indexPath, PHAsset photoKitAsset)
		{
			throw new NotImplementedException();
		}

		#endregion



		public struct Config
        {
            public ColorGroup Color { get; set; }
            public ImageGroup Image { get; set; }
            public StringGroup Strings { get; set; }

            public struct ColorGroup
            {
                public UIColor Background { get; set; }
                public UIColor Empty { get; set; }
                public UIColor Separator { get; set; }
            }
 
            public struct ImageGroup
            {
                public UIImage PickedSmall { get; set; }
    			public UIImage PickedLarge { get; set; }
    			public UIImage DroppedSmall { get; set; }
    			public UIImage DroppedLarge { get; set; }
            }

			public struct StringGroup
			{
				public string AlbumListTitle { get; set; }
    			public string AlbumListMomentTitle { get; set; }
    			public string AlbumListEmptyMessage { get; set; }
    			public string AlbumListEmptyDescription { get; set; }
    			public string ToolbarTitleNoLimit { get; set; }
    			public string ToolbarTitleHasLimit { get; set; }
			}
        }
    }
}
