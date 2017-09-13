using System;
using UIKit;
using Photos;
using Foundation;
using NohanaImagePicker.Xamarin.Common;

namespace NohanaImagePicker.Xamarin.ViewControllers
{

    public enum MediaType
    { 
        Any = 0, Photo, Video
    }

    public class NohanaImagePickerController : UIViewController
    {
        public int MaximumNumberOfSelection = 21;
        public int NumberOfColumnsInPortrait = 4;
        public int NumberOfColumnsInLandscape = 7;
        // open weak var delegate: NohanaImagePickerControllerDelegate?
        bool ShouldShowMoment = true;

        bool ShouldShowEmptyAlbum = false;

        bool ToolbarHidden = false; 

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
        bool EnableExpandingPhotoAnimation = false;


        public NohanaImagePickerController()
        {
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
