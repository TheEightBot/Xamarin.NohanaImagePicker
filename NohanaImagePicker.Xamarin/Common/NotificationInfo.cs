using System;
using Foundation;

namespace NohanaImagePicker.Xamarin.Common
{
    public struct NotificationInfo
    { 
        struct Asset 
        {
            struct PhotoKit
            {
                static readonly string DidPick = "jp.co.nohana.NotificationName.Asset.PhotoKit.didPick"; //;Notification.Name("jp.co.nohana.NotificationName.Asset.PhotoKit.didPick");

                static readonly string DidPickUserInfoKeyAsset = "asset";

                static readonly string DidPickUserInfoKeyPickedAssetsCount = "pickedAssetsCount";

                static readonly string DidDrop = "jp.co.nohana.NotificationName.Asset.PhotoKit.didDrop"; //Notification.Name("jp.co.nohana.NotificationName.Asset.PhotoKit.didDrop");

                static readonly string DidDropUserInfoKeyAsset = "asset";

                static readonly string DidDropUserInfoKeyPickedAssetsCount = "pickedAssetsCount";
                
            }
        }
    }
}
