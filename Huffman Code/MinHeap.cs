namespace Huffman_Code;

public class MinHeap
{
    private int _size ;
    private int _pointer ;
    private int _len ;
    private Tuple<HuffmanTree ,int >[] _array ;

    public MinHeap()
    {
        _size = 16 ;
        _pointer = -1 ; 
        _array = new Tuple<HuffmanTree, int>[_size];
    }
    
    public void Enqueue( HuffmanTree tree , int priority )
    {
        _len++;
        _pointer += 1 ;
        _array[_pointer] = Tuple.Create(tree , priority) ;
        var pointerToSwap = _pointer ;
        var parent = ( int )Math.Round( ( double )pointerToSwap - 1 ) / 2 ;
        while ( pointerToSwap > 0 &&_array[ pointerToSwap ].Item2 >= _array[ parent ].Item2 )
        {
            ( _array[ pointerToSwap ] , _array[ parent ] ) = ( _array[ parent ] , _array[ pointerToSwap ] ) ;
            pointerToSwap = ( int )Math.Round( ( double )pointerToSwap - 1 ) / 2 ;
            if ( pointerToSwap <= 0 ) { break ; }
        }

        if (_pointer >= _size / 2)
        {
            _size *= 4 ;
            Array.Resize( ref _array, _size ) ;
        }
    }

    public HuffmanTree Dequeue()
    {
        _len--;
        var ToReturn = _array[0];
        if (_pointer == -1)
        {
            return ToReturn.Item1 ;
        }
        
        _array[0] = _array[_pointer];
        _pointer-- ;
        
        var parent = 0 ;
        while (true)
        {
            int smallestChild = parent;
            int leftChild = 2 * smallestChild + 1;
            int rightChild = 2 * smallestChild + 2;


            if (leftChild <= _pointer &&
                _array[leftChild].Item2 > _array[smallestChild].Item2)
            {
                smallestChild = rightChild;
            }

            if (rightChild <= _pointer && _array[rightChild].Item2 >
                _array[smallestChild].Item2)
            {
                smallestChild = leftChild;
            }

            if (smallestChild != parent)
            {
                (_array[parent], _array[smallestChild]) = (_array[smallestChild], _array[parent]);

                parent = smallestChild;
            }
            else
            {
                break;
            }
        }

        return ToReturn.Item1 ;
    }

    public int Count()
    {
        return _len;
    }
}