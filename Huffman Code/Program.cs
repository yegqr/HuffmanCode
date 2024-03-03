using System.Text;
using Huffman_Code;

class Program
{ 
    private static void Main()
    {
        var dictionary = new Dictionary< char , int >() ;
        var NumOfChars = 0 ;
        var InitalText = string.Join("", File.ReadLines("C:\\Users\\iskos\\RiderProjects\\Huffman Code\\Huffman Code\\sherlock.txt"));
        foreach ( var key in InitalText )
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = 0 ;
            }
            dictionary[ key ] += 1 ;
        }
        Console.WriteLine(dictionary.Count) ; 
        
        var PQ = new MinHeap() ; 
        foreach ( var keyValuePair in dictionary )
        {
            PQ.Enqueue( new HuffmanTree( keyValuePair.Key , keyValuePair.Value ) , keyValuePair.Value ) ;
        }
        
        while ( PQ.Count() != 1)
        {
            var node1 = PQ.Dequeue() ;
            var node2 = PQ.Dequeue() ;
            PQ.Enqueue(new HuffmanTree(node1._value + node2._value, node1, node2) , node1._value + node2._value) ;
        }

        var FinalTree = PQ.Dequeue();

        var dict = new Dictionary<char, string>() ;
        dict = FinalTree.GetCode(null , dict) ;
        Console.WriteLine(dict.Count);

        foreach (var Pair in dict)
        {
            Console.WriteLine($"{Pair.Key} : {Pair.Value}" );
        }

        var FinalCode = new StringBuilder() ;
        foreach (var character in InitalText)
        {
            FinalCode.Append(dict[character]) ;
        }

        File.Delete("C:\\Users\\iskos\\RiderProjects\\Huffman Code\\Huffman Code\\code.txt") ;
        File.AppendAllText("C:\\Users\\iskos\\RiderProjects\\Huffman Code\\Huffman Code\\code.txt" , FinalCode.ToString() );
        
        var New = dict.ToDictionary(x => x.Value, x => x.Key);
        var previous = false ;
        var temporary = new StringBuilder() ;
        var result = new StringBuilder() ;
        foreach (var character in string.Join("", File.ReadLines("C:\\Users\\iskos\\RiderProjects\\Huffman Code\\Huffman Code\\code.txt")) )
        {
            if (previous )
            {
                result.Append(New[temporary.ToString()]) ;
                previous = false ;
                temporary.Clear().Append(character) ;
            }
            else
            {
                temporary.Append(character) ;
                if (New.TryGetValue(temporary.ToString() , out var c ))
                {
                    previous = true ;
                } 
            }
        }
        Console.WriteLine(result.ToString());
    }
    
}