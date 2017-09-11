using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;

namespace NohanaImagePicker.Xamarin.Common
{
    public class ItemList<T> : ICollection<T>
    {
        ICollection<T> _items;

        public ItemList()
        {
            _items = new List<T>();       
        }
		protected ItemList(ICollection<T> collection)
		{ 
			_items = collection;
		}

        public string Title { get; set; }

		public int Count
		{
			get { return _items.Count; }
		} 

        public int Index(int i) {
            return i + 1;
        }

        public void Add(T item)
        {
			_items.Add(item);
		}

        public void Clear()
        {
            _items.Clear();
		}

        public bool Contains(T item)
        {
            return _items.Contains(item);
		}

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
		}

        public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			return _items.Remove(item);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _items.GetEnumerator();
		}
    }
}
