using System;
using Foundation;
using Newtonsoft.Json;
using UIKit; 
using CoreGraphics;

namespace Xamarin.NohanaImagePicker
{
    // DONE
    public partial class AssetDetailCell : UICollectionViewCell, IUIScrollViewDelegate
    {
        public UIScrollView ScrollView { get; set; }

        public UIImageView ImageView { get; set; }

        public NSLayoutConstraint ImageViewHeightContraint { get; set; }

        public NSLayoutConstraint ImageViewWidthConstraint { get; set; }

        UITapGestureRecognizer _doubleTapGestureRecognizer = new UITapGestureRecognizer(); 

        public AssetDetailCell(NSCoder aDecoder) : base(aDecoder)
        {
            _doubleTapGestureRecognizer.AddTarget(this, new ObjCRuntime.Selector("DidDoubleTap:"));
            _doubleTapGestureRecognizer.NumberOfTapsRequired = 2; 
		}

        protected internal AssetDetailCell(IntPtr handle) : base(handle)
        {
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            ScrollView.RemoveGestureRecognizer(_doubleTapGestureRecognizer);
            ScrollView.AddGestureRecognizer(_doubleTapGestureRecognizer);
        }

        ~AssetDetailCell()
        { 
            ScrollView.RemoveGestureRecognizer(_doubleTapGestureRecognizer); 

            ScrollView.Dispose();
            _doubleTapGestureRecognizer.Dispose();
        }

        #region UISCrollViewDelegate

        [Export("viewForZoomingInScrollView:")]
        public UIView ViewForZoomingInScrollView(UIScrollView scrollView)
        {
            return ImageView;
        } 

        #endregion

        #region Zoom

        void DidDoubleTap(UITapGestureRecognizer sender)
        {
            if (ScrollView.ZoomScale < ScrollView.MaximumZoomScale)
            {
                var center = sender.LocationInView(ImageView);
                ScrollView.ZoomToRect(ZoomRect(center), animated: true);
            }
            else
            {
                var defaultScale = (nfloat)1;
                ScrollView.SetZoomScale(defaultScale, animated: true);
            }
        }

        CGRect ZoomRect(CGPoint center)
        {
            var size = new CGSize(
                ScrollView.Frame.Size.Width / ScrollView.MaximumZoomScale, 
                ScrollView.Frame.Size.Height / ScrollView.MaximumZoomScale
            );

            var point = new CGPoint(
                center.X - size.Width / 2.0, 
                center.Y - size.Height / 2.0
            );
     
            return new CGRect(point, size); 
        }

        #endregion
    }
}
