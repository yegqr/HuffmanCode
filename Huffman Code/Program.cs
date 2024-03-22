using System.Text;
using Huffman_Code;

class Program
{
    private static void Main()
    {
        ;
        while (true)
        {
            var choice = Console.ReadLine();
            if (choice == "stop") break;
            var dictionary = new Dictionary<char, int>();
            var InitalText = string.Join("",
                File.ReadLines("C:\\Users\\iskos\\RiderProjects\\HuffmanCode\\Huffman Code\\sherlock.txt"));
            foreach (var key in InitalText)
            {
                dictionary[key] = (!dictionary.ContainsKey(key)) ? 0 : dictionary[key] + 1;
            }

            var PQ = new MinHeap();
            foreach (var keyValuePair in dictionary)
            {
                PQ.Enqueue(new HuffmanTree(keyValuePair.Key, keyValuePair.Value), keyValuePair.Value);
            }

            while (PQ.Count() != 1)
            {
                var node1 = PQ.Dequeue();
                var node2 = PQ.Dequeue();
                PQ.Enqueue(new HuffmanTree(node1._value + node2._value, node1, node2), node1._value + node2._value);
            }

            var FinalTree = PQ.Dequeue();

            var dict = new Dictionary<char, string>();
            dict = FinalTree.GetCode(null, dict);
            var FinalCode = new StringBuilder();

            var bitsInCoded = 0;
            foreach (var Pair in dict)
            {
                Console.WriteLine($"{Pair.Key} : {Pair.Value}");
                FinalCode.Append($"{Pair.Key} : {Pair.Value} : {Convert.ToInt32(Pair.Value, 2)}\n");
                bitsInCoded += $"{Pair.Key} : {Pair.Value} : {Convert.ToInt32(Pair.Value, 2)}\n".Length;
            }

            FinalCode.Append("#\n");

            foreach (var character in InitalText)
            {
                FinalCode.Append($"{Convert.ToInt32(dict[character], 2)} ");
                bitsInCoded += $"{Convert.ToInt32(dict[character], 2)} ".Length;
            }

            Console.WriteLine($"in the start : {dictionary.Values.Sum()}\n" +
                              $"in the end : {bitsInCoded}");
            

            File.Delete("C:\\Users\\iskos\\RiderProjects\\HuffmanCode\\Huffman Code\\code.txt");
            File.AppendAllText("C:\\Users\\iskos\\RiderProjects\\HuffmanCode\\Huffman Code\\code.txt",
                FinalCode.ToString());
            var DeCode = new Dictionary<string, string>();
            var status = true;
            var temporary = new StringBuilder();
            var result = new StringBuilder();
            var previous = false;

            var toCheck = new HashSet<string>();

            foreach (var row in File.ReadLines("C:\\Users\\iskos\\RiderProjects\\HuffmanCode\\Huffman Code\\code.txt"))
            {
                if (status)
                {
                    if (row == "#")
                    {
                        status = false;
                        continue;
                    }

                    var r = row.Split(" : ");
                    DeCode[r[2]] = r[0];
                    continue;
                }

                foreach (var character in row)
                {
                    if (previous)
                    {
                        result.Append(DeCode[temporary.ToString()]);
                        previous = false;
                        temporary.Clear().Append(character);
                    }
                    else
                    {

                        if (character == ' ')
                        {
                            previous = true;
                            continue;
                        }

                        temporary.Append(character);
                    }
                }
            }

            Console.WriteLine(result.ToString());
        }

    }
}