using System;
using CoreGraphics;
using Foundation;
using Xamarin.NohanaImagePicker.Common;
using Xamarin.NohanaImagePicker.Photos;
using Xamarin.NohanaImagePicker.Views;
using UIKit;

namespace Xamarin.NohanaImagePicker.ViewControllers
{
    public class MomentViewController : AssetListViewController, IActivityIndicatable
    {
        public MomentViewController()
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
            if (_nohanaImagePickerController != null)
                Title = NSString.LocalizedFormat("albums.moment.title", "NohanaImagePicker", _nohanaImagePickerController.AssetBundle, string.Empty);
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
                    var indexPath = NSIndexPath.FromItemSection(MomentAlbumList[lastSection].Count, lastSection);
                    ScrollCollectionView(indexPath);
                    IsFirstAppearance = false;
                }
            }
        }

        #region UICollectionViewDataSource

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            if (ActivityIndicator != null)
                UpdateVisibilityOfActivityIndicator(ActivityIndicator);

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

            if (_nohanaImagePickerController == null)
                throw new Exception("failed to dequeueReusableCellWithIdentifier(\"AssetCell\")");

            var cell = collectionView.DequeueReusableCell("AssetCell", indexPath) as AssetCell;
            if (cell != null)
            {
                var asset = MomentAlbumList[indexPath.Section][indexPath.Row];
                cell.Tag = indexPath.Item;
                cell.Update(asset: asset, nohanaImagePickerController: _nohanaImagePickerController);

                var imageSize = new CGSize(
                    width: CellSize.Width * UIScreen.MainScreen.Scale,
                    height: CellSize.Width * UIScreen.MainScreen.Scale
                );
                // TODO: aj
                asset.Image(imageSize, ImageData =>
                {
                    InvokeOnMainThread(() =>
                    {
                        if (cell.Tag == indexPath.Item)
                        {
                            cell.ImageView.Image = ImageData.Image;
                        }
                    });
                });

                return (_nohanaImagePickerController.pickerDelegate.NohanaImagePickerList(_nohanaImagePickerController, this, cell, indexPath, asset.OriginalAsset)) ?? cell;
            }


            return base.GetCell(collectionView, indexPath);
        }


        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            if (elementKind == UICollectionElementKindSectionKey.Header)
            {
                var album = MomentAlbumList[indexPath.Section];
                var header = CollectionView.DequeueReusableSupplementaryView(elementKind, "MomentHeader", indexPath) as MomentSectionHeaderView;
                if (header != null)
                {
                    header.LocationLabel.Text = album.Title;
                    if (album.Date != null)
                    {
                        var formatter = new NSDateFormatter();
                        formatter.DateStyle = NSDateFormatterStyle.Long;
                        formatter.TimeStyle = NSDateFormatterStyle.None;
                        header.DateLabel.Text = formatter.StringFor(album.Date);
                    }
                    else
                    {
                        header.DateLabel.Text = string.Empty;
                    }
                }
                else
                {
                    throw new Exception("failed to create MomentHeader");
                }
            }

            return base.GetViewForSupplementaryElement(collectionView, elementKind, indexPath);
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

        public void UpdateVisibilityOfActivityIndicator(UIView activityIndicator)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UICollectionViewDelegate

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (_nohanaImagePickerController != null)
            {
                _nohanaImagePickerController.pickerDelegate.NahonaImagePickerDidSelect(_nohanaImagePickerController, MomentAlbumList[indexPath.Section][indexPath.Row].OriginalAsset);
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

            }


            base.PrepareForSegue(segue, sender);
        }

        #endregion
    }
}
