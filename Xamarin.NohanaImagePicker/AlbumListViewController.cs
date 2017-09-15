using System;
using Xamarin.NohanaImagePicker.Common;
using Xamarin.NohanaImagePicker.Photos;
using UIKit;
using Foundation;
using Xamarin.NohanaImagePicker.Extensions;
using CoreGraphics;
using Xamarin.NohanaImagePicker;
using Photos;
using System.Collections.Generic;

namespace Xamarin.NohanaImagePicker
{
    public partial class AlbumListViewController : UITableViewController, IEmptyIndicatable, IActivityIndicatable
    {

		enum AlbumListViewControllerSectionType
		{
			Moment, Albums
		}

		int albumListViewControllerSectionTypeCount = Enum.GetNames(typeof(AlbumListViewControllerSectionType)).Length;


        public NohanaImagePickerController NohanaImagePickerController;
        public PhotoKitAlbumList PhotoKitAlbumList;

		public AlbumListViewController()
        {
            
        }

        public AlbumListViewController(NSCoder coder) : base(coder)
        {
        }

        protected internal AlbumListViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var nohanaImagePickerController = NohanaImagePickerController;
            if (nohanaImagePickerController != null)
            {
                View.BackgroundColor = nohanaImagePickerController.Conf.Color.Background ?? UIColor.White;
                Title = nohanaImagePickerController.Conf.Strings.AlbumListTitle ?? NSString.LocalizedFormat("Photos", "NohanaImagePicker", nohanaImagePickerController, string.Empty);
                this.SetUpToolbarItems();
                NavigationController.SetToolbarHidden(nohanaImagePickerController.ToolbarHidden, false); 
            }
            SetUpEmptyIndicator();
            SetUpActivityIndicator();
        }

