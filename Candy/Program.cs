int n = int.Parse(Console.ReadLine());
string[] input = Console.ReadLine().Split();
int[] rating = new int[n];

for (int i = 0; i < n; i++) {
    rating[i] = int.Parse(input[i]);
}

Console.WriteLine(new Solution().Candy(rating));

public class Child {
    public readonly int Rating;
    public int Candies = 1;
    public Child Left;
    public Child Right;

    public Child(int rating) {
        Rating = rating;
    }
}

public class Solution {

    public int Candy(int[] ratings) {
        List<Child> children = new();
        children.Add(new(ratings[0]));
        for (int i = 1; i < ratings.Length; ++i) {
            children.Add(new(ratings[i]));
            children[i].Left = children[i - 1];
            children[i-1].Right = children[i];
        }
        children = children.OrderBy(x => x.Rating).ToList();
        int candiesCount = 0;
        foreach(var child in children) {
            int candies = 1;
            int rating = child.Rating;
            if (child.Left != null && child.Left.Rating < rating) {
                candies = child.Left.Candies + 1;
            }
            if (child.Right != null && child.Right.Rating < rating 
                && child.Right.Candies >= candies) {
                candies = child.Right.Candies + 1;
            }
            child.Candies = candies;
            candiesCount += candies;
        }
        return candiesCount;
    }
}
