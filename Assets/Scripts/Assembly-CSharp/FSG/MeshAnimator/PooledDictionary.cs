using System.Collections.Generic;

namespace FSG.MeshAnimator
{
	public class PooledDictionary<T, T2> : Dictionary<T, T2>
	{
		private static Stack<PooledDictionary<T, T2>> stack = new Stack<PooledDictionary<T, T2>>();

		private static uint checkedOut = 0u;

		private static uint returned = 0u;

		public bool recycleable { get; set; }

		public PooledDictionary()
		{
		}

		public PooledDictionary(int capacity)
			: base(capacity)
		{
		}

		public PooledDictionary(int capacity, IEqualityComparer<T> comparer)
			: base(capacity, comparer)
		{
		}

		public static void Init(int initialPoolSize = 10)
		{
			for (int i = 0; i < initialPoolSize; i++)
			{
				stack.Push(new PooledDictionary<T, T2>());
			}
		}

		public static PooledDictionary<T, T2> Get(int capacity, IEqualityComparer<T> comparer = null)
		{
			lock (stack)
			{
				if (stack.Count > 0)
				{
					checkedOut++;
					PooledDictionary<T, T2> pooledDictionary = stack.Pop();
					pooledDictionary.recycleable = true;
					return pooledDictionary;
				}
			}
			checkedOut++;
			PooledDictionary<T, T2> pooledDictionary2;
			if (comparer != null)
			{
				pooledDictionary2 = new PooledDictionary<T, T2>(capacity, comparer);
				pooledDictionary2.recycleable = true;
				return pooledDictionary2;
			}
			pooledDictionary2 = new PooledDictionary<T, T2>(capacity);
			pooledDictionary2.recycleable = true;
			return pooledDictionary2;
		}

		public static PooledDictionary<T, T2> Get()
		{
			return Get(0);
		}

		public static PooledDictionary<T, T2> Get(IEqualityComparer<T> comparer)
		{
			return Get(0, comparer);
		}

		public void ReturnToPool(bool force = false)
		{
			if (!recycleable && !force)
			{
				return;
			}
			recycleable = true;
			returned++;
			Clear();
			lock (stack)
			{
				stack.Push(this);
			}
		}
	}
}
