using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;

namespace NohanaImagePicker.Xamarin.Common
{
    public interface IItemList
    {
        string Title { get; }

        void Update(Action handler);

        int Index(int i);

        //subscript (index: Int) -> Item { get } // TODO:
	}
}

 