using System;
using UIKit;

namespace NohanaImagePicker.Xamarin.Common
{
    public interface IEmptyIndicatable
    {
        bool IsEmpty();
        //void UpdateVisibilityOfEmptyIndicator(UIView emptyIndicator); // moved to extensions
    }
}
