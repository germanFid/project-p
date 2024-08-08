using ProjectP.ItemSystem;

public sealed class ItemController : Component
{
	[Property] private Item _itemDefenition;
	public Item ItemDefenition
	{
		get => _itemDefenition;

		set
		{
			_itemDefenition = value;
			UpdateModel();
		}
	}

	[Property] private GameObject ModelObject {get; set;}

	private void UpdateModel()
	{
		if (_itemDefenition is null) 
		{
			return;
		}

		if (_itemDefenition.Model is not null)
		{
			var collider = ModelObject.Components.Get<ModelCollider>();
			var renderer = ModelObject.Components.Get<ModelRenderer>();

			renderer.Model = Model.Load(_itemDefenition.Model);
			collider.Model = renderer.Model;
		}
	}

	protected override void OnAwake()
	{
		var collider = ModelObject.Components.Get<ModelCollider>();
		var renderer = ModelObject.Components.Get<ModelRenderer>();

		UpdateModel();
		collider.Model = renderer.Model;
		
		base.OnAwake();
	}
}
