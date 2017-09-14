using System;
using UIKit;

namespace Xamarin.NohanaImagePicker.Common
{
    public interface IEmptyIndicatable
    {
        bool IsEmpty();
        //void UpdateVisibilityOfEmptyIndicator(UIView emptyIndicator); // moved to extensions
    }
}
