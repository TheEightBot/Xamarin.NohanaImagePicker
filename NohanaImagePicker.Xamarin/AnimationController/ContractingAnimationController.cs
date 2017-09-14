using System;
using Foundation;
using NohanaImagePicker.Xamarin.Common;
using NohanaImagePicker.Xamarin.ViewControllers;
using NohanaImagePicker.Xamarin.Views;
using UIKit;

namespace NohanaImagePicker.Xamarin.AnimationController
{
    public class ContractingAnimationController : NSObject, IUIViewControllerAnimatedTransitioning
    {

        AssetDetailCell FromCell;

        public ContractingAnimationController(AssetDetailCell fromCell)
        {
            this.FromCell = fromCell;
        }

		public double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
		{
			return 0.3;
		}

        public void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
			var fromVC = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey) as AssetDetailListViewController;
			var toVC = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey) as AssetListViewController;
            var toCell = toVC.CollectionView.CellForItem(fromVC.CurrentIndexPath) as AssetCell;

			if (fromVC == null || toVC == null || toCell == null) return;
 
			var contractingImageView = new UIImageView(FromCell.ImageView.Image);
            contractingImageView.ContentMode = toCell.ImageView.ContentMode;
			contractingImageView.ClipsToBounds = true;
            contractingImageView.Frame = Size.ContractingAnimationFromCellRect(fromVC, FromCell, contractingImageView.Image.Size);

			transitionContext.ContainerView.AddSubview(toVC.View);
			transitionContext.ContainerView.AddSubview(contractingImageView);
			toVC.View.Alpha = 0; 
            toCell.Alpha = 0;
            FromCell.Alpha = 0;


			UIView.Animate(
				duration: TransitionDuration(transitionContext),
				delay: 0,
                options: new UIViewAnimationOptions(),
				animation: () =>
				{
					toVC.View.Alpha = 1;
                    contractingImageView.Frame = Size.ContractingAnimationToCellRect(toVC, toCell);
				},
				completion: () =>
				{
					this.FromCell.Alpha = 1;
                    toCell.Alpha = 1;
                    contractingImageView.RemoveFromSuperview();
					transitionContext.CompleteTransition(!transitionContext.TransitionWasCancelled); 
				});
        } 
    }
}
