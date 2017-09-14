using System;
using Foundation;
using NohanaImagePicker.Xamarin.Photos;
using UIKit;
using NohanaImagePicker.Xamarin.Extensions;
using CoreGraphics;

namespace NohanaImagePicker.Xamarin.ViewControllers
{
    public class AssetListViewController : UICollectionViewController
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
                
            }

            base.ItemSelected(collectionView, indexPath);
        } 

        #endregion
    }
}
