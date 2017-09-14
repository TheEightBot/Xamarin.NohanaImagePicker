using System;
using UIKit;
using Foundation;
namespace NohanaImagePicker.Xamarin.AnimationController
{ 
    public class SwipeInteractionController : UIPercentDrivenInteractiveTransition
    {
        UIViewController viewController; 

        public void AttachToViewController(UIViewController viewController)
        { 
            var count = viewController.NavigationController?.ViewControllers.Length ?? 0;
            if (count > 1)
            {
                var target = viewController.NavigationController?.ValueForKey(new NSString("_cachedInteractionController"));
                var gesture = new UIScreenEdgePanGestureRecognizer(target, new ObjCRuntime.Selector("handleNavigationTransition:"));
                gesture.Edges = UIRectEdge.Left;
                viewController.View.AddGestureRecognizer(gesture);

            }
        }
    }
}
