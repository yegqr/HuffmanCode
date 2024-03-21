using System;

namespace Huffman_Code
{
    public class MinHeap
    {
        private int _size;
        private int _len;
        private Tuple<HuffmanTree, int>[] _array;

        public MinHeap()
        {
            _size = 16;
            _len = 0;
            _array = new Tuple<HuffmanTree, int>[_size];
        }

        public void Enqueue(HuffmanTree tree, int priority)
        {
            _len++;
            int pointerToSwap = _len - 1;
            _array[pointerToSwap] = Tuple.Create(tree, priority);
            while (pointerToSwap > 0 && _array[pointerToSwap].Item2 < _array[(pointerToSwap - 1) / 2].Item2)
            {
                ( _array[pointerToSwap], _array[(pointerToSwap - 1) / 2]) = ( _array[(pointerToSwap - 1) / 2], _array[pointerToSwap]);
                pointerToSwap = (pointerToSwap - 1) / 2;
            }

            if (_len >= _size)
            {
                _size *= 2;
                Array.Resize(ref _array, _size);
            }
        }

        public HuffmanTree Dequeue()
        {
            if (_len == 0)
                throw new InvalidOperationException("Heap is empty");

            HuffmanTree toReturn = _array[0].Item1;
            _len--;
            _array[0] = _array[_len];
            int parent = 0;

            while (true)
            {
                int smallestChild = parent;
                int leftChild = 2 * parent + 1;
                int rightChild = 2 * parent + 2;

                if (leftChild < _len && _array[leftChild].Item2 < _array[smallestChild].Item2)
                {
                    smallestChild = leftChild;
                }

                if (rightChild < _len && _array[rightChild].Item2 < _array[smallestChild].Item2)
                {
                    smallestChild = rightChild;
                }

                if (smallestChild != parent)
                {
                    ( _array[parent], _array[smallestChild]) = ( _array[smallestChild], _array[parent]);
                    parent = smallestChild;
                }
                else
                {
                    break;
                }
            }

            return toReturn;
        }

        public int Count()
        {
            return _len;
        }
    }
}
