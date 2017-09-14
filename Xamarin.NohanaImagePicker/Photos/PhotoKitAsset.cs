using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using NohanaImagePicker.Xamarin.Common;
using Photos;

namespace NohanaImagePicker.Xamarin.Photos
{
    // DONE
    public class PhotoKitAsset : NSObject, IAsset
    { 
        public PHAsset Asset { get; set; }
 
        public PhotoKitAsset(PHAsset asset)
        {
            this.Asset = asset;
        }

        public PHAsset OriginalAsset 
        { 
            get => Asset as PHAsset; 
        }

        #region IAsset

        public int Identifier => Asset.LocalIdentifier.GetHashCode();

		public void Image(CGSize targetSize, Action<ImageData> handler)
		{
            var option = new PHImageRequestOptions();
            option.NetworkAccessAllowed = true;
 
            PHImageManager.DefaultManager.RequestImageForAsset(
                this.Asset,
                targetSize,
                PHImageContentMode.AspectFit,
                option, 
                (result, info) => new ImageData { Image = result, Info = info as NSDictionary<NSObject, NSObject> }); 
		}

        #endregion
    }
}
