using System.Text;

string s = Console.ReadLine();

Console.WriteLine(new Solution().Calculate(s));

public class Solution {
    Stack<int> numbersStack = new();
    Stack<char> operationsStack = new();
    List<char> operationsChars = new() { '-', '+', '(', ')' };

    public bool ProcessOperation(char operation, bool isUnary) {
        if (operation == '(') {
            operationsStack.Push('(');
            return true;
        }
        if (operation == ')') {
            CalculateInParentheses();
        } else {
            if (operation == '-' && !isUnary)
                operationsStack.Push('+');
            operationsStack.Push(operation);
        }
        return false;
    }

    public void CalculateInParentheses() {
        char operation;
        int result = 0;
        while(operationsStack.TryPop(out operation) && operation != '(') {
            switch (operation) {
                case '+':
                    Plus();
                    break;
                case '-':
                    numbersStack.Push(-numbersStack.Pop());
                    break;
            }
        }
    }

    public void Plus() {
        int right = numbersStack.Pop();
        int left = GetNextNumber();
        numbersStack.Push(left + right);
    }

    public int GetNextNumber() {
        int number = numbersStack.Pop();
        char operation;
        if (operationsStack.TryPeek(out operation) && operation == '-') {
            operationsStack.Pop();
            return -number;
        }
        return number;
    }

    public int Calculate(string s) {
        s = s.Replace(" ", "");
        StringBuilder numberStr = new("");
        bool isUnary = true;
        foreach(var ch in s) {
            if (operationsChars.Contains(ch)) {
                if (numberStr.Length > 0) {
                    numbersStack.Push(int.Parse(numberStr.ToString()));
                    numberStr.Clear();
                }
                isUnary = ProcessOperation(ch, isUnary);
            } else {
                numberStr.Append(ch);
                isUnary = false;
            }
        }
        if (numberStr.Length > 0) {
            numbersStack.Push(int.Parse(numberStr.ToString()));
        }
        CalculateInParentheses();
        return numbersStack.First();
    }
}