using System;
using CoreGraphics;
using Foundation;
using Xamarin.NohanaImagePicker.ViewControllers;
using UIKit;

namespace Xamarin.NohanaImagePicker.Views
{
    // DONE
    public class AlbumListEmptyIndicator : UILabel
    {
        public AlbumListEmptyIndicator(string message, string description, CGRect frame, NohanaImagePickerController.Config config) : base(frame)
        {
            var centerStyle = new NSMutableParagraphStyle();
            centerStyle.Alignment = UITextAlignment.Center;

            var messageAttributes = new NSDictionary<NSString, NSObject>();
            messageAttributes.SetValueForKey(config.Color.Empty ?? new UIColor(red: 0x88 / 0xff, green: 0x88 / 0xff, blue: 0x88 / 0xff, alpha: 1), UIStringAttributeKey.ForegroundColor);
            messageAttributes.SetValueForKey(UIFont.SystemFontOfSize(14), UIStringAttributeKey.Font);
            messageAttributes.SetValueForKey(centerStyle, UIStringAttributeKey.ParagraphStyle);

            var messageText = new NSAttributedString(message, messageAttributes);

            var descriptionAttributes = new NSDictionary<NSString, NSObject>();
            descriptionAttributes.SetValueForKey(config.Color.Empty ?? new UIColor(red: 0x88 / 0xff, green: 0x88 / 0xff, blue: 0x88 / 0xff, alpha: 1), UIStringAttributeKey.ForegroundColor);
			descriptionAttributes.SetValueForKey(UIFont.SystemFontOfSize(14), UIStringAttributeKey.Font);
			descriptionAttributes.SetValueForKey(centerStyle, UIStringAttributeKey.ParagraphStyle);

            var descriptionText = new NSAttributedString(description, descriptionAttributes);

            var attributedTextAttributes = new NSDictionary<NSString, NSObject>();
            attributedTextAttributes.SetValueForKey(UIFont.SystemFontOfSize(14), UIStringAttributeKey.Font);

            var attributedText = new NSMutableAttributedString();
            attributedText.Append(messageText);
            attributedText.Append(new NSAttributedString("\n\n", attributedTextAttributes));
            attributedText.Append(descriptionText);

            this.Lines = 0;
            this.AttributedText = attributedText;
        }

        public AlbumListEmptyIndicator(NSCoder aDecoder) : base(aDecoder)
        {
            throw new NotImplementedException();
        }
    }
}
