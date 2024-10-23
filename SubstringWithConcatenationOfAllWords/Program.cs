int n = int.Parse(Console.ReadLine());
string s = Console.ReadLine();
string[] words = Console.ReadLine().Split();

foreach(int indice in new Solution().FindSubstring(s, words)) {
    Console.WriteLine(indice);
}

public class Solution {
    const int wrongWordIndex = -1;
    int wordsInSubstring;
    int maxWords;
    int wordLength;
    Dictionary<string, int> wordToIndex = new();
    Dictionary<int, WordCount> wordsCount = new();
    LinkedList<int> wordWindow = new();
    List<int> indices = new();

    class WordCount {
        public int count = 0;
        public int maxCount = 1;
    }

    public void AddWord(string word) {
        int index;
        if (!wordToIndex.TryGetValue(word, out index)) {
            wordWindow.AddLast(wrongWordIndex);
            return;
        }
        var wordInfo = wordsCount[index];
        if (wordInfo.count < wordInfo.maxCount)
            wordsInSubstring++;
        wordInfo.count++;
        wordWindow.AddLast(index);
    }

    public void RemoveWord() {
        int index = wordWindow.First.Value;
        wordWindow.RemoveFirst();
        if (index == wrongWordIndex)
            return;
        var wordInfo = wordsCount[index];
        wordInfo.count--;
        if (wordInfo.count < wordInfo.maxCount)
            wordsInSubstring--;
    }

    public void Check(int start) {
        if (wordsInSubstring == maxWords)
            indices.Add(start);
    }

    public void SearchFromPosition(int start, string s) {
        int end = start;
        foreach (var word in wordsCount)
            word.Value.count = 0;
        wordsInSubstring = 0;
        wordWindow.Clear();
        for (; end < maxWords * wordLength && end <= s.Length - wordLength; end += wordLength) {
            string word = s.Substring(end, wordLength);
            AddWord(word);
        }
        Check(start);
        for (; end <= s.Length - wordLength;) {
            string word = s.Substring(end, wordLength);
            AddWord(word);
            end += wordLength;
            RemoveWord();
            start += wordLength;
            Check(start);
        }
    }

    public IList<int> FindSubstring(string s, string[] words) {
        int ind = 0;
        foreach (var word in words) {
            int prevInd;
            if (wordToIndex.TryGetValue(word, out prevInd)) {
                wordsCount[prevInd].maxCount++;
            } else {
                wordToIndex[word] = ind;
                wordsCount[ind] = new();
                ind++;
            }
        }
        wordLength = words[0].Length;
        maxWords = words.Length;
        for (int start = 0; start < wordLength; ++start) {
            SearchFromPosition(start, s);
        }
        return indices;
    }
}