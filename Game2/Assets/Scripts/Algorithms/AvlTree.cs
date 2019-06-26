
namespace System.Collections.Generic
{
	public class AvlTree<TKey, TValue>
		where TKey : IComparable<TKey>
	{
		public static readonly AvlTree<TKey, TValue> Empty = new AvlTree<TKey, TValue>(null);
        
		public Node<TKey, TValue> Root { get; private set; }

		private AvlTree(Node<TKey, TValue> node)
		{
			this.Root = node;
		}

		public TValue this[TKey key]
		{
			get
			{
				var node = Get(key, this.Root);
				return node == null ? default(TValue) : node.Value;
			}
		}

		public AvlTree<TKey, TValue> Add(TKey key, TValue value)
		{
			var result = Add(this.Root, key, value);
			return new AvlTree<TKey, TValue>(result);
		}

		private static Node<TKey, TValue> Get(TKey key, Node<TKey, TValue> node)
		{
			if (node == null)
				return null;

			var comparison = node.Key.CompareTo(key);
			if (comparison == 0)
				return node;

			if (comparison > 0)
				return Get(key, node.Left);
			else
				return Get(key, node.Right);
		}

		private static Node<TKey, TValue> Add(Node<TKey, TValue> node, TKey key, TValue value)
		{
			if (node == null)
				return new Node<TKey, TValue>(key, value, null, null);

			var comparison = node.Key.CompareTo(key);
			if (comparison == 0)
				return new Node<TKey, TValue>(key, value, node.Left, node.Right);

			var l = node.Left;
			var r = node.Right;

			if (comparison > 0)
				l = Add(node.Left, key, value);
			else
				r = Add(node.Right, key, value);

			var n = new Node<TKey, TValue>(node.Key, node.Value, l, r);
			var lh = n.Left == null ? 0 : n.Left.Height;
			var rh = n.Right == null ? 0 : n.Right.Height;
			var b = lh - rh;

			if (Math.Abs(b) == 2) // 2 or -2 means unbalanced
			{
				if (b == 2) // L
				{
					var llh = n.Left.Left == null ? 0 : n.Left.Left.Height;
					var lrh = n.Left.Right == null ? 0 : n.Left.Right.Height;
					var lb = llh - lrh;
					if (lb == 1) // LL
					{
						// rotate right
						n = RotateRight(n);
					}
					else // LR
					{
						// rotate left
						// rotate right
						l = RotateLeft(l);
						n = new Node<TKey, TValue>(n.Key, n.Value, l, r);
						n = RotateRight(n);
					}
				}
				else // R
				{
					var rlh = n.Right.Left == null ? 0 : n.Right.Left.Height;
					var rrh = n.Right.Right == null ? 0 : n.Right.Right.Height;
					var rb = rlh - rrh;
					if (rb == 1) // RL
					{
						// rotate right
						// rotate left
						r = RotateRight(r);
						n = new Node<TKey, TValue>(n.Key, n.Value, l, r);
						n = RotateLeft(n);
					}
					else // RR
					{
						// rotate left
						n = RotateLeft(n);
					}
				}
			}

			return n;
		}

		private static Node<TKey, TValue> RotateRight(Node<TKey, TValue> node)
		{
			//       (5)            4     
			//       / \           / \    
			//      4   D         /   \   
			//     / \           3     5  
			//    3   C    -->  / \   / \ 
			//   / \           A   B C   D
			//  A   B                     

			var L = node.Left.Left;
			var R = new Node<TKey, TValue>(node.Key, node.Value, node.Left.Right, node.Right);
			var N = new Node<TKey, TValue>(node.Left.Key, node.Left.Value, L, R);
			return N;
		}

		private static Node<TKey, TValue> RotateLeft(Node<TKey, TValue> node)
		{
			//    (3)               4     
			//    / \              / \    
			//   A   4            /   \   
			//      / \          3     5  
			//     B   5   -->  / \   / \ 
			//        / \      A   B C   D
			//       C   D                

			var L = new Node<TKey, TValue>(node.Key, node.Value, node.Left, node.Right.Left);
			var R = node.Right.Right;
			var N = new Node<TKey, TValue>(node.Right.Key, node.Right.Value, L, R);
			return N;
		}

		public class Node<K, V>
		{
			public K Key { get; private set; }
			public V Value { get; private set; }

			public int Height { get; private set; }

			public Node<K, V> Left { get; private set; }
			public Node<K, V> Right { get; private set; }

			public Node(K key, V value, Node<K, V> left, Node<K, V> right)
			{
				this.Key = key;
				this.Value = value;
				this.Left = left;
				this.Right = right;

				var l = left == null ? 0 : left.Height;
				var r = right == null ? 0 : right.Height;
				this.Height = Math.Max(l, r) + 1;
			}
		}
	}
}
