using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class Paths : MonoBehaviour
	{
		public List<Vector3> nodes = new List<Vector3>();

		public List<NodeCommand> nodeCommands = new List<NodeCommand>();

		public ActionListSource commandSource;

		public AC_PathType pathType = AC_PathType.ForwardOnly;

		public PathSpeed pathSpeed;

		public bool teleportToStart;

		public bool affectY;

		public float nodePause;

		private void Awake()
		{
			if (nodePause < 0f)
			{
				nodePause = 0f;
			}
			if (nodes == null || nodes.Count == 0)
			{
				nodes.Add(transform.position);
			}
			else
			{
				nodes[0] = transform.position;
			}
		}

		public bool WillStopAtNextNode(int currentNode)
		{
			if (GetNextNode(currentNode, currentNode - 1, false) == -1)
			{
				return true;
			}
			return false;
		}

		public void BuildNavPath(Vector3[] pointData)
		{
			if (pointData.Length > 0)
			{
				pathType = AC_PathType.ForwardOnly;
				affectY = false;
				nodePause = 0f;
				nodes.Clear();
				nodes.Add(transform.position);
				nodeCommands.Clear();
				foreach (Vector3 item in pointData)
				{
					nodes.Add(item);
				}
			}
		}

		public int GetNextNode(int currentNode, int prevNode, bool playerControlled)
		{
			int count = nodes.Count;
			if (count == 1)
			{
				return -1;
			}
			if (playerControlled)
			{
				if (currentNode == 0)
				{
					return 1;
				}
				if (currentNode == count - 1)
				{
					return -1;
				}
				return currentNode + 1;
			}
			if (pathType == AC_PathType.ForwardOnly)
			{
				if (currentNode == count - 1)
				{
					return -1;
				}
				return currentNode + 1;
			}
			if (pathType == AC_PathType.Loop)
			{
				if (currentNode == count - 1)
				{
					return 0;
				}
				return currentNode + 1;
			}
			if (pathType == AC_PathType.PingPong)
			{
				if (prevNode > currentNode)
				{
					if (currentNode == 0)
					{
						return 1;
					}
					return currentNode - 1;
				}
				if (currentNode == count - 1)
				{
					return currentNode - 1;
				}
				return currentNode + 1;
			}
			if (pathType == AC_PathType.IsRandom)
			{
				if (count > 0)
				{
					int num;
					for (num = Random.Range(0, count); num == currentNode; num = Random.Range(0, count))
					{
					}
					return num;
				}
				return 0;
			}
			return -1;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			int count = nodes.Count;
			if (nodes.Count > 0)
			{
				nodes[0] = transform.position;
			}
			if (pathType == AC_PathType.IsRandom && count > 1)
			{
				for (int i = 1; i < count; i++)
				{
					for (int j = 0; j < count; j++)
					{
						if (i != j)
						{
							ConnectNodes(i, j);
						}
					}
				}
				return;
			}
			if (count > 1)
			{
				for (int i = 1; i < count; i++)
				{
					Gizmos.DrawIcon(nodes[i], string.Empty, true);
					ConnectNodes(i, i - 1);
				}
			}
			if (pathType == AC_PathType.Loop && !teleportToStart && count > 2)
			{
				ConnectNodes(count - 1, 0);
			}
		}

		public float GetLengthToNode(int n)
		{
			if (n > 0 && nodes.Count > n)
			{
				float num = 0f;
				for (int i = 1; i <= n; i++)
				{
					num += Vector3.Distance(nodes[i - 1], nodes[i]);
				}
				return num;
			}
			return 0f;
		}

		public float GetLengthBetweenNodes(int a, int b)
		{
			if (a == b)
			{
				return 0f;
			}
			if (b < a)
			{
				int num = a;
				a = b;
				b = num;
			}
			float num2 = 0f;
			for (int i = a + 1; i <= b; i++)
			{
				num2 += Vector3.Distance(nodes[i - 1], nodes[i]);
			}
			return num2;
		}

		public float GetTotalLength()
		{
			if (nodes.Count > 1)
			{
				return GetLengthToNode(nodes.Count - 1);
			}
			return 0f;
		}

		private void ConnectNodes(int a, int b)
		{
			Vector3 vector = nodes[a];
			Vector3 to = nodes[b];
			Gizmos.DrawLine(vector, to);
		}
	}
}
