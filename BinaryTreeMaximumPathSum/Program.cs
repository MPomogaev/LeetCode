TreeNode root = new(-10, new(9), new(20, new(15), new(7)));

Console.WriteLine(new Solution().MaxPathSum(root));

public class TreeNode {
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
        this.val = val;
        this.left = left;
        this.right = right;
    }
}

public class TreeNodeExtension {
    public int childMaxSum = 0;
    public int val;
    public TreeNodeExtension left = null;
    public TreeNodeExtension right = null;

    public TreeNodeExtension(TreeNode node) {
        val = node.val;
        left = node.left == null ? null : new(node.left);
        right = node.right == null ? null : new(node.right);
    }
}

public class Solution {

    public int NodeMaxSumSearch(TreeNodeExtension node) {
        int val = node.val;
        int left = val, right = val;
        int maxRootSum = val;
        int leftAsRoot = val, rightAsRoot = val;
        if (node.left != null) {
            leftAsRoot = NodeMaxSumSearch(node.left);
            int leftNodeMaxSum = node.left.childMaxSum;
            left = leftNodeMaxSum > 0 ? val + leftNodeMaxSum : val;
            maxRootSum += leftNodeMaxSum > 0 ? leftNodeMaxSum : 0;
        }
        if (node.right != null) {
            rightAsRoot = NodeMaxSumSearch(node.right);
            int rightNodeMaxSum = node.right.childMaxSum;
            right = rightNodeMaxSum > 0 ? val + rightNodeMaxSum : val;
            maxRootSum += rightNodeMaxSum > 0 ? rightNodeMaxSum : 0;
        }
        node.childMaxSum = right > left ? right : left;
        if (maxRootSum < leftAsRoot)
            maxRootSum = leftAsRoot;
        if (maxRootSum < rightAsRoot)
            maxRootSum = rightAsRoot;
        return maxRootSum;
    }

    public int MaxPathSum(TreeNode root) {
        TreeNodeExtension extensionRoot = new(root);
        return NodeMaxSumSearch(extensionRoot);
    }
}
