using System;
using System.Collections.Generic;

namespace FSG.MeshAnimator
{
	public static class GenericObjectPool<T>
	{
		private static Stack<T> pool = new Stack<T>();

		public static int Count
		{
			get
			{
				lock (pool)
				{
					return pool.Count;
				}
			}
		}

		public static void InitPool(int count)
		{
			for (int i = 0; i < count; i++)
			{
				T obj = Get();
				Return(obj);
			}
		}

		public static T Get()
		{
			if (Count > 0)
			{
				lock (pool)
				{
					return pool.Pop();
				}
			}
			return Activator.CreateInstance<T>();
		}

		public static void Return(T obj)
		{
			lock (pool)
			{
				pool.Push(obj);
			}
		}
	}
}
