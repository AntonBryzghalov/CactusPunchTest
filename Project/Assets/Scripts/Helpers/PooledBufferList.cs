using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;

namespace TowerDefence.Helpers
{
    public sealed class PooledBufferList<T> : IReadOnlyList<T>, IDisposable
    {
        private readonly bool _isValueType = typeof(T).IsValueType;
        private T[] _array;
        private int _count;

        public int Count => _count;
        public T this[int index] => index < _count ? _array[index] : throw new ArgumentOutOfRangeException(nameof(index));

        public PooledBufferList<T> Rent(int minCapacity)
        {
            _array = ArrayPool<T>.Shared.Rent(minCapacity);
            _count = 0;
            return this;
        }

        public void CopyFrom(IReadOnlyList<T> source)
        {
            EnsureCapacity(source.Count);
            for (int i = 0; i < source.Count; i++)
                _array[i] = source[i];
            _count = source.Count;
        }

        public void CopyFrom(List<T> source)
        {
            EnsureCapacity(source.Count);
            source.CopyTo(_array, 0);
            _count = source.Count;
        }

        public void ReturnToPool()
        {
            if (_array != null)
            {
                ArrayPool<T>.Shared.Return(_array, clearArray: true);
                _array = null;
            }
            _count = 0;
        }

        public void Dispose()
        {
            ReturnToPool();
        }

        private void EnsureCapacity(int requiredCapacity)
        {
            if (_array == null || _array.Length < requiredCapacity)
            {
                ReturnToPool();
                _array = ArrayPool<T>.Shared.Rent(requiredCapacity);
            }
            else if (!_isValueType && _count >= requiredCapacity)
            {
                for (int i = requiredCapacity; i < _array.Length; i++)
                {
                    _array[i] = default;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                yield return _array[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}