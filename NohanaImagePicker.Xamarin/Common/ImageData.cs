using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace NohanaImagePicker.Xamarin.Common
{
    public struct ImageData
    {
        public UIImage Image { get; set; }
        public Dictionary<NSObject, object> Info { get; set; }
    }
}
