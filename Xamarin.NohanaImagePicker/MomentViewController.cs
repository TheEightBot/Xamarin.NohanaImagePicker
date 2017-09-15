using System;
using CoreGraphics;
using Foundation;
using Xamarin.NohanaImagePicker.Common;
using Xamarin.NohanaImagePicker.Photos;
using Xamarin.NohanaImagePicker;
using UIKit;
using Xamarin.NohanaImagePicker.Extensions;

namespace Xamarin.NohanaImagePicker
{
    public partial class MomentViewController : AssetListViewController, IActivityIndicatable
    {
        public MomentViewController()
        {
        }

        protected internal MomentViewController(IntPtr handle) : base(handle)
        {
        }

        public PhotoKitAlbumList MomentAlbumList;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.SetUpActivityIndicator();
        }

        public override void UpdateTitle()
        {
            if (NohanaImagePickerController != null)
                Title = NSString.LocalizedFormat("Moment");
        }

        public override void ScrollCollectionView(NSIndexPath indexPath)
        {
            var count = MomentAlbumList.Count;
            if (count > 0)
                InvokeOnMainThread(() =>
                {
                    CollectionView.ScrollToItem(indexPath, UICollectionViewScrollPosition.Bottom, false);
                });
        }

        public override void ScrollCollectionViewToInitialPosition()
        {
            if (IsFirstAppearance)
            {
                var lastSection = MomentAlbumList.Count - 1;
                if (lastSection >= 0)
                {
                    var indexPath = NSIndexPath.FromItemSection(MomentAlbumList[lastSection].Count - 1, lastSection);
                    ScrollCollectionView(indexPath);
                    IsFirstAppearance = false;
                }
            }
        }

        #region UICollectionViewDataSource

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            if (ActivityIndicator != null)
                this.UpdateVisibilityOfActivityIndicator(ActivityIndicator);

            return MomentAlbumList.Count;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return MomentAlbumList[(int)section].Count;
        }

        #endregion

        #region UICollectionViewDelegate

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {

            if (NohanaImagePickerController == null)
                throw new Exception("failed to dequeueReusableCellWithIdentifier(\"AssetCell\")");

            var cell = collectionView.DequeueReusableCell("AssetCell", indexPath) as AssetCell;
            if (cell != null)
            {
                var asset = MomentAlbumList[indexPath.Section][indexPath.Row];
                cell.Tag = indexPath.Item;
                cell.Update(asset: asset, nohanaImagePickerController: NohanaImagePickerController);

                var imageSize = new CGSize(
                    width: CellSize.Width * UIScreen.MainScreen.Scale,
                    height: CellSize.Width * UIScreen.MainScreen.Scale
                );
                // TODO: aj
                asset.Image(imageSize, (Action<ImageData>)(ImageData =>
                {
                    InvokeOnMainThread((Action)(() =>
                    {
                        if (cell.Tag == indexPath.Item)
                        {
                            cell.imageView.Image = ImageData.Image;
                        }
                    }));
                }));

                return (NohanaImagePickerController.pickerDelegate?.NohanaImagePickerList(NohanaImagePickerController, this, cell, indexPath, asset.OriginalAsset)) ?? cell;
            }


            return base.GetCell(collectionView, indexPath);
        }

        [Export("collectionView:viewForSupplementaryElementOfKind:atIndexPath:")]
        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            if (elementKind == UICollectionElementKindSectionKey.Header)
            {
                var album = MomentAlbumList[indexPath.Section];
                var header = CollectionView.DequeueReusableSupplementaryView(elementKind, "MomentHeader", indexPath) as MomentSectionHeaderView;
                if (header != null)
                {
                    header.locationLabel.Text = album.Title;
                    if (album.Date != null)
                    {
                        var formatter = new NSDateFormatter();
                        formatter.DateStyle = NSDateFormatterStyle.Long;
                        formatter.TimeStyle = NSDateFormatterStyle.None;
                        header.dateLabel.Text = formatter.StringFor(album.Date);
                    }
                    else
                    {
                        header.dateLabel.Text = string.Empty;
                    }
                    return header;
                }
                else
                {
                    throw new Exception("failed to create MomentHeader");
                }
            }
            return null;

            //return base.GetViewForSupplementaryElement(collectionView, elementKind, indexPath);
        }

        #endregion 

        #region ActivityIndicatable

        public UIActivityIndicatorView ActivityIndicator;

        public bool IsLoading = false;

        public void SetUpActivityIndicator()
        {
            ActivityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
            var screenRect = Size.ScreenRectWithoutAppBar(this);
            ActivityIndicator.Center = new CGPoint(screenRect.Size.Width / 2, screenRect.Size.Height / 2);
            ActivityIndicator.StartAnimating();
        }

        public bool IsEmpty()
        {
            if (IsProgressing())
                return false;

            return MomentAlbumList.Count == 0;
        }

        public bool IsProgressing()
        {
            return IsLoading;
        }

        #endregion

        #region UICollectionViewDelegate

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (NohanaImagePickerController != null)
            {
                NohanaImagePickerController.pickerDelegate?.NahonaImagePickerDidSelect(NohanaImagePickerController, MomentAlbumList[indexPath.Section][indexPath.Row].OriginalAsset);
            }
            base.ItemSelected(collectionView, indexPath);
        }

        #endregion


        #region Storyboard

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            var indexPaths = CollectionView.GetIndexPathsForSelectedItems();
            if (indexPaths.Length > 0)
            {
                var selectedIndexPath = indexPaths[0];
                var assetListDetailViewController = segue.DestinationViewController as AssetDetailListViewController;
                assetListDetailViewController.PhotoKitAssetList = MomentAlbumList[selectedIndexPath.Section];
                assetListDetailViewController.NohanaImagePickerController = NohanaImagePickerController;
                assetListDetailViewController.CurrentIndexPath = selectedIndexPath;
            }


            //base.PrepareForSegue(segue, sender);
        }

        #endregion
    }
}
