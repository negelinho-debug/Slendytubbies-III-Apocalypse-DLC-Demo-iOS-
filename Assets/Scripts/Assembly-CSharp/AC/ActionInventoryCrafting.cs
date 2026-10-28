using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionInventoryCrafting : Action
	{
		public enum ActionCraftingMethod
		{
			ClearRecipe,
			CreateRecipe
		}

		public ActionCraftingMethod craftingMethod;

		public override float Run()
		{
			if (craftingMethod == ActionCraftingMethod.ClearRecipe)
			{
				GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>().RemoveRecipes();
			}
			else if (craftingMethod == ActionCraftingMethod.CreateRecipe)
			{
				PlayerMenus.CreateRecipe();
			}
			return 0f;
		}
	}
}
