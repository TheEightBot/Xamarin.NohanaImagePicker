using System;
using Foundation;
using Xamarin.NohanaImagePicker.Photos;
using UIKit;
using Xamarin.NohanaImagePicker.Extensions;
using CoreGraphics;
using Xamarin.NohanaImagePicker.Views;
using Photos;
using System.Linq;

namespace Xamarin.NohanaImagePicker.ViewControllers
{
    public class AssetListViewController : UICollectionViewController, IUICollectionViewDelegateFlowLayout
    {

        public NohanaImagePickerController _nohanaImagePickerController;
        public PhotoKitAssetList _photoKitAssetList;

        public AssetListViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = _nohanaImagePickerController?.Conf.Color.Background ?? UIColor.White;
            UpdateTitle();
            this.SetUpToolbarItems();
            this.AddPickPhotoKitAssetNotificationObservers();
        }

        public virtual CGSize CellSize
        {
            get
            {
                if (_nohanaImagePickerController == null)
                {
                    return CGSize.Empty;
                }
                var numberOfColumns = _nohanaImagePickerController.NumberOfColumnsInLandscape;
                if (UIInterfaceOrientation.Portrait.IsPortrait())
                {
                    numberOfColumns = _nohanaImagePickerController.NumberOfColumnsInPortrait;
                }
                var cellMargin = (nfloat)2;
                var cellWidth = (View.Frame.Width - cellMargin * ((nfloat)numberOfColumns) - 1) / (nfloat)numberOfColumns;
                return new CGSize(width: cellWidth, height: cellWidth);
            }
        }

        ~AssetListViewController()
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(this);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (_nohanaImagePickerController != null)
            {
                this.SetToolbarTitle(_nohanaImagePickerController);
            }
            this.CollectionView?.ReloadData();
            this.ScrollCollectionViewToInitialPosition();
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            base.ViewWillTransitionToSize(toSize, coordinator);
            // TODO: check
            coordinator.AnimateAlongsideTransition((_) => 
            {
                if (this.NavigationController?.VisibleViewController != this)
                {
                    this.View.Frame = new CGRect(
                        x: this.View.Frame.X,
                        y: this.View.Frame.Y,
                        width: toSize.Width,
                        height: toSize.Height);

                    this.CollectionView?.ReloadData();
                    this.ScrollCollectionViewToInitialPosition();
                    this.View.Hidden = false;

                }
            }, null);
        } 

        public bool IsFirstAppearance = true;

        public virtual void UpdateTitle()
        {
            Title = _photoKitAssetList.Title;
        }

        public virtual void ScrollCollectionView(NSIndexPath indexPath)
        {
            var count = _photoKitAssetList?.Count ?? 0;
            if (count > 0)
            {
                InvokeOnMainThread(() => {
                    this.CollectionView?.ScrollToItem(indexPath, UICollectionViewScrollPosition.Bottom, false);  
                });
            }
        } 

        public virtual void ScrollCollectionViewToInitialPosition()
        {
            if (IsFirstAppearance)
            {
                var indexPath = NSIndexPath.FromItemSection(this._photoKitAssetList.Count - 1, 0);
                this.ScrollCollectionView(indexPath);
                IsFirstAppearance = false;
            }
        }

        #region UICollectionViewDataSource

        //numberOfItemsInSection
        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        { 
            return _photoKitAssetList.Count; 
        }

        #endregion

        #region UICollectionViewDelegate

        // didSelectItemAt
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (_nohanaImagePickerController != null)
            {
                _nohanaImagePickerController.pickerDelegate.NahonaImagePickerDidSelect(_nohanaImagePickerController, _photoKitAssetList[(int)indexPath.Item].OriginalAsset);
            }

            base.ItemSelected(collectionView, indexPath);
        } 

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {

            var cell = CollectionView.DequeueReusableCell("AssetCell", indexPath) as AssetCell;
            if (cell != null)
            { 
                cell.Tag = indexPath.Item;
                cell.Update(_photoKitAssetList[indexPath.Row], _nohanaImagePickerController);

                var imageSize = new CGSize(
                    width: CellSize.Width * UIScreen.MainScreen.Scale,
                    height: CellSize.Width * UIScreen.MainScreen.Scale
                );

                var asset = _photoKitAssetList[(int)indexPath.Item];
                asset.Image(imageSize, (imageData) => 
                {
                    InvokeOnMainThread(() => 
                    {
                        if (cell.Tag == indexPath.Item)
                        {
                            cell.ImageView.Image = imageData.Image;
                        }
                    });
                });

                return (_nohanaImagePickerController.pickerDelegate.NohanaImagePickerList(_nohanaImagePickerController, this, cell, indexPath, asset.OriginalAsset)) ?? cell;

            }
 
            return base.GetCell(collectionView, indexPath);
        }

        #endregion

        #region UICollectionViewDelegateFlowLayout

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            return CellSize;
        }

        #endregion

        #region Storyboard

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            var selectedIndices = CollectionView.GetIndexPathsForSelectedItems();
            if (selectedIndices.Length > 0)
            {
                var selectedIndex = selectedIndices[0];
                if (selectedIndex != null)
                {
                     var assetListDetailViewController = segue.DestinationViewController as AssetDetailListViewController;
                     if (assetListDetailViewController != null)
                     {
                        assetListDetailViewController._photoKitAssetList = _photoKitAssetList;
                        assetListDetailViewController._nohanaImagePickerController = _nohanaImagePickerController;
                        assetListDetailViewController.CurrentIndexPath = selectedIndex;
                                                                                
                     }
                } 
            }
        }

        #endregion

        #region IBAction

        public void DidPushDone(NSObject sender)
        {
            var pickedPhotoKitAsset = _nohanaImagePickerController.PickedAssetList.OfType<PHAsset>().ToList();
            var phAssetList = pickedPhotoKitAsset.Select(x => (x as PHAsset)).ToList();
            _nohanaImagePickerController.pickerDelegate.NahonaImagePicker(_nohanaImagePickerController, phAssetList);

        }

        #endregion
    }
}
