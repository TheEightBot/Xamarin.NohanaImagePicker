using System;
using UIKit;
using Photos;
using Foundation;
using Xamarin.NohanaImagePicker.Common;
using System.Collections.Generic;

namespace Xamarin.NohanaImagePicker
{

    public enum MediaType
    { 
        Any = 0, Photo, Video
    }

    public class NohanaImagePickerController : UIViewController
    {
        public int MaximumNumberOfSelection = 21; // set 0 to no limit
		public int NumberOfColumnsInPortrait = 4;
        public int NumberOfColumnsInLandscape = 7;

        public INohanaImagePickerControllerDelegate pickerDelegate;

        public bool ShouldShowMoment = true;

        public bool ShouldShowEmptyAlbum = false;

        public bool ToolbarHidden = false; 
        
        public bool CanPickAsset(IAsset asset)
        {
            return true;
        }
		
		public Config Conf { get; set; } = new Config();

        NSBundle _assetBundle;
        public NSBundle AssetBundle
        { 
            get 
            {
                if (_assetBundle == null)
                {
                    var bundle = NSBundle.MainBundle;
                    var path = bundle.BundlePath;
                    if (path != null)
                        _assetBundle = new NSBundle(path);
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

        public NohanaImagePickerController(List<PHAssetCollectionSubtype> assetCollectionSubtypes, MediaType mediaType, bool enableExpandingPhotoAnimation) : base(nibName: null, bundle: null)
        {
            this.AssetCollectionSubtypes = assetCollectionSubtypes;
            this.MediaType = mediaType;
            this.EnableExpandingPhotoAnimation = enableExpandingPhotoAnimation;
            this.PickedAssetList = new PickedAssetList();
            
            this.PickedAssetList.NohanaImagePickerController = this;
        }

        public NohanaImagePickerController(NSCoder aDecoder) : base(aDecoder)
        {
            throw new NotImplementedException();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var storyboard = UIStoryboard.FromName("NohanaImagePicker", null);
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
                var types = new List<PHAssetCollectionType>
                { 
                    PHAssetCollectionType.SmartAlbum,
                    PHAssetCollectionType.Album
                };
                var albumListViewController = navigationController.TopViewController as AlbumListViewController;
                if (albumListViewController != null)
                {
                    albumListViewController
                        .PhotoKitAlbumList =
                            new Photos.PhotoKitAlbumList(
                                assetCollectionTypes: types,
                                assetCollectionSubtypes: AssetCollectionSubtypes,
                                mediaType: MediaType,
                                shouldShowEmptyAlbum: ShouldShowEmptyAlbum,
                                handler: () => 
                                {
                                    // TODO:
                                    InvokeOnMainThread(() =>
                                    {
                                        albumListViewController.IsLoading = false;
                                        albumListViewController.TableView.ReloadData();
                                    });
                                });

                    albumListViewController.NohanaImagePickerController = this;
                            
                } 
            }
        }

        public void PickAsset(IAsset asset)
        { 
            PickedAssetList.Pick(asset);
        }

        public void DropAsset(IAsset asset)
        { 
            PickedAssetList.Drop(asset);
        }

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
