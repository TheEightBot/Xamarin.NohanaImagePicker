using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using NohanaImagePicker.Xamarin.ViewControllers;
using Photos;
using Item = NohanaImagePicker.Xamarin.Photos.PhotoKitAsset;

namespace NohanaImagePicker.Xamarin.Photos
{
    // TODO: ICollection needs work
    public class PhotoKitAssetList : NSObject, Common.IItemList, ICollection<Item>
    { 
        private MediaType _mediaType;
        private PHAssetCollection _assetList;
        private PHFetchResult _fetchResult;

        public PhotoKitAssetList(PHAssetCollection album, MediaType mediaType)
        {
            _assetList = album;
            _mediaType = mediaType;
            Update();
        }
  
		#region ItemList

        public string Title => _assetList.LocalizedTitle ?? string.Empty;

        public NSDate Date => _assetList.StartDate;  

		public int Index(int i)
		{
			return i + 1;
		}

        public Item this[int index] => new Item((PHAsset)_fetchResult.ObjectAt(index));


        public static PHFetchOptions FetchOptions(MediaType mediaType)
        {
            var options = new PHFetchOptions();

            switch (mediaType)
            { 
                case MediaType.Photo:
                    options.Predicate = NSPredicate.FromFormat(string.Format("mediaType == {0}", (int)PHAssetMediaType.Image));
                    break;
                default:
                    throw new NotSupportedException("not supported. Non photo types are not supported yet."); 
            }

            return options;
        } 
         
        public void Update(Action handler = null)
		{
            _fetchResult = PHAsset.FetchAssets(_assetList, FetchOptions(_mediaType));
            handler?.Invoke();
        }
 

        #endregion

        #region ICollection
 
        public int StartIndex { get => 0; }

        public int EndIndex { get => (int)_fetchResult.Count; }

        public int Count => (int)_fetchResult.Count;

        public bool IsReadOnly => false;

		public void Add(Item item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{ 
			throw new NotImplementedException();
		}

		public bool Contains(Item item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(Item[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(Item item)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<Item> GetEnumerator()
		{ 
            return _fetchResult.GetEnumerator() as IEnumerator<Item>;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _fetchResult.GetEnumerator() as IEnumerator<Item>;
		}

        #endregion

    }
}
