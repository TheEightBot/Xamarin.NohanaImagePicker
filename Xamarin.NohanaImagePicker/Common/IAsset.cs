using System;
using CoreGraphics;

namespace Xamarin.NohanaImagePicker.Common
{
    public interface IAsset
    {
        int Identifier { get; } 
        void Image(CGSize targetSize, Action<ImageData> handler);

    }
}
