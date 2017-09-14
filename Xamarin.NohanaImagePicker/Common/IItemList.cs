using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;

namespace Xamarin.NohanaImagePicker.Common
{
    public interface IItemList
    {
        string Title { get; }

        void Update(Action handler);

        int Index(int i);

        //subscript (index: Int) -> Item { get } // note: this is ported at the class level
	}
}

 