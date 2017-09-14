using System;
using Foundation;
using NohanaImagePicker.Xamarin.Views;
using UIKit;
using AVFoundation;
using NohanaImagePicker.Xamarin.ViewControllers;
using NohanaImagePicker.Xamarin.Common;

namespace NohanaImagePicker.Xamarin.AnimationController
{
    public class ExpandingAnimationController : NSObject, IUIViewControllerAnimatedTransitioning
    {
        AssetCell fromCell;

        public ExpandingAnimationController(AssetCell fromCell)
        {
            this.fromCell = fromCell;
        }

        public double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return 0.3;
        }

        public void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var fromVC = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey) as AssetListViewController;
            var toVC = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey) as AssetDetailListViewController;

            if (fromVC == null || toVC == null) return;

            var expandingImageView = new UIImageView(fromCell.ImageView.Image);
            expandingImageView.ContentMode = fromCell.ImageView.ContentMode;
            expandingImageView.ClipsToBounds = true;
            expandingImageView.Frame = Size.ExpandingAnimationFromCellRect(fromVC, fromCell);

            transitionContext.ContainerView.AddSubview(toVC.View);
            transitionContext.ContainerView.AddSubview(expandingImageView);
            toVC.View.Alpha = 0;
            toVC.CollectionView.Hidden = true;
            toVC.View.BackgroundColor = UIColor.Black;
            fromCell.Alpha = 0;

            UIView.Animate(
                duration: TransitionDuration(transitionContext),
                delay: 2,
                options: UIViewAnimationOptions.CurveEaseOut,
                animation: () =>
                {
                    toVC.View.Alpha = 1;
                    expandingImageView.Frame = Size.ExpandingAnimationToCellRect(fromVC, expandingImageView.Image.Size);
                },
                completion: () =>
                {
                    this.fromCell.Alpha = 1;
                    toVC.CollectionView.Hidden = false;
                    expandingImageView.RemoveFromSuperview();
                    transitionContext.CompleteTransition(!transitionContext.TransitionWasCancelled);

                });
            
            
                
        }
    }
}
