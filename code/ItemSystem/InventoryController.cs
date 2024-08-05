using System;
using System.Threading;
using ProjectP.ItemSystem;
using Sandbox;

public sealed class InventoryController : Component
{
	/// <summary>
	/// Shows capacity of the inventory. 
	/// If negative, then inventory has infinite capacity.
	/// </summary>
	[Property] public int ItemCapacity { get; set; }
	[Property] public List<Item> Items;
	[Property] public GameObject BaseItem { get; set; }

	/// <summary>
	/// If inventory is occupied by player or other object
	/// </summary>
	public bool InventoryOccupied { get; }

	private Item AddItem(Item item)
	{
		if (Items.Count + 1 <= ItemCapacity || ItemCapacity < 0)
		{
			if (item is not null) Items.Add(item);
			return item;
		}

		else return null;
	}

	private Item RemoveItem(int index)
	{
		Item removed_item = null;
		if (Items.Count > index)
		{
			removed_item = Items[index];
			Items.RemoveAt(index);
		}

		return removed_item;
	}

	private Item RemoveItem(Item item)
	{
		Item removed_item = null;

		if (Items.Contains(item))
		{
			removed_item = item;
			Items.Remove(item);
		}

		return removed_item;
	}

	private Item DropItem(Item item)
	{
		var droppedItem = RemoveItem(item);

		if (droppedItem is not null)
		{
			GameObject dropped = BaseItem.Clone(GameObject.Transform.Position);
			var itemController = dropped.Components.Get<ItemController>();

			itemController.ItemDefenition = item;
			itemController.UpdateModel();
		}

		return droppedItem;
	}

	private void DebugOutput()
	{
		var s = "";
		foreach (var item in Items)
		{
			s += item.Name + " ";
		}

		Log.Info($"Inventory contains {Items.Count} items: {s}");
	}

	protected override void OnAwake()
	{
		Items ??= new List<Item>();

		// if (BaseItem is null)
		// {
		// 	GameObject baseItemTemp = new GameObject();
		// 	baseItemTemp.SetPrefabSource("/items/prefabs/baseitem.prefab");
		// 	BaseItem = baseItemTemp;
		// }

		base.OnAwake();
	}

	protected override void OnStart()
	{
		DebugOutput();
		DropItem(Items[0]);
		base.OnStart();
	}
}
