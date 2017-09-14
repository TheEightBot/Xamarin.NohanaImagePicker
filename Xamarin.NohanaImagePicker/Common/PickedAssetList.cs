using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using NohanaImagePicker.Xamarin.Common;
using NohanaImagePicker.Xamarin.Photos;
using NohanaImagePicker.Xamarin.ViewControllers;
using static NohanaImagePicker.Xamarin.Common.NotificationInfo;
using Item = NohanaImagePicker.Xamarin.Common.IAsset;

namespace NohanaImagePicker.Xamarin.Common
{
    //TODO: pick drop
    public class PickedAssetList : Common.IItemList, ICollection<Item>
    {
        public PickedAssetList()
        {
        } 

        List<Item> _assetList;
        public NohanaImagePickerController NohanaImagePickerController;

        #region ItemList
 
        public string Title => "Selected Assets";
 
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

		#region ICollection

		public int Count => _assetList.Count;

		public bool IsReadOnly => false;
 
        public void Add(Item item)
        {
            _assetList.Add(item);
        }

		public void Clear()
		{
			_assetList.Clear();
		}

		public bool Contains(Item item)
		{
			return _assetList.Contains(item);
		}

		public void CopyTo(Item[] array, int arrayIndex)
		{
			_assetList.CopyTo(array, arrayIndex);
		} 

		public bool Remove(Item item)
		{
			return _assetList.Remove(item);
		}

		public IEnumerator<Item> GetEnumerator()
		{
			return _assetList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _assetList.GetEnumerator();
		}

        #endregion

        #region Manage assetlist

        public bool Pick(IAsset asset)
        {
            if (!IsPicked(asset))
                return false;

            var assetsCountBeforePicking = this.Count;
            if (asset is PhotoKitAsset)
            {
                var canPick =
                    NohanaImagePickerController
                        .pickerDelegate.NahonaImagePickerWillPick(NohanaImagePickerController, (asset as PhotoKitAsset).OriginalAsset, assetsCountBeforePicking);

                if (!canPick) return false;
            }

            if (!(NohanaImagePickerController.MaximumNumberOfSelection == 0
                || assetsCountBeforePicking < NohanaImagePickerController.MaximumNumberOfSelection))
            {
                return false;
            }
            _assetList.Append(asset);

            var assetsCountAfterPicker = this.Count;
            if (asset is PhotoKitAsset)
            {
                var originalAsset = (asset as PhotoKitAsset).OriginalAsset;
                NohanaImagePickerController.pickerDelegate.NahonaImagePickerDidPick(NohanaImagePickerController, originalAsset, assetsCountBeforePicking);
                var info = new NSDictionary<NSString, NSObject>();
                info.SetValueForKey(originalAsset, (Foundation.NSString)NotificationInfo.Asset.PhotoKit.DidPickUserInfoKeyAsset);
                info.SetValueForKey((NSNumber)assetsCountBeforePicking, (Foundation.NSString)NotificationInfo.Asset.PhotoKit.DidPickUserInfoKeyPickedAssetsCount);

                NSNotificationCenter.DefaultCenter.PostNotification(
                    NSNotification.FromName(
                        name: NotificationInfo.Asset.PhotoKit.DidPick,
                        obj: NohanaImagePickerController,

                        userInfo: info
                    ) 
                );
            }

            return true; 
        }

		public bool Drop(IAsset asset)
		{ 
            var assetsCountBeforeDropping = this.Count();
            if (asset is PhotoKitAsset)
            {
                var canDrop = NohanaImagePickerController.pickerDelegate.NohanaImagePickerWillDrop(NohanaImagePickerController, (asset as PhotoKitAsset).OriginalAsset, assetsCountBeforeDropping);
                if (!canDrop) return false;

                _assetList = _assetList.Where(x => x.Identifier != asset.Identifier).ToList();
                var assetsCountAfterDropping = this.Count();
                if (asset is PhotoKitAsset)
                {
                    var originalAsset = (asset as PhotoKitAsset).OriginalAsset;
                    if (originalAsset != null)
                    {
                        //TODO: uncomment after event is implemented 
                        //NohanaImagePickerController.NahonaImagePickerDidDrop(NohanaImagePickerController, originalAsset, assetsCountAfterDropping);

                        var info = new NSDictionary<NSString, NSObject>();
                        info.SetValueForKey(originalAsset, (Foundation.NSString)NotificationInfo.Asset.PhotoKit.DidDropUserInfoKeyAsset);
                        info.SetValueForKey((NSNumber)assetsCountAfterDropping, (Foundation.NSString)NotificationInfo.Asset.PhotoKit.DidDropUserInfoKeyPickedAssetsCount);

                        NSNotificationCenter.DefaultCenter.PostNotification(
                           NSNotification.FromName(
                               name: NotificationInfo.Asset.PhotoKit.DidPick,
                               obj: NohanaImagePickerController,
                               userInfo: info
                           )
                       );
                    } 
                } 
            } 
                
			return true;
		}

		public bool IsPicked(IAsset asset)
		{  
            return _assetList.FirstOrDefault(x => x.Identifier == asset.Identifier) != null;
		}
        #endregion
 
    }
} 