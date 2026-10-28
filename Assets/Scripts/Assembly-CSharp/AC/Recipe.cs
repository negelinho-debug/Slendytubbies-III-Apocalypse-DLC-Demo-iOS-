using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class Recipe
	{
		public string label;

		public int id;

		public bool isEditing;

		public bool autoCreate = true;

		public List<Ingredient> ingredients = new List<Ingredient>();

		public ActionListAsset invActionList;

		public int resultID;

		public bool useSpecificSlots;

		public OnCreateRecipe onCreateRecipe;

		public Recipe(int[] idArray)
		{
			isEditing = false;
			ingredients = new List<Ingredient>();
			resultID = 0;
			useSpecificSlots = false;
			invActionList = null;
			autoCreate = true;
			onCreateRecipe = OnCreateRecipe.JustMoveToInventory;
			foreach (int num in idArray)
			{
				if (id == num)
				{
					id++;
				}
			}
			label = "Recipe " + (id + 1);
		}
	}
}
