using ProjectP.ItemSystem;
using Sandbox;

public sealed class ItemController : Component
{
	[Property] public Item ItemDefenition {get; set;}
	[Property] public GameObject ModelObject {get; set;}

	public void UpdateModel()
	{
		if (ItemDefenition is null) return;
		if (ItemDefenition.Model is not null)
		{
			var collider = ModelObject.Components.Get<ModelCollider>();
			var renderer = ModelObject.Components.Get<ModelRenderer>();

			renderer.Model = Model.Load(ItemDefenition.Model);
			collider.Model = renderer.Model;
		}
	}

	protected override void OnAwake()
	{
		var collider = ModelObject.Components.Get<ModelCollider>();
		var renderer = ModelObject.Components.Get<ModelRenderer>();

		collider.Model = renderer.Model;

		UpdateModel();
		
		base.OnAwake();
	}
}
