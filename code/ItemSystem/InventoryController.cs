using System;
using System.Threading;
using ProjectP.ItemSystem;

public sealed class InventoryController : Component
{
	/// <summary>
	/// Shows capacity of the inventory. 
	/// If negative, then inventory has infinite capacity.
	/// </summary>
	[Property] public int ItemCapacity { get; private set; }
	[Property] private List<Item> _items;
	[Property] private GameObject BaseItem { get; set; }

	public int ItemsCount 
	{ 
		get 
		{
			return _items.Count;
		}
	}

	/// <summary>
	/// If inventory is occupied by player or other object
	/// </summary>
	public bool InventoryOccupied { get; private set; }

	public Item GetItem(int index)
	{
		return _items[index];
	}

	public Item AddItem(Item item)
	{
		if (_items.Count + 1 <= ItemCapacity || ItemCapacity < 0)
		{
			if (item is not null) _items.Add(item);
			return item;
		}

		else return null;
	}

	public Item RemoveItem(int index)
	{
		Item removed_item = null;
		if (_items.Count > index)
		{
			removed_item = _items[index];
			_items.RemoveAt(index);
		}

		return removed_item;
	}

	public Item RemoveItem(Item item)
	{
		Item removed_item = null;

		if (_items.Contains(item))
		{
			removed_item = item;
			_items.Remove(item);
		}

		return removed_item;
	}

	public Item DropItem(Item item)
	{
		var droppedItem = RemoveItem(item);

		if (droppedItem is not null)
		{
			GameObject dropped = BaseItem.Clone(GameObject.Transform.Position);
			var itemController = dropped.Components.Get<ItemController>();

			itemController.ItemDefenition = item;
		}

		return droppedItem;
	}

	private void DebugOutput()
	{
		var s = "";
		foreach (var item in _items)
		{
			s += item.Name + " ";
		}

		Log.Info($"Inventory of {GameObject.Name} contains {_items.Count} items: {s}");
	}

	protected override void OnAwake()
	{
		_items ??= new List<Item>();

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
		base.OnStart();
	}
}
