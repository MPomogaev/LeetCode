using System.Text;
using static Solution;

string s = Console.ReadLine();

Console.WriteLine(new Solution().ShortestPalindrome(s));

public class SymbolCount {
    public SymbolCount(char symbol)
        : this(0, symbol) { }

    public SymbolCount(int count, char symbol) {
        this.count = count;
        this.symbol = symbol;
    }

    public int count;
    public char symbol;
}

public class Solution {
    int middleSymbolIndex = 0;
    int middle;
    int rightEndSymbolIndex = 0, rightEndCountIndex = 0;
    List<SymbolCount> compresed;
    Dictionary<char, int> leftPartSymbolsCounts = new();
    Dictionary<char, int> rightPartSymbolsCounts = new();
    int compresedStringLength = 0;
    int differencesCount = 0;
    HashSet<char> differences = new();
    HashSet<char> changedSymbols = new();

    public int DivisionByTwoCeiling(int num) => num / 2 + num % 2;

    public void CompressAndFindMiddle(string s) {
        compresed = new() { new SymbolCount(s[0]) };
        int length = s.Length;
        var last = compresed.Last();
        for (int i = 0; i < length; i++) {
            if (last.symbol != s[i]) {
                if (2 * (i - DivisionByTwoCeiling(last.count)) + last.count % 2 <= length)
                    middleSymbolIndex = compresed.Count() - 1;
                compresed.Add(new(s[i]));
                last = compresed.Last();
            }
            last.count++;
        }
        int leftCompresedPartLength = 0;
        for (int i = 0; i < middleSymbolIndex; i++)
            leftCompresedPartLength += compresed[i].count;
        
        rightEndSymbolIndex = compresed.Count() - 1;
        rightEndCountIndex = compresed[rightEndSymbolIndex].count;
        compresedStringLength = 2 * leftCompresedPartLength 
            + compresed[middleSymbolIndex].count;
        int num = length - compresedStringLength;
        while (num >= rightEndCountIndex) {
            num -= rightEndCountIndex;
            rightEndSymbolIndex--;
            rightEndCountIndex = compresed[rightEndSymbolIndex].count;
        }
        rightEndCountIndex -= num;
    }

    public void SetLeftAndRightStringParts() {
        for(int i = 0; i <= rightEndSymbolIndex; ++i) 
            changedSymbols.Add(compresed[i].symbol);
        foreach(var symbol in changedSymbols) {
            leftPartSymbolsCounts[symbol] = 0;
            rightPartSymbolsCounts[symbol] = 0;
        }
        SymbolCount compresedSymbol;
        for (int i = 0; i < middleSymbolIndex; i++) {
            compresedSymbol = compresed[i];
            leftPartSymbolsCounts[compresedSymbol.symbol] += compresedSymbol.count;
        }
        for (int i = rightEndSymbolIndex - 1; i > middleSymbolIndex; i--) {
            compresedSymbol = compresed[i];
            rightPartSymbolsCounts[compresedSymbol.symbol] += compresedSymbol.count;
        }
        compresedSymbol = compresed[rightEndSymbolIndex];
        rightPartSymbolsCounts[compresedSymbol.symbol] += rightEndCountIndex;
    }

    public bool CheckPossition() {
        if (DivisionByTwoCeiling(rightEndSymbolIndex) != middleSymbolIndex)
            return false;
        int i = middleSymbolIndex;
        int j = i;
        while(i >= 0) {
            int leftSideSymbolCount = compresed[i].count;
            char leftSymbol = compresed[i].symbol;
            int rightSideSymbolCount = compresed[j].count;
            if (j == rightEndSymbolIndex)
                rightSideSymbolCount = rightEndCountIndex;
            char rightSymbol = compresed[j].symbol;
            if (leftSymbol != rightSymbol 
                || leftSideSymbolCount != rightSideSymbolCount)
                return false;
            --i;
            ++j;
        }
        return true;
    }

    public void UpdateDifferences() {
        foreach (var symbol in changedSymbols)
            if (leftPartSymbolsCounts[symbol] == rightPartSymbolsCounts[symbol]) {
                if (differences.Contains(symbol)) {
                    differences.Remove(symbol);
                    differencesCount--;
                }
            } else if (!differences.Contains(symbol)) {
                differences.Add(symbol);
                differencesCount++;
            }
        changedSymbols.Clear();
    }

    public void MoveRightEnd(int num) {
        var symbolCount = compresed[rightEndSymbolIndex];
        char symbol;
        while (num >= rightEndCountIndex) {
            num -= rightEndCountIndex;
            symbol = symbolCount.symbol;
            rightPartSymbolsCounts[symbol] -= rightEndCountIndex;
            changedSymbols.Add(symbol);
            rightEndSymbolIndex--;
            rightEndCountIndex = compresed[rightEndSymbolIndex].count;
            symbolCount = compresed[rightEndSymbolIndex];
        }
        rightEndCountIndex -= num;
        symbol = compresed[rightEndSymbolIndex].symbol;
        rightPartSymbolsCounts[symbol] -= num;
        changedSymbols.Add(symbol);
    }

    public string ShortestPalindrome(string s) {
        if (s.Length == 0)
            return "";
        CompressAndFindMiddle(s);
        SetLeftAndRightStringParts();
        UpdateDifferences();
        var compressedMiddle = compresed[middleSymbolIndex];
        char middleSymbol = compressedMiddle.symbol;
        int middleSymbolCount = compressedMiddle.count;
        while (middleSymbolIndex > 0) {
            if (differencesCount == 0)
                if (CheckPossition())
                    break;
            int rightEndMoveCount = 0;
            int leftEndMoveCount = 0;
            int middleSymbolLengthChange = 0;
            do {
                middleSymbolLengthChange += middleSymbolCount % 2;
                leftEndMoveCount += middleSymbolCount / 2;
                rightPartSymbolsCounts[middleSymbol] += middleSymbolCount;
                middleSymbolIndex--;
                compressedMiddle = compresed[middleSymbolIndex];
                middleSymbol = compressedMiddle.symbol;
                middleSymbolCount = compressedMiddle.count;
                leftPartSymbolsCounts[middleSymbol] -= middleSymbolCount;
                changedSymbols.Add(middleSymbol);
                leftEndMoveCount += DivisionByTwoCeiling(middleSymbolCount);
                middleSymbolLengthChange -= middleSymbolCount % 2;
                rightEndMoveCount = leftEndMoveCount * 2 + middleSymbolLengthChange;
            } while (s[0] != s[compresedStringLength - rightEndMoveCount - 1]);
            compresedStringLength -= rightEndMoveCount;
            MoveRightEnd(rightEndMoveCount);
            UpdateDifferences();
        }
        int stringRightPartIndex = s.Length;
        for (int rightSymbol = compresed.Count - 1; rightSymbol > rightEndSymbolIndex; --rightSymbol)
            stringRightPartIndex -= compresed[rightSymbol].count;
        stringRightPartIndex -= compresed[rightEndSymbolIndex].count - rightEndCountIndex;
        return new string(s.Substring(stringRightPartIndex).Reverse().ToArray()) + s;
    }
}