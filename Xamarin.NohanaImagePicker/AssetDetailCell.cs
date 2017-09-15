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
        UITapGestureRecognizer _doubleTapGestureRecognizer = new UITapGestureRecognizer(); 

        public AssetDetailCell(NSCoder aDecoder) : base(aDecoder)
        {
            _doubleTapGestureRecognizer.AddTarget((x) => DidDoubleTap(x as UITapGestureRecognizer));
            _doubleTapGestureRecognizer.NumberOfTapsRequired = 2; 
		}

        protected internal AssetDetailCell(IntPtr handle) : base(handle)
        {
            _doubleTapGestureRecognizer.AddTarget((x) => DidDoubleTap(x as UITapGestureRecognizer));
            _doubleTapGestureRecognizer.NumberOfTapsRequired = 2; 
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            scrollView.RemoveGestureRecognizer(_doubleTapGestureRecognizer);
            scrollView.AddGestureRecognizer(_doubleTapGestureRecognizer);
        }

        ~AssetDetailCell()
        { 
            scrollView.RemoveGestureRecognizer(_doubleTapGestureRecognizer); 

            scrollView.Dispose();
            _doubleTapGestureRecognizer.Dispose();
        }

        #region UISCrollViewDelegate

        [Export("viewForZoomingInScrollView:")]
        public UIView ViewForZoomingInScrollView(UIScrollView scrollView)
        {
            return imageView;
        } 

        #endregion

        #region Zoom

        void DidDoubleTap(UITapGestureRecognizer sender)
        {
            if (scrollView.ZoomScale < scrollView.MaximumZoomScale)
            {
                var center = sender.LocationInView(imageView);
                scrollView.ZoomToRect(ZoomRect(center), animated: true);
            }
            else
            {
                var defaultScale = (nfloat)1;
                scrollView.SetZoomScale(defaultScale, animated: true);
            }
        }

        CGRect ZoomRect(CGPoint center)
        {
            var size = new CGSize(
                scrollView.Frame.Size.Width / scrollView.MaximumZoomScale, 
                scrollView.Frame.Size.Height / scrollView.MaximumZoomScale
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
