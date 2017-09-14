using System;
using CoreGraphics;
using Foundation;
using Xamarin.NohanaImagePicker.Photos;
using Xamarin.NohanaImagePicker.Views;
using UIKit;
using static Xamarin.NohanaImagePicker.Common.NotificationInfo;

namespace Xamarin.NohanaImagePicker.ViewControllers
{
    public class AssetDetailListViewController : AssetListViewController, IUICollectionViewDelegateFlowLayout, IUIScrollViewDelegate
    {
        //var currentIndexPath: IndexPath = IndexPath()
        //{
        //	willSet {
        //		if currentIndexPath != newValue {
        //			didChangeAssetDetailPage(newValue)

        //	}
        //	}
        //}  ^^^^^^ this is swift, is below converted right
        
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

        public UIButton PickButton;

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
            if (_nohanaImagePickerController != null)
            {
                var droppedImage = _nohanaImagePickerController.Conf.Image.DroppedSmall ?? UIImage.FromBundle("btn_select_m", _nohanaImagePickerController.AssetBundle, null);
                var pickedImage = _nohanaImagePickerController.Conf.Image.DroppedSmall ?? UIImage.FromBundle("btn_selected_m", _nohanaImagePickerController.AssetBundle, null);

                PickButton.SetImage(droppedImage, new UIControlState());
                PickButton.SetImage(pickedImage, UIControlState.Selected);
            }

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
            if (_nohanaImagePickerController != null)
            {
                var asset = _photoKitAssetList[(int)indexPath.Item];
                this.PickButton.Selected = _nohanaImagePickerController.PickedAssetList.IsPicked(asset);
                this.PickButton.Hidden = !(_nohanaImagePickerController.CanPickAsset(asset));
                _nohanaImagePickerController.pickerDelegate.NohanaImagePickerDidChange(_nohanaImagePickerController, this, indexPath, asset.OriginalAsset);

            }
        }

        public override void ScrollCollectionView(NSIndexPath indexPath)
        {
            var count = _photoKitAssetList.Count;
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
            if (IsFirstAppearance) return;

            var indexPath = NSIndexPath.FromItemSection(CurrentIndexPath.Item, 0);
            ScrollCollectionView(indexPath);
            IsFirstAppearance = false;
        }

        #region IBAction

        public void DidPushPickButton(UIButton sender)
        {
            var asset = _photoKitAssetList[CurrentIndexPath.Row];
            if (PickButton.Selected)
            {
                if (_nohanaImagePickerController.PickedAssetList.Drop(asset))
                {
                    PickButton.Selected = false;
                }
            }
            else
            {
                if (_nohanaImagePickerController.PickedAssetList.Drop(asset))
                {
                    PickButton.Selected = true;
                }
            }
        }

        #endregion

        #region UICollectionViewDelegate

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (_nohanaImagePickerController == null)
                throw new Exception("failed to dequeueReusableCellWithIdentifier(\"AssetDetailCell\")");

            var cell = collectionView.DequeueReusableCell("AssetDetailCell", indexPath) as AssetDetailCell;
            if (cell != null)
            {
                cell.ScrollView.ZoomScale = 1;
                cell.Tag = indexPath.Item;

                var imageSize = new CGSize(
                    width: CellSize.Width * UIScreen.MainScreen.Scale,
                    height: CellSize.Width * UIScreen.MainScreen.Scale
                );
                var asset = _photoKitAssetList[(int)indexPath.Item];
                cell.Tag = indexPath.Item;
                // TODO: aj
                asset.Image(imageSize, imageData =>
                {
                    InvokeOnMainThread(() =>
                    {
                        if (cell.Tag == indexPath.Item)
                        {
                            cell.ImageView.Image = imageData.Image;
                            cell.ImageViewHeightContraint.Constant = this.CellSize.Height;
                            cell.ImageViewWidthConstraint.Constant = this.CellSize.Width;
                        }
                    });
                });

                return (_nohanaImagePickerController.pickerDelegate.NohanaImagePickerList(_nohanaImagePickerController, this, cell, indexPath, asset.OriginalAsset)) ?? cell;
            }

            return base.GetCell(collectionView, indexPath);
        }

		#endregion

		#region UIScrollViewDelegate

        public override void Scrolled(UIScrollView scrollView)
        {
            if (CollectionView != null)
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
                    CurrentIndexPath = NSIndexPath.FromRowSection((int)row, CurrentIndexPath.Section);
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
	}
}
