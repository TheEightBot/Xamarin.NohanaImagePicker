using System;
using UIKit;

namespace Xamarin.NohanaImagePicker.Common
{
    public interface IActivityIndicatable
    {
        bool IsProgressing();
        //void UpdateVisibilityOfActivityIndicator(UIView activityIndicator); // moved to extensions 
    }
}
