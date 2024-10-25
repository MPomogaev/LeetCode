string beginWord = Console.ReadLine();
string endWord = Console.ReadLine();
IList<string> wordList = Console.ReadLine().Split();

Console.WriteLine(new Solution().LadderLength(beginWord, endWord, wordList));

public class Solution {
    static int CountDifferences(string str1, string str2) {
        int differenceCount = 0;
        for (int i = 0; i < str1.Length; i++) {
            if (str1[i] != str2[i]) {
                differenceCount++;
                if (differenceCount > 1) {
                    break;
                }
            }
        }
        return differenceCount;
    }

    public int LadderLength(string beginWord, string endWord, IList<string> wordList) {
        if (!wordList.Contains(endWord))
            return 0;
        int count = 1;
        LinkedList<string> linkedWordList = new(wordList);
        List<string> layer = new() { beginWord };
        List<string> nextLayer = new();
        while(linkedWordList.Count != 0 && layer.Count != 0) {
            var current = linkedWordList.First;
            while (current != null) {
                var next = current.Next; 
                foreach(var layerNode in layer)
                    if (CountDifferences(current.Value, layerNode) == 1) {
                        if (current.Value == endWord)
                            return count + 1;
                        nextLayer.Add(current.Value);
                        linkedWordList.Remove(current);
                        break;
                    }
                current = next;
            }
            layer.Clear();
            layer.AddRange(nextLayer);
            nextLayer.Clear();
            count += 1;
        }
        return 0;
    }
}