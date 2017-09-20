using System;
using Foundation;
using Xamarin.NohanaImagePicker.Photos;
using UIKit;
using Xamarin.NohanaImagePicker.Extensions;
using CoreGraphics;
using Xamarin.NohanaImagePicker;
using Photos;
using System.Linq;
using Xamarin.NohanaImagePicker.Common;

namespace Xamarin.NohanaImagePicker
{
    public partial class AssetListViewController : UICollectionViewController, IUICollectionViewDelegateFlowLayout
    {

        public NohanaImagePickerController NohanaImagePickerController;
        public PhotoKitAssetList PhotoKitAssetList;

        public AssetListViewController()
        {
        }

        protected internal AssetListViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = NohanaImagePickerController?.Conf.Color.Background ?? UIColor.White;
            UpdateTitle();
            this.SetUpToolbarItems();
            this.AddPickPhotoKitAssetNotificationObservers();
        }

        public virtual CGSize CellSize
        {
            get
            {
                if (NohanaImagePickerController == null)
                {
                    return CGSize.Empty;
                }
                var numberOfColumns = NohanaImagePickerController.NumberOfColumnsInLandscape;
                if (UIInterfaceOrientation.Portrait.IsPortrait())
                {
                    numberOfColumns = NohanaImagePickerController.NumberOfColumnsInPortrait;
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

            if (NohanaImagePickerController != null)
            {
                this.SetToolbarTitle(NohanaImagePickerController);
            }
            this.CollectionView?.ReloadData();
            this.ScrollCollectionViewToInitialPosition();
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            base.ViewWillTransitionToSize(toSize, coordinator);
            
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
            Title = PhotoKitAssetList.Title;
        }

        public virtual void ScrollCollectionView(NSIndexPath indexPath)
        {
            var count = PhotoKitAssetList?.Count ?? 0;
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
                var indexPath = NSIndexPath.FromItemSection(this.PhotoKitAssetList.Count - 1, 0);
                this.ScrollCollectionView(indexPath);
                IsFirstAppearance = false;
            }
        }

        #region UICollectionViewDataSource

        //numberOfItemsInSection
        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        { 
            return PhotoKitAssetList.Count; 
        }

        #endregion

        #region UICollectionViewDelegate
        
        [Export("collectionView:didSelectItemAtIndexPath:")]
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (NohanaImagePickerController != null && PhotoKitAssetList != null)
            {
                NohanaImagePickerController.PickerDelegate?.NahonaImagePickerDidSelect(NohanaImagePickerController, PhotoKitAssetList[(int)indexPath.Item].OriginalAsset);
            }
        } 
        
        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {

            var cell = CollectionView.DequeueReusableCell("AssetCell", indexPath) as AssetCell;
            if (cell != null)
            { 
                cell.Tag = indexPath.Item;
                cell.Update(PhotoKitAssetList[indexPath.Row], NohanaImagePickerController);

                var imageSize = new CGSize(
                    width: CellSize.Width * UIScreen.MainScreen.Scale,
                    height: CellSize.Width * UIScreen.MainScreen.Scale
                );

                var asset = PhotoKitAssetList[(int)indexPath.Item];
                asset.Image(imageSize, (Action<Common.ImageData>)((imageData) => 
                {
                    InvokeOnMainThread((Action)(() => 
                    {
                        if (cell.Tag == indexPath.Item)
                        {
                            cell.imageView.Image = imageData.Image;
                        }
                    }));
                }));

                return (NohanaImagePickerController.PickerDelegate?.NohanaImagePickerList(NohanaImagePickerController, this, cell, indexPath, asset.OriginalAsset)) ?? cell;

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
                        assetListDetailViewController.PhotoKitAssetList = PhotoKitAssetList;
                        assetListDetailViewController.NohanaImagePickerController = NohanaImagePickerController;
                        assetListDetailViewController.CurrentIndexPath = selectedIndex;
                     }
                } 
            }
        }

        #endregion

        #region IBAction

        [Action("didPushDone:")]
        public void didPushDone(NSObject sender)
        {
            var phAssetList = 
                NohanaImagePickerController?.PickedAssetList?
                    .Where(x => x is PhotoKitAsset)?
                    .Select(x => (x as PhotoKitAsset).OriginalAsset)?
                    .ToList();
            NohanaImagePickerController.PickerDelegate?.NahonaImagesPicked(NohanaImagePickerController, phAssetList);
        }

        #endregion
    }
}
