using System;
using UIKit;
using Photos;
using Foundation;

namespace NohanaImagePicker.Xamarin.ViewControllers
{

    public enum MediaType
    { 
        Any = 0, Photo, Video
    }

    public class NohanaImagePickerController : UIViewController
    {
        int _maximumNumberOfSelection = 21;
        int _numberOfColumnsInPortrait = 4;
        int _numberOfColumnsInLandscape = 7;
        // open weak var delegate: NohanaImagePickerControllerDelegate?
        bool _shouldShowMoment = true;

        bool _shouldShowEmptyAlbum = false;

        bool _toolbarHidden = false; 

        public bool CanPickAsset(Action asset)
        {
            return true;
        }

		Config Conf { get; set; } = new Config();

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
                public UIImage pickedSmall { get; set; }
    			public UIImage pickedLarge { get; set; }
    			public UIImage droppedSmall { get; set; }
    			public UIImage droppedLarge { get; set; }
            }

			public struct StringGroup
			{
				public string albumListTitle { get; set; }
    			public string albumListMomentTitle { get; set; }
    			public string albumListEmptyMessage { get; set; }
    			public string albumListEmptyDescription { get; set; }
    			public string toolbarTitleNoLimit { get; set; }
    			public string toolbarTitleHasLimit { get; set; }
			}
        }
    }
}