        ~AlbumListViewController()
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(this);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var nohanaImagePickerController = NohanaImagePickerController;
            if (nohanaImagePickerController != null)
            { 
                this.SetToolbarTitle(nohanaImagePickerController);
            }
            var indexPathForSelectedRow = this.TableView.IndexPathForSelectedRow;
            if (indexPathForSelectedRow != null)
            { 
                TableView.DeselectRow(indexPathForSelectedRow, true);
            }
        }

        public override void ViewWillTransitionToSize(CoreGraphics.CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            base.ViewWillTransitionToSize(toSize, coordinator);
            TableView.ReloadData();
        }

		#region UITableViewDataSource
       
        public override nint NumberOfSections(UITableView tableView)
        {
            return albumListViewControllerSectionTypeCount;
        }

		public override nint RowsInSection(UITableView tableView, nint section)
        {  
            if (EmptyIndicator != null) 
                this.UpdateVisibilityOfEmptyIndicator(EmptyIndicator);
      
            if (ActivityIndicator != null) 
                this.UpdateVisibilityOfActivityIndicator(ActivityIndicator); 

            if ((section + 1) > albumListViewControllerSectionTypeCount)
                throw new Exception("wrong section type selected");

            var sectionType = (AlbumListViewControllerSectionType)Enum.ToObject(typeof(AlbumListViewControllerSectionType), (int)section);

            switch (sectionType)
            { 
                case AlbumListViewControllerSectionType.Moment:
                    if (NohanaImagePickerController != null) 
                        return NohanaImagePickerController.ShouldShowMoment ? 1 : 0; 
                    return 0;
                case AlbumListViewControllerSectionType.Albums: 
                    return this.PhotoKitAlbumList.Count; 
                default:
                    break;
            }
 
            return base.RowsInSection(tableView, section); 
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            if ((indexPath.Section + 1) > albumListViewControllerSectionTypeCount)
				throw new Exception("bad section type selected");

            var sectionType = (AlbumListViewControllerSectionType)Enum.ToObject(typeof(AlbumListViewControllerSectionType), indexPath.Section);

            switch (sectionType)
            { 
                case AlbumListViewControllerSectionType.Moment:
                    var cell = TableView.DequeueReusableCell("MomentAlbumCell") as MomentCell;
                    if (cell == null)
                        throw new Exception("failed to dequeueReusableCellWithIdentifier(\"MomentAlbumCell\")");
                    if (NohanaImagePickerController != null)
                    {
                        cell.Config = NohanaImagePickerController.Conf;
                        cell.titleLabel.Text = NohanaImagePickerController.Conf.Strings.AlbumListMomentTitle ?? NSString.LocalizedFormat("Moment", "NohanaImagePicker", NohanaImagePickerController.AssetBundle, string.Empty); 
                    }
                    return cell;
                case AlbumListViewControllerSectionType.Albums:
					var albumCell = TableView.DequeueReusableCell("AlbumCell") as AlbumCell;
					if (albumCell == null)
						throw new Exception("failed to dequeueReusableCellWithIdentifier(\"AlbumCell\")");
                    var albumList = PhotoKitAlbumList[indexPath.Row];
                    albumCell.titleLabel.Text = albumList.Title;
                    albumCell.Tag = indexPath.Row;
                    var imageSize = new CGSize(
                        width: albumCell.thumbnailImageView.Frame.Size.Width * UIScreen.MainScreen.Scale,
                        height: albumCell.thumbnailImageView.Frame.Size.Width * UIScreen.MainScreen.Scale
                    );
                    var albumCount = albumList.Count;
                    if (albumCount > 0)
                    {
                        var lastAsset = albumList[albumCount - 1];
                        
                        // TODO: aj check
                        lastAsset.Image(imageSize, (imageData) =>
                        {
                            InvokeOnMainThread(() =>
                            {
                                if (albumCell.Tag == indexPath.Row)
                                    albumCell.thumbnailImageView.Image = imageData.Image;
                            });
                        });
                    }
                    else
                    {
                        albumCell.thumbnailImageView.Image = null;
                    }
                    return albumCell;
            }


            return base.GetCell(tableView, indexPath);
        }

        #endregion

        #region Storyboard

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            var index = TableView.IndexPathForSelectedRow.Section;
			if ((index + 1) > albumListViewControllerSectionTypeCount)
				throw new Exception("bad section type selected");

			var sectionType = (AlbumListViewControllerSectionType)Enum.ToObject(typeof(AlbumListViewControllerSectionType), index);

            switch (sectionType)
            { 
                case AlbumListViewControllerSectionType.Moment:
                    var momentViewController = segue.DestinationViewController as MomentViewController;
                    if (momentViewController == null)
                        return;
                        
                    momentViewController.NohanaImagePickerController = NohanaImagePickerController;
                    momentViewController.MomentAlbumList = new PhotoKitAlbumList(
                        new List<PHAssetCollectionType> { PHAssetCollectionType.Moment },
                        new List<PHAssetCollectionSubtype> { PHAssetCollectionSubtype.Any },
                        NohanaImagePickerController.MediaType,
                        NohanaImagePickerController.ShouldShowEmptyAlbum,
                        () =>
                        {
                            InvokeOnMainThread(() =>
                            {
                                if (momentViewController == null)
                                    return;

                                momentViewController.IsLoading = false;
                                momentViewController.CollectionView?.ReloadData();
                                momentViewController.IsFirstAppearance = true;
                                momentViewController.ScrollCollectionViewToInitialPosition();
                            });
                        });
                    break;
                    
                case AlbumListViewControllerSectionType.Albums:
                    var assetListViewController = segue.DestinationViewController as AssetListViewController;
                    if (assetListViewController == null)
                        return;
                        
                    assetListViewController.PhotoKitAssetList = PhotoKitAlbumList[TableView.IndexPathForSelectedRow.Row];
                    assetListViewController.NohanaImagePickerController = NohanaImagePickerController;
                    break;
            }

            base.PrepareForSegue(segue, sender);
        }

        #endregion


        #region EmptyIndicatable

        UIView EmptyIndicator;

        void SetUpEmptyIndicator()
        {
            var frame = new CGRect(CGPoint.Empty, Size.ScreenRectWithoutAppBar(this).Size);
            var nohanaImagePickerController = NohanaImagePickerController;
            if (nohanaImagePickerController != null)
            {
                EmptyIndicator = new AlbumListEmptyIndicator(
                    message: nohanaImagePickerController.Conf.Strings.AlbumListEmptyMessage ?? NSString.LocalizedFormat("albumlist.empty.message","NohanaImagePicker", nohanaImagePickerController.AssetBundle, string.Empty),
                    description: nohanaImagePickerController.Conf.Strings.AlbumListEmptyDescription ?? NSString.LocalizedFormat("albumlist.empty.description", "NohanaImagePicker", nohanaImagePickerController.AssetBundle, string.Empty),
                    frame: frame,
                    config: nohanaImagePickerController.Conf
                );      
            } 
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
      
            return this.PhotoKitAlbumList.Count == 0;
        } 

		#endregion
 
        public bool IsProgressing()
        {
            return IsLoading;
        }
 
    }
}
