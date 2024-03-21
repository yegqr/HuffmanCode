using System.Collections;
using System.Text;
using Huffman_Code;

class Program
{
    private static void Main()
    {
        while (true)
        {
            var choice = Console.ReadLine();
            if (choice == "stop") break;
            var dictionary = new Dictionary<char, int>();
            var InitalText = string.Join("",
                File.ReadAllText("C:\\Users\\iskos\\RiderProjects\\HuffmanCode\\Huffman Code\\sherlock.txt"));
            foreach (var key in InitalText)
            {
                dictionary[key] = !dictionary.ContainsKey(key) ? 0 : dictionary[key] + 1;
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

            foreach (var pair in dict)
            {
                Console.WriteLine($"{pair.Key} : {pair.Value}");
            }

            using (var fs = new FileStream("C:\\Users\\iskos\\RiderProjects\\HuffmanCode\\Huffman Code\\code.bin",
                       FileMode.Create))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    // Write the number of entries in the dictionary
                    bw.Write(dict.Count);

                    // Write each entry (character and its code) bit by bit
                    foreach (var pair in dict)
                    {
                        // Write code length as byte (8 bits)
                        bw.Write((byte)pair.Value.Length);
                        
                        // Write character as char (16 bits)
                        bw.Write(pair.Key);

                        // Write code as a sequence of bits
                        foreach (var bit in pair.Value)
                        {
                            bw.Write(bit == '1');
                        }

                    }

                    foreach (var character in InitalText)
                    {
                        foreach (var code in dict[character])
                        {
                            var value = code == '1';
                            bw.Write(value);
                        }
                    }

                    // second part reading and decdoing part

                    fs.Seek(0, SeekOrigin.Begin);
                   using (var binaryReader = new BinaryReader(fs))
                    {
                        byte[] binaryData = binaryReader.ReadBytes((int)fs.Length);
                        var decodingDict = new Dictionary<string, char>();
                        int len_of_dict = 0;
                        var temp = new StringBuilder();
                        char currentChar = '\0';
                        int currentLen = 0;
                        var switcher = true;

                        foreach (var b in binaryData)
                        {
                            string binaryString = Convert.ToString(b, 2).PadLeft(8, '0');
                            temp.Append(binaryString);
                        }

                        var t = new StringBuilder();
                        foreach (var character in temp.ToString())
                        {
                            t.Append(character) ; 
                            if (len_of_dict == 0 )
                            {
                                len_of_dict = t.Length == 8 ? Convert.ToInt32(t.ToString(), 2) : 0 ;
                                if (len_of_dict != 0)
                                {
                                    t.Clear();
                                }
                            }
                            else if (decodingDict.Count < len_of_dict)
                            {
                                if (currentLen == 0)
                                {
                                    if (t.Length == 8)
                                    currentLen = Convert.ToChar(Convert.ToInt32(temp.ToString().Substring(temp.Length - 8, 8), 2));
                                    temp.Remove(temp.Length - 8, 8);
                                }
                                else if (switcher)
                                {
                                    switcher = false;
                                    currentChar =
                                        Convert.ToChar(Convert.ToInt32(temp.ToString().Substring(temp.Length - 8, 8), 2));
                                    temp.Remove(temp.Length - 8, 8);
                                }
                                else
                                {
                                    
                                    {
                                        decodingDict[temp.Substring(0, currentLen)] = currentChar;
                                        temp = temp.Substring(currentLen);
                                        currentChar = '\0';
                                        currentLen = 0;
                                        switcher = true;
                                    }
                                }
                            }
                        }

                        Console.WriteLine(temp);

                        var FINAL = "";
                        // var queue = "";
                        // var previous = false;
                        // foreach (var tf in temp)
                        // {
                        //     if (previous)
                        //     {
                        //         FINAL += decodingDict[queue];
                        //         previous = false;
                        //         queue = "";
                        //         queue += tf;
                        //     }
                        //     else
                        //     {
                        //         queue += tf;
                        //         if (decodingDict.TryGetValue(queue, out var c))
                        //         {
                        //             previous = true;
                        //         }
                        //     }
                        // }
                        //
                        // Console.WriteLine(FINAL);
                    }
                }
            }
        }
    }
}