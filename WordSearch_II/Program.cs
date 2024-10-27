using System.Text;

int m = int.Parse(Console.ReadLine());
int n = int.Parse(Console.ReadLine());
char[][] board = new char[m][];
for (int i = 0; i < m; ++i) {
    board[i] = new char[n];
    var strs = Console.ReadLine().Split();
    for (int j = 0; j < n; ++j) 
        board[i][j] = char.Parse(strs[j]);
}

string[] words = Console.ReadLine().Split();

foreach(var word in new Solution().FindWords(board, words))
    Console.WriteLine(word);

public struct Point {
    public int x;
    public int y;
    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }
}

public class Trie {
    public TrieNode root = new();
    public Trie(LinkedList<string> words) {
        foreach (var word in words)
            AddWord(word);
    }

    public void AddWord(string word) {
        var current = root;
        foreach (var ch in word) {
            var children = current.Children;
            if (children.TryGetValue(ch, out TrieNode child))
                current = child;
            else {
                current = new();
                children[ch] = current;
            }
        }
        current.isWordNode = true;
    }
}

public class TrieNode {
    public bool isWordNode = false;
    public bool isOnBoard = false;
    public bool allChildrenOnBoard = false;
    public Dictionary<char, TrieNode> Children = new();
}

public class Solution {
    public List<Point> visited = new();
    public List<string> answer = new();
    public HashSet<string> reversed = new();
    public LinkedList<string> words;
    public Trie trie;
    public char[][] board;
    int n, m;

    public string ReverseString(string str) {
        return new string(str.Reverse().ToArray());
    }

    public void GetWordsOnBoard(StringBuilder word, TrieNode node) {
        foreach (var child in node.Children) {
            word.Append(child.Key);
            if (child.Value.isOnBoard) {
                string wordStr = word.ToString();
                if (reversed.Contains(wordStr))
                    answer.Add(ReverseString(wordStr));
                else
                    answer.Add(wordStr);
            }
            GetWordsOnBoard(word, child.Value);
            word.Remove(word.Length - 1, 1);
        }
    }

    public void CheckPosition(Point point, TrieNode trieNode, int chPosition) {
        if (trieNode.Children.Count == 0 || trieNode.allChildrenOnBoard)
            return;

        int i = point.x, j = point.y;
        char ch = board[i][j];
        if (!trieNode.Children.TryGetValue(ch, out TrieNode nextNode))
            return;

        var CheckIfNodeHasAllChildrenOnBoard = (TrieNode node) => {
            node.allChildrenOnBoard = true;
            foreach (var child in node.Children)
                if (!child.Value.allChildrenOnBoard) {
                    node.allChildrenOnBoard = false;
                    break;
                }
        };

        if (nextNode.isWordNode) {
            nextNode.isOnBoard = true;
            CheckIfNodeHasAllChildrenOnBoard(nextNode);
        }

        visited.Add(point);
        int nextPosition = chPosition + 1;

        var CheckNextPosition = (Point p) => {
            if (!visited.Contains(p))
                CheckPosition(p, nextNode, nextPosition);
        };

        if (i > 0)
            CheckNextPosition(new(i - 1, j));
        if (i < m - 1)
            CheckNextPosition(new(i + 1, j));
        if (j > 0)
            CheckNextPosition(new(i, j - 1));
        if (j < n - 1)
            CheckNextPosition(new(i, j + 1));
        visited.Remove(point);
        CheckIfNodeHasAllChildrenOnBoard(trieNode);
    }

    public void SiftWords() {
        var countChar = (Dictionary<char, int> dict, char ch) => {
            if (dict.ContainsKey(ch))
                dict[ch]++;
            else
                dict[ch] = 1;
        };

        Dictionary<char, int> boardCharFrequency = new();
        for (int i = 0; i < m; ++i)
            for (int j = 0; j < n; ++j) {
                countChar(boardCharFrequency, board[i][j]);
            }

        Dictionary<char, int> wordCharFrequency = new();
        var current = words.First;
        while (current != null) {
            var next = current.Next;
            string word = current.Value;
            foreach (var ch in word)
                countChar(wordCharFrequency, ch);
            bool isRemoved = false;
            foreach (var ch in wordCharFrequency.Keys) {
                if (boardCharFrequency.TryGetValue(ch, out int frequency)
                    && frequency >= wordCharFrequency[ch])
                    continue;
                words.Remove(current);
                isRemoved = true;
                break;
            }
            if (!isRemoved &&
                wordCharFrequency[word[0]] > wordCharFrequency[word[word.Length - 1]]) {
                string reverse = ReverseString(word);
                words.AddAfter(current, reverse);
                words.Remove(current);
                reversed.Add(reverse);
            }
            wordCharFrequency.Clear();
            current = next;
        }
    }

    public IList<string> FindWords(char[][] _board, string[] _words) {
        board = _board;
        words = new(_words);
        m = _board.Length;
        n = _board[0].Length;

        SiftWords();

        trie = new(words);

        for (int i = 0; i < m && !trie.root.allChildrenOnBoard; ++i)
            for (int j = 0; j < n && !trie.root.allChildrenOnBoard; ++j)
                CheckPosition(new Point(i, j), trie.root, 0);

        GetWordsOnBoard(new StringBuilder(), trie.root);

        return answer;
    }
}