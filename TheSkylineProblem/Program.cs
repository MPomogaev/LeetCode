int n = int.Parse(Console.ReadLine());
int[][] buildings = new int[n][];

for (int i = 0; i < n; i++) {
    string[] nums = Console.ReadLine().Split();
    buildings[i] = new int[3];
    buildings[i][0] = int.Parse(nums[0]);
    buildings[i][1] = int.Parse(nums[1]);
    buildings[i][2] = int.Parse(nums[2]);
}

var answer = new Solution().GetSkyline(buildings);
for (int i = 0; i < answer.Count(); ++i) {
    Console.Write("[" + answer[i][0] +  "," + answer[i][1] + "]");
}

public class MaxHeap {
    private List<int[]> heap = new List<int[]>();

    public void Insert(int[] item) {
        heap.Add(item);
        HeapifyUp(heap.Count - 1);
    }

    public bool TryExtractMax(out int[] max) {
        if (heap.Count == 0) {
            max = default(int[]);
            return false;
        }
        max = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        HeapifyDown(0);
        return true;
    }

    public int[] PeekMax() {
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

        if (index > 0 && heap[index][2] - heap[parentIndex][2] > 0) 
        {
            Swap(index, parentIndex);
            HeapifyUp(parentIndex);
        }
    }

    private void HeapifyDown(int index) {
        int largest = index;
        int leftChild = 2 * index + 1;
        int rightChild = 2 * index + 2;

        if (leftChild < heap.Count && heap[leftChild][2] - heap[largest][2] > 0)
        {
            largest = leftChild;
        }

        if (rightChild < heap.Count && heap[rightChild][2] - heap[largest][2] > 0)
        {
            largest = rightChild;
        }

        if (largest != index) {
            Swap(index, largest);
            HeapifyDown(largest);
        }
    }

    private void Swap(int i, int j) {
        int[] temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}

public class Solution {

    public IList<IList<int>> GetSkyline(int[][] buildings) {
        int maxRight = int.MaxValue;
        MaxHeap rightEndHeights = new();
        rightEndHeights.Insert(new int[]{0, maxRight, 0});
        List<IList<int>> answer = new List<IList<int>>();
        int buildingIndex = 0;
        var building = buildings[buildingIndex];
        int[] last = new int[3] { -1, maxRight, 0};
        int nextLeft = 0;
        int buildingCount = buildings.Count();
        var nextBuilding = () => {
            buildingIndex++;
            if (buildingIndex < buildingCount)
                building = buildings[buildingIndex];
        };
        var setNextLeft = () 
            => nextLeft = buildingIndex < buildingCount ? building[0] : maxRight;
        while (buildingIndex < buildingCount) {
            while (buildingIndex < buildingCount
                && last[1] >= building[0]) {
                while (buildingIndex < buildingCount - 1
                    && buildings[buildingIndex+1][0] == building[0]) {
                    rightEndHeights.Insert(buildings[buildingIndex]);
                    ++buildingIndex;
                    var newBuilding = buildings[buildingIndex];
                    if (newBuilding[2] >= building[2])
                        building = newBuilding;
                    
                }
                if (last[2] < building[2]) {
                    if (last[0] < building[0]) {
                        answer.Add(new int[] { building[0], building[2] });
                        last = building;
                    }
                }
                rightEndHeights.Insert(buildings[buildingIndex]);
                nextBuilding();
            }
            setNextLeft();
            var nextRight = rightEndHeights.PeekMax();
            while (last[1] <= nextLeft) {
                if (nextRight[2] == 0)
                    break;
                while (buildingIndex < buildingCount
                && nextRight[1] >= nextLeft) {
                    if (nextRight[2] == building[2])
                        nextRight = building;
                    if (nextRight[2] > building[2])
                        rightEndHeights.Insert(building);
                    if (nextRight[2] < building[2])
                        break;
                    nextBuilding();
                    setNextLeft();
                }
                if (last[1] < nextRight[1] ) {
                    if (last[2] > nextRight[2])
                        answer.Add(new int[] { last[1], nextRight[2] });
                    last[2] = nextRight[2];
                    last[1] = nextRight[1];
                }
                if (nextRight[1] < nextLeft
                    || buildingIndex >= buildingCount) {
                    rightEndHeights.RemoveAt(0);
                    nextRight = rightEndHeights.PeekMax();
                } else
                    break;
            }
            if (nextRight[2] == 0)
                answer.Add(new int[] { last[1], 0 });
            last[1] = building[1];
            last[2] = nextRight[2];
        }
        return answer;
    }
}