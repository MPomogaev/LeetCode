int k = int.Parse(Console.ReadLine());
int w = int.Parse(Console.ReadLine());
string[] profitsStr = Console.ReadLine().Split();
int[] profits = new int[profitsStr.Length];
for (int i = 0; i < profitsStr.Length; i++) {
    profits[i] = int.Parse(profitsStr[i]);
}
string[] capitalStr = Console.ReadLine().Split();
int[] capital = new int[capitalStr.Length];
for (int i = 0; i < capitalStr.Length; i++) {
    capital[i] = int.Parse(capitalStr[i]);
}

Console.WriteLine(new Solution().FindMaximizedCapital(k, w, profits, capital));

public class MaxHeap<T> where T : IComparable {
    private List<T> heap = new List<T>();

    public void Insert(T item) {
        heap.Add(item);
        HeapifyUp(heap.Count - 1);
    }

    public bool TryExtractMax(out T max) {
        if (heap.Count == 0) {
            max = default(T);
            return false;
        }
        max = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        HeapifyDown(0);
        return true;
    }

    public T PeekMax() {
        if (heap.Count == 0)
            throw new InvalidOperationException("Heap is empty");

        return heap[0];
    }

    public void RemoveAt(int index) {
        if (index < 0 || index >= heap.Count)
            throw new IndexOutOfRangeException("Invalid index");

        if (index == heap.Count - 1) {
            heap.RemoveAt(index);
        } else {
            heap[index] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(index);
            HeapifyUp(index);
        }
    }

    private void HeapifyUp(int index) {
        int parentIndex = (index - 1) / 2;

        if (index > 0 && heap[index].CompareTo(heap[parentIndex]) > 0) // CompareTo > 0 for max-heap
        {
            Swap(index, parentIndex);
            HeapifyUp(parentIndex);
        }
    }

    private void HeapifyDown(int index) {
        int largest = index;
        int leftChild = 2 * index + 1;
        int rightChild = 2 * index + 2;

        if (leftChild < heap.Count && heap[leftChild].CompareTo(heap[largest]) > 0) // CompareTo > 0 for max-heap
        {
            largest = leftChild;
        }

        if (rightChild < heap.Count && heap[rightChild].CompareTo(heap[largest]) > 0) // CompareTo > 0 for max-heap
        {
            largest = rightChild;
        }

        if (largest != index) {
            Swap(index, largest);
            HeapifyDown(largest);
        }
    }

    private void Swap(int i, int j) {
        T temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}

public class ProfitCapital: IComparable {
    public int profit;
    public int capital;

    public ProfitCapital(int profit, int capital) {
        this.profit = profit;
        this.capital = capital;
    }

    public int CompareTo(object obj) {
        ProfitCapital other = obj as ProfitCapital;
        return this.profit.CompareTo(other.profit);
    }
}

public class Solution {
    public List<ProfitCapital> capitalOrder = new();
    public MaxHeap<ProfitCapital> profitOrder = new();
    int maxIndex;
    int maxCapital;

    public void TransferFromCapitalToProfitOrder(ref int lastIndex) {
        while (lastIndex < maxIndex 
            && capitalOrder[lastIndex].capital <= maxCapital) {
            profitOrder.Insert(capitalOrder[lastIndex]);
            lastIndex++;
        }
    }

    public int FindMaximizedCapital(int k, int w, int[] profits, int[] capital) {
        maxIndex = profits.Length;
        for (int i = 0; i < maxIndex; i++) {
            capitalOrder.Add(new(profits[i], capital[i]));
        }
        capitalOrder.Sort((left, right) => {
            return left.capital.CompareTo(right.capital);
        });
        maxCapital = w;
        int index = 0;
        for(int i = 0; i < k; ++i) {
            TransferFromCapitalToProfitOrder(ref index);
            ProfitCapital profit;
            if (!profitOrder.TryExtractMax(out profit)) 
                break;
            maxCapital += profit.profit;
        }
        return maxCapital;
    }
}