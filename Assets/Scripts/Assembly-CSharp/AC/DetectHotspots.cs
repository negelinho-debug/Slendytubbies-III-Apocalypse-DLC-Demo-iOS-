using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class DetectHotspots : MonoBehaviour
	{
		private Hotspot nearestHotspot;

		private int selected;

		private List<Hotspot> hotspots = new List<Hotspot>();

		private StateHandler stateHandler;

		private void Start()
		{
			stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
		}

		private void OnLevelWasLoaded()
		{
			Start();
			hotspots.Clear();
			selected = 0;
		}

		public Hotspot GetSelected()
		{
			if (hotspots.Count > 0)
			{
				if (AdvGame.GetReferences().settingsManager.hotspotsInVicinity == HotspotsInVicinity.NearestOnly)
				{
					if (hotspots[selected].gameObject.layer == LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer))
					{
						return nearestHotspot;
					}
					nearestHotspot = null;
					hotspots.Remove(nearestHotspot);
				}
				else if (AdvGame.GetReferences().settingsManager.hotspotsInVicinity == HotspotsInVicinity.CycleMultiple)
				{
					if (selected >= hotspots.Count)
					{
						selected = hotspots.Count - 1;
					}
					else if (selected < 0)
					{
						selected = 0;
					}
					if (hotspots[selected].gameObject.layer == LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer))
					{
						return hotspots[selected];
					}
					if (nearestHotspot == hotspots[selected])
					{
						nearestHotspot = null;
					}
					hotspots.RemoveAt(selected);
				}
			}
			return null;
		}

		private void OnTriggerStay(Collider other)
		{
			if (!other.GetComponent<Hotspot>() || other.gameObject.layer != LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer))
			{
				return;
			}
			if (nearestHotspot == null || Vector3.Distance(transform.position, other.transform.position) <= Vector3.Distance(transform.position, nearestHotspot.transform.position))
			{
				nearestHotspot = other.GetComponent<Hotspot>();
			}
			foreach (Hotspot hotspot in hotspots)
			{
				if (hotspot == other.GetComponent<Hotspot>())
				{
					return;
				}
			}
			hotspots.Add(other.GetComponent<Hotspot>());
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (!other.GetComponent<Hotspot>() || other.gameObject.layer != LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer))
			{
				return;
			}
			if (nearestHotspot == null || Vector3.Distance(transform.position, other.transform.position) <= Vector3.Distance(transform.position, nearestHotspot.transform.position))
			{
				nearestHotspot = other.GetComponent<Hotspot>();
			}
			foreach (Hotspot hotspot in hotspots)
			{
				if (hotspot == other.GetComponent<Hotspot>())
				{
					return;
				}
			}
			hotspots.Add(other.GetComponent<Hotspot>());
		}

		private void OnTriggerExit(Collider other)
		{
			if ((bool)other.GetComponent<Hotspot>())
			{
				Hotspot component = other.GetComponent<Hotspot>();
				if (nearestHotspot == component)
				{
					nearestHotspot = null;
				}
				if (IsHotspotInTrigger(component))
				{
					hotspots.Remove(component);
				}
				if (component.highlight != null)
				{
					component.highlight.HighlightOff();
				}
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if ((bool)other.GetComponent<Hotspot>())
			{
				Hotspot component = other.GetComponent<Hotspot>();
				if (nearestHotspot == component)
				{
					nearestHotspot = null;
				}
				if (IsHotspotInTrigger(component))
				{
					hotspots.Remove(component);
				}
				if (component.highlight != null)
				{
					component.highlight.HighlightOff();
				}
			}
		}

		public void _Update()
		{
			if ((bool)nearestHotspot && nearestHotspot.gameObject.layer == LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.deactivatedLayer))
			{
				nearestHotspot = null;
			}
			if (stateHandler == null)
			{
				Start();
			}
			if (!(stateHandler != null) || stateHandler.gameState != GameState.Normal)
			{
				return;
			}
			try
			{
				if (Input.GetButtonDown("CycleHotspotsLeft"))
				{
					CycleHotspots(false);
				}
				else if (Input.GetButtonDown("CycleHotspotsRight"))
				{
					CycleHotspots(true);
				}
				else if (Input.GetAxis("CycleHotspots") > 0.1f)
				{
					CycleHotspots(true);
				}
				else if (Input.GetAxis("CycleHotspots") < -0.1f)
				{
					CycleHotspots(false);
				}
			}
			catch
			{
			}
		}

		private void CycleHotspots(bool goRight)
		{
			if (goRight)
			{
				selected++;
			}
			else
			{
				selected--;
			}
			if (selected >= hotspots.Count)
			{
				selected = 0;
			}
			else if (selected < 0)
			{
				selected = hotspots.Count - 1;
			}
		}

		public bool IsHotspotInTrigger(Hotspot hotspot)
		{
			if (hotspots.Contains(hotspot))
			{
				return true;
			}
			return false;
		}

		public void HighlightAll()
		{
			foreach (Hotspot hotspot in hotspots)
			{
				if (hotspot.highlight != null)
				{
					hotspot.highlight.HighlightOn();
				}
			}
		}
	}
}
