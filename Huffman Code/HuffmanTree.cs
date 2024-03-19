using System.Text;

namespace Huffman_Code
{
    public class HuffmanTree
    {
        private char _character;
        public int _value;
        private HuffmanTree _left;
        private HuffmanTree _right;
        private StringBuilder _num;

        public HuffmanTree(char character, int value)
        {
            _character = character ;
            _value = value;
        }

        public HuffmanTree(int value, HuffmanTree left, HuffmanTree right)
        {
            _value = value;
            _left = left;
            _right = right;
        }

        public Dictionary<char , string> GetCode(StringBuilder previous , Dictionary<char , string> dict)
        {
            if (_left != null)
            {
                _left._num = (previous == null) ? new StringBuilder() : new StringBuilder(previous.ToString());
                _left._num.Append(0);
                dict =_left.GetCode(_left._num , dict) ;
            }
            
            if (_right != null)
            {
                _right._num = (previous == null) ? new StringBuilder() : new StringBuilder(previous.ToString());
                _right._num.Append(1);
                dict = _right.GetCode(_right._num , dict ) ;
            }

            if (_character != '\0' ) 
            {
                dict[_character] = $"1{_num}";
            }
            return dict;
        }
    }
}