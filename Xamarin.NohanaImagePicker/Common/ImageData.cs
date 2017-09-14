using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace Xamarin.NohanaImagePicker.Common
{
    public struct ImageData
    {
        public UIImage Image { get; set; }
        public NSDictionary<NSObject, NSObject> Info { get; set; }
    }
}
