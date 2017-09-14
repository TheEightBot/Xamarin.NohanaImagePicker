using System;
using UIKit;

namespace NohanaImagePicker.Xamarin.Common
{
    public interface IActivityIndicatable
    {
        bool IsProgressing();
        //void UpdateVisibilityOfActivityIndicator(UIView activityIndicator); in extensions
    }
}
