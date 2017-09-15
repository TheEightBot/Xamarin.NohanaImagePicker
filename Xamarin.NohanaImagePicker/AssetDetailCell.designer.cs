// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;
using UIKit;

namespace Xamarin.NohanaImagePicker
{
	[Register ("AssetDetailCell")]
	partial class AssetDetailCell
	{
        [Outlet]
		public UIScrollView scrollView { get; set; }

        [Outlet]
        public UIImageView imageView { get; set; }

        [Outlet]
        public NSLayoutConstraint imageViewHeightConstraint { get; set; }

        [Outlet]
        public NSLayoutConstraint imageViewWidthConstraint { get; set; }
        
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
