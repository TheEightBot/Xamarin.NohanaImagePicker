using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace NohanaImagePicker.Xamarin.Common
{
    public struct ImageData
    {
        public UIImage Image { get; set; }
        public NSDictionary<NSObject, NSObject> Info { get; set; }
    }
}
