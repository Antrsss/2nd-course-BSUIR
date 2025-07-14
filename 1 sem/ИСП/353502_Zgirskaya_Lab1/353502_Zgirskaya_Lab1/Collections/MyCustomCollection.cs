using _353502_Zgirskaya_Lab1.Interfaces;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab1.Collections
{
    internal class MyCustomCollection<T> : ICustomCollection<T>, IEnumerable<T>
    {
        private CollectionElement _firstElement;

        private CollectionElement _currentElement;

        private CollectionElement _lastElement;

        private int _currentElementPosition;

        private int _count;

        public MyCustomCollection()
        {
            _firstElement = null;
            _currentElement = null;
            _lastElement = null;
            _currentElementPosition = 0;
            _count = 0;
        }

        public T this[int index] 
        {   
            get 
            {
                if (index < 0 || index >= _count)
                {
                    throw new IndexOutOfRangeException();
                }

                CollectionElement indexedEl = _firstElement;

                for (int i = 1; i <= index; ++i)
                {
                    indexedEl = indexedEl.NextEl;
                }
                return indexedEl.Data;
            }
            set
            {
                if (index < 0 || index >= _count)
                {
                    throw new IndexOutOfRangeException();
                }

                CollectionElement indexedEl = _firstElement;

                for (int i = 1; i <= index; ++i)
                {
                    indexedEl = indexedEl.NextEl;
                }
                indexedEl.Data = value;
            }
        }

        public int Count
        {
            get { return _count; }
        }

        public void Add(T item)
        {
            CollectionElement newElement = new CollectionElement(item);
            if (_count == 0)
            {
                _firstElement = newElement;
                _lastElement = _firstElement;
            }
            else
            {
                newElement.PreviousEl = _lastElement;
                _lastElement.NextEl = newElement;
                _lastElement = newElement;
            }
            _currentElement = newElement;
            _count++;
        }

        public T Current()
        {
            return _currentElement.Data;
        }

        public void Next()
        {
            if (_currentElement.NextEl != null)
            {
                _currentElement = _currentElement.NextEl;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public void Remove(T item)
        {
            CollectionElement currentEl = _firstElement;
            CollectionElement currentElement = _currentElement;

            for (int i = 0; i <_count; ++i)
            {
                if (currentEl.Data.Equals(item))
                {
                    _currentElement = currentEl;
                    RemoveCurrent();
                    _currentElement = currentElement;
                    return;
                }
                else
                {
                    currentEl = currentEl.NextEl;
                }
            }

            throw new Entities.ItemDoesNotExistException();
        }

        public T RemoveCurrent()
        {
            T removedEl = _currentElement.Data;
            if (_currentElement.PreviousEl != null && _currentElement.NextEl != null)
            {
                _currentElement.PreviousEl.NextEl = _currentElement.NextEl;
                _currentElement.NextEl.PreviousEl = _currentElement.PreviousEl;
                _currentElement = _currentElement.NextEl;
            }
            else if (_currentElement.PreviousEl != null)
            {
                _currentElement.PreviousEl.NextEl = null;
                _lastElement = _currentElement.PreviousEl;
                _currentElement = _lastElement;
            }
            else if (_currentElement.NextEl != null)
            {
                _currentElement.NextEl.PreviousEl = null;
                _firstElement = _currentElement.NextEl;
                _currentElement = _firstElement;
            }
            else
            {
                _firstElement = null;
                _lastElement = null;
                _currentElement = null;
            }
            _count--;
            return removedEl;
        }

        public void Reset()
        {
            _currentElement = _firstElement;
        }

        private class CollectionElement
        {
            public T Data { get; set; }

            public CollectionElement PreviousEl;

            public CollectionElement NextEl;

            public CollectionElement(T data)
            {
                Data = data;
                PreviousEl = null;
                NextEl = null;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            CollectionElement current = _firstElement;
            while (current != null)
            {
                yield return current.Data;
                current = current.NextEl;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
