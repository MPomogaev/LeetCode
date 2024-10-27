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

public class Solution {
    public List<Point> visited = new();
    public List<string> answer = new();
    public HashSet<string> reversed = new();
    public LinkedList<string> words;
    
    public char[][] board;
    int n, m;

    public void ConcatLinkedLists(LinkedList<string> to, LinkedList<string> from) {
        var cur = from.First;
        while(cur != null) {
            var next = cur.Next;
            from.Remove(cur);
            to.AddLast(cur);
            cur = next;
        }
    }

    public void CheckPosition(Point point, LinkedList<string> wordsToCheck, int chPosition) {
        int i = point.x, j = point.y;
        char ch = board[i][j];
        LinkedList<string> missingWords = new();
        for(var current = wordsToCheck.First; current != null;) {
            var next = current.Next;
            var word = current.Value;
            if (word[chPosition] != ch) {
                wordsToCheck.Remove(current);
                missingWords.AddLast(current);
            } else if (chPosition == word.Length - 1) {
                if (reversed.Contains(word))
                    answer.Add(new string(word.Reverse().ToArray()));
                else answer.Add(word);
                wordsToCheck.Remove(current);
            }
            current = next;
        }

        if (wordsToCheck.Count != 0) {
            visited.Add(point);
            int nextPosition = chPosition + 1;

            var CheckNextPosition = (Point p) => {
                if (!visited.Contains(p))
                    CheckPosition(p, wordsToCheck, nextPosition);
            };

            if (i > 0)
                CheckNextPosition(new(i - 1, j));
            if (wordsToCheck.Count != 0 && i < m - 1 )
                CheckNextPosition(new(i + 1, j));
            if (wordsToCheck.Count != 0 && j > 0)
                CheckNextPosition(new(i, j - 1));
            if (wordsToCheck.Count != 0 && j < n - 1)
                CheckNextPosition(new(i, j + 1));
            visited.Remove(point);
        }
        ConcatLinkedLists(wordsToCheck, missingWords);
    }

    public IList<string> FindWords(char[][] _board, string[] _words) {
        board = _board;
        words = new(_words);
        m = _board.Length;
        n = _board[0].Length;

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
        while(current != null) {
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
                string reverse = new string(word.Reverse().ToArray());
                words.AddAfter(current, reverse);
                words.Remove(current);
                reversed.Add(reverse);
            }
            wordCharFrequency.Clear();
            current = next;
        }



        for (int i = 0; i < m && words.Count != 0; ++i)
            for (int j = 0; j < n && words.Count != 0; ++j)
                CheckPosition(new Point(i, j), words, 0);
            
        return answer;
    }
}