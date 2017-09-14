using System;
using Foundation;

namespace NohanaImagePicker.Xamarin.Common
{
    public struct NotificationInfo
    { 
        public struct Asset 
        {
            public struct PhotoKit
            {
                public static readonly NSString DidPick = (Foundation.NSString)NSNotification.FromName("jp.co.nohana.NotificationName.Asset.PhotoKit.didPick", null).Name;
			
               public static readonly string DidPickUserInfoKeyAsset = "asset";
			    
               public static readonly string DidPickUserInfoKeyPickedAssetsCount = "pickedAssetsCount";
			    
               public static readonly NSString DidDrop = (Foundation.NSString)NSNotification.FromName("jp.co.nohana.NotificationName.Asset.PhotoKit.didDrop", null).Name; 
			    
               public static readonly string DidDropUserInfoKeyAsset = "asset";
			    
               public static readonly string DidDropUserInfoKeyPickedAssetsCount = "pickedAssetsCount";
                
            }
        }
    }
}
