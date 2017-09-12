using System;
using System.Collections.Generic;
using NohanaImagePicker.Xamarin.ViewControllers;
using Photos;
using Item = NohanaImagePicker.Xamarin.Common.IAsset;

namespace NohanaImagePicker.Xamarin.Photos
{
    public class PhotoKitAlbumList : Common.IItemList
    {

        private List<Item> _assetList = new List<Item>();
        private List<PHAssetCollectionType> _assetCollectionTypes;
        private List<PHAssetCollectionSubtype> _assetCollectionSubtypes;
        private MediaType _mediaType { get; set; }
        private bool _shouldShowEmptyAlbum;

        public PhotoKitAlbumList(List<PHAssetCollectionType> assetCollectionTypes, List<PHAssetCollectionSubtype> assetCollectionSubtypes, MediaType mediaType, bool shouldShowEmptyAlbum, Action handler)
        {
            _assetCollectionTypes = assetCollectionTypes;
            _assetCollectionSubtypes = assetCollectionSubtypes;
            _mediaType = mediaType;
            _shouldShowEmptyAlbum = shouldShowEmptyAlbum; 
            Update(handler);
        }

		#region ItemList

		public string Title => "PhotoKit";

		public int Index(int i)
		{
			return i + 1;
		}

		public void Update(Action handler)
		{
			throw new Exception("Not supported");
		}

		public int StartIndex { get => 0; }

		public int EndIndex { get => _assetList.Count; }

		public Item this[int index] => _assetList[index];

		#endregion
	}
}
