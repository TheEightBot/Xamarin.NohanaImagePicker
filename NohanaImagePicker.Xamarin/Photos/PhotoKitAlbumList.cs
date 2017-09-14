using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using NohanaImagePicker.Xamarin.ViewControllers;
using Photos;
using Item = NohanaImagePicker.Xamarin.Photos.PhotoKitAssetList;

namespace NohanaImagePicker.Xamarin.Photos
{
    // DONE
    public class PhotoKitAlbumList : NSObject, Common.IItemList, ICollection<Item>
    { 
        private List<Item> _albumList = new List<Item>();
        private List<PHAssetCollectionType> _assetCollectionTypes;
        private List<PHAssetCollectionSubtype> _assetCollectionSubtypes;
        private MediaType _mediaType { get; set; }
        private bool _shouldShowEmptyAlbum;

        // TODO: need help with Action
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

        public void Update(Action handler = null)
        {
            List<PHFetchResult> albumListFetchResult = new List<PHFetchResult>();
            InvokeInBackground(() =>
            {
                foreach (var type in _assetCollectionTypes)
                {

                    albumListFetchResult.AddRange(new List<PHFetchResult> { PHAssetCollection.FetchAssetCollections(type, PHAssetCollectionSubtype.Any, null) });
                }

                this._albumList = new List<Item>();
                var tmpAlbumList = new List<Item>();
                var isAssetCollectionSubtypeAny = this._assetCollectionSubtypes.Contains(PHAssetCollectionSubtype.Any);

                foreach (var fetchResult in albumListFetchResult)
                {

                    fetchResult.Enumerate((NSObject album, nuint index, out bool stop) =>
                    {
                        var phAlbum = album as PHAssetCollection;
                        if (this._assetCollectionSubtypes.Contains((album as PHAssetCollection).AssetCollectionSubtype) || isAssetCollectionSubtypeAny)
                        {
                            if (this._shouldShowEmptyAlbum || PHAsset.FetchAssets((album as PHAssetCollection), PhotoKitAssetList.FetchOptions(this._mediaType)).Count() != 0)
                            {
                                tmpAlbumList.Add(new PhotoKitAssetList(album as PHAssetCollection, this._mediaType));
                            }
                        }

                        stop = false;
                    });
                }

                if (this._assetCollectionTypes.Count() == 1 && this._assetCollectionTypes.Contains(PHAssetCollectionType.Moment))
                    this._albumList = tmpAlbumList.OrderByDescending(x => x.Date).ToList();
                else
                    this._albumList = tmpAlbumList;

                handler?.Invoke();

            });
        }

        public Item this[int index] => _albumList[index];

		#endregion

		#region ICollection

        public int Count => _albumList.Count;

		public bool IsReadOnly => false; 

        public void Add(Item item)
		{
			_albumList.Add(item);
		}

		public void Clear()
		{
			_albumList.Clear();
		}

		public bool Contains(Item item)
		{
			return _albumList.Contains(item);
		}

		public void CopyTo(Item[] array, int arrayIndex)
		{
			_albumList.CopyTo(array, arrayIndex);
		}

		public bool Remove(Item item)
		{
			return _albumList.Remove(item);
		}

		public IEnumerator<Item> GetEnumerator()
		{
			return _albumList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _albumList.GetEnumerator();
		} 

        #endregion
    }
}
