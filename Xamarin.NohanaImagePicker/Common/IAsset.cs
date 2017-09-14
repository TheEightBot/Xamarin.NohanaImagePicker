using System;
using CoreGraphics;

namespace NohanaImagePicker.Xamarin.Common
{
    public interface IAsset
    {
        int Identifier { get; } 
        void Image(CGSize targetSize, Action<ImageData> handler);

    }
}
