using System;
using CoreGraphics;
using Foundation;
using Xamarin.NohanaImagePicker;
using UIKit;

namespace Xamarin.NohanaImagePicker
{
    // DONE
    public partial class AlbumListEmptyIndicator : UILabel
    {
        public AlbumListEmptyIndicator(string message, string description, CGRect frame, NohanaImagePickerController.Config config) : base(frame)
        {
            var centerStyle = new NSMutableParagraphStyle();
            centerStyle.Alignment = UITextAlignment.Center;

            var messageAttributes = new UIStringAttributes
            {
                ForegroundColor = config.Color.Empty ?? new UIColor(red: 0x88 / 0xff, green: 0x88 / 0xff, blue: 0x88 / 0xff, alpha: 1),
                Font = UIFont.SystemFontOfSize(26),
                ParagraphStyle = centerStyle
            };

            var messageText = new NSAttributedString(message, messageAttributes);

            var descriptionAttributes = new UIStringAttributes
            {
                ForegroundColor = config.Color.Empty ?? new UIColor(red: 0x88 / 0xff, green: 0x88 / 0xff, blue: 0x88 / 0xff, alpha: 1),
                Font = UIFont.SystemFontOfSize(14),
                ParagraphStyle = centerStyle
            };

            var descriptionText = new NSAttributedString(description, descriptionAttributes);

            var attributedTextAttributes = new UIStringAttributes
            {
                Font = UIFont.SystemFontOfSize(14)
            };

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
