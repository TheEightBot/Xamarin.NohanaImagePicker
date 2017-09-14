using System;
using Foundation;
using NohanaImagePicker.Xamarin.ViewControllers;
using NohanaImagePicker.Xamarin.Views;
using UIKit;

namespace NohanaImagePicker.Xamarin.AnimationController
{
    public class AnimatableNavigationController : UINavigationController, IUINavigationControllerDelegate
    {
        public AnimatableNavigationController()
        {
        }

        SwipeInteractionController _swipeInteractionController;

        public AnimatableNavigationController(NSCoder aDecoder) : base(aDecoder)
        {
            this.Delegate = this; 
        }


        [Export("navigationController:animationControllerForOperation:fromViewController:toViewController:")]
        public IUIViewControllerAnimatedTransitioning GetAnimationControllerForOperation(UINavigationController navigationController, UINavigationControllerOperation operation, UIViewController fromViewController, UIViewController toViewController)
        {
            switch (operation)
            { 
                case UINavigationControllerOperation.Push:
                    var fromVC = fromViewController as AssetListViewController;
                    if (fromVC != null)
                    {
                        var selectedIndices = fromVC.CollectionView.IndexPathsForVisibleItems;
                        if (selectedIndices.Length > 0)
                        {
                            var selectedIndex = selectedIndices[0];
                            var fromCell = fromVC.CollectionView.CellForItem(selectedIndex) as AssetCell;
                            if (fromCell != null)
                            {
                                return new ExpandingAnimationController(fromCell);
                            }
                        } 
                    } 
                    break;
                case UINavigationControllerOperation.Pop:
                    var fromDetailVC = fromViewController as AssetDetailListViewController;
                    if (fromDetailVC != null)
                    {
                        var indexPath = NSIndexPath.FromItemSection(fromDetailVC.CurrentIndexPath.Item,0);
                        var fromCell = fromDetailVC.CollectionView.CellForItem(indexPath) as AssetDetailCell;
                        if (fromCell != null)
                        {
                            return new ContractingAnimationController(fromCell);
                        }
                    
                    }

                    break;
                default:
                    return null;
            }

            return null;
        } 
    }
}
