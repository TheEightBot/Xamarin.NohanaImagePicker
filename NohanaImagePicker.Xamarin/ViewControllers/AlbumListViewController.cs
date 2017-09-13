using System;
using UIKit;

namespace NohanaImagePicker.Xamarin.ViewControllers
{
    public class AlbumListViewController : UICollectionViewController
    {

		enum AlbumListViewControllerSectionType
		{
			Moment, Albums
		}

		int albumListViewControllerSectionTypeCount = Enum.GetNames(typeof(AlbumListViewControllerSectionType)).Length;


		public AlbumListViewController()
        {
        }
    }
}
