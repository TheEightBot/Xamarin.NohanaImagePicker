using System;
using CoreGraphics;
using Foundation;
using Xamarin.NohanaImagePicker.Photos;
using Xamarin.NohanaImagePicker;
using UIKit;
using static Xamarin.NohanaImagePicker.Common.NotificationInfo;

namespace Xamarin.NohanaImagePicker
{
    public partial class AssetDetailListViewController : AssetListViewController, IUICollectionViewDelegateFlowLayout, IUIScrollViewDelegate
    {
        public bool IsFinishedLoading { get; set; }

        NSIndexPath _currentIndexPath;
        public NSIndexPath CurrentIndexPath { 
            get
            {  
                return _currentIndexPath; 
            }
            set 
            {
                if (_currentIndexPath != value)
                    DidChangeAssetDetailPage(value);
                    
                _currentIndexPath = value;
            }
        }

        protected internal AssetDetailListViewController(IntPtr handle) : base(handle)
        {
        }

        public override CGSize CellSize
        {
            get
            {
                return Common.Size.ScreenRectWithoutAppBar(this).Size;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (NohanaImagePickerController != null)
            {
                var droppedImage = NohanaImagePickerController.Conf.Image.DroppedSmall ?? UIImage.FromBundle("btn_select_m", NohanaImagePickerController.AssetBundle, null);
                var pickedImage = NohanaImagePickerController.Conf.Image.DroppedSmall ?? UIImage.FromBundle("btn_selected_m", NohanaImagePickerController.AssetBundle, null);

                pickButton.SetImage(droppedImage, UIControlState.Normal);
                pickButton.SetImage(pickedImage, UIControlState.Selected);
                
                pickButton.TouchUpInside -= PickButton_TouchUpInside;
                pickButton.TouchUpInside += PickButton_TouchUpInside;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            IsFinishedLoading = true;
        }
 

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            base.ViewWillTransitionToSize(toSize, coordinator);
            var indexPath = CurrentIndexPath;
            View.Hidden = true;
            coordinator.AnimateAlongsideTransition(null, (_) =>
            {
                this.View.InvalidateIntrinsicContentSize();
                this.CollectionView?.ReloadData();
                this.ScrollCollectionView(indexPath);
                this.View.Hidden = false; 
            }); 
        }

        public override void UpdateTitle()
        {
            this.Title = string.Empty;
        }

        void DidChangeAssetDetailPage(NSIndexPath indexPath)
        {
            if (NohanaImagePickerController != null && PhotoKitAssetList != null)
            {
                var asset = PhotoKitAssetList[(int)indexPath.Item];
                this.pickButton.Selected = NohanaImagePickerController.PickedAssetList.IsPicked(asset);
                this.pickButton.Hidden = !(NohanaImagePickerController.CanPickAsset(asset));
                NohanaImagePickerController.PickerDelegate?.NohanaImagePickerDidChange(NohanaImagePickerController, this, indexPath, asset.OriginalAsset);
            }
        }

        public override void ScrollCollectionView(NSIndexPath indexPath)
        {
            var count = PhotoKitAssetList.Count;
            if (count > 0)
            {
                InvokeOnMainThread(() =>
                {
                    var toIndexPath = NSIndexPath.FromItemSection(indexPath.Item, 0);
                    this.CollectionView.ScrollToItem(toIndexPath, UICollectionViewScrollPosition.CenteredHorizontally, false);
                });
            }
        }

        public override void ScrollCollectionViewToInitialPosition()
        {
            if (!IsFirstAppearance) return;

            var indexPath = NSIndexPath.FromItemSection(CurrentIndexPath.Item, 0);
            ScrollCollectionView(indexPath);
            IsFirstAppearance = false;
        }

        #region IBAction
        
        public void DidPushPickButton()
        {
            var asset = PhotoKitAssetList[CurrentIndexPath.Row];
            if (pickButton.Selected)
            {
                if (NohanaImagePickerController.PickedAssetList.Drop(asset))
                {
                    pickButton.Selected = false;
                }
            }
            else
            {
                if (NohanaImagePickerController.PickedAssetList.Pick(asset))
                {
                    pickButton.Selected = true;
                }
            }
        }

        #endregion

        #region UICollectionViewDelegate

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (NohanaImagePickerController == null)
                throw new Exception("failed to dequeueReusableCellWithIdentifier(\"AssetDetailCell\")");

            var cell = collectionView.DequeueReusableCell("AssetDetailCell", CurrentIndexPath) as AssetDetailCell;
            if (cell != null)
            {
                cell.scrollView.ZoomScale = 1;
                cell.Tag = indexPath.Item;

                var imageSize = new CGSize(
                    width: CellSize.Width * UIScreen.MainScreen.Scale,
                    height: CellSize.Width * UIScreen.MainScreen.Scale
                );
                var asset = PhotoKitAssetList[(int)indexPath.Item];
                cell.Tag = indexPath.Item;
                
                asset.Image(imageSize, imageData =>
                {
                    InvokeOnMainThread(() =>
                    {
                        if (cell.Tag == indexPath.Item)
                        {
                            cell.imageView.Image = imageData.Image;
                            cell.imageViewHeightConstraint.Constant = this.CellSize.Height;
                            cell.imageViewWidthConstraint.Constant = this.CellSize.Width;
                        }
                    });
                });

                return (NohanaImagePickerController.PickerDelegate?.NohanaImagePickerList(NohanaImagePickerController, this, cell, indexPath, asset.OriginalAsset)) ?? cell;
            }

            return base.GetCell(collectionView, indexPath);
        }

		#endregion

		#region UIScrollViewDelegate

        public override void Scrolled(UIScrollView scrollView)
        {
            if (CollectionView != null && IsFinishedLoading)
            {
                var row = (int)(CollectionView.ContentOffset.X + CellSize.Width * 0.5) / CellSize.Width;
                if (row < 0)
                {
                    CurrentIndexPath = NSIndexPath.FromRowSection(0, CurrentIndexPath.Section);
                }
                else if ((int)row >= CollectionView.NumberOfItemsInSection(0))
                {
                    CurrentIndexPath = NSIndexPath.FromRowSection(CollectionView.NumberOfItemsInSection(0) - 1, CurrentIndexPath.Section);
                }
                else
                { 
                    CurrentIndexPath = NSIndexPath.FromRowSection(2, CurrentIndexPath.Section);
                }
            }


        }

		#endregion

		#region UICollectionViewDelegateFlowLayout

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            return CellSize;
        }

		#endregion
        
        void PickButton_TouchUpInside(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("fires");
            DidPushPickButton();
        }
	}
}
