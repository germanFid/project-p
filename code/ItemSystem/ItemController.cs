using ProjectP.ItemSystem;
using Sandbox;

public sealed class ItemController : Component
{
	[Property] public Item ItemDefenition {get; set;}
	[Property] public GameObject ModelObject {get; set;}

	protected override void OnAwake()
	{
		var collider = ModelObject.Components.Get<ModelCollider>();
		var renderer = ModelObject.Components.Get<ModelRenderer>();

		collider.Model = renderer.Model;

		if (ItemDefenition is not null)
		{
			if (ItemDefenition.Model is not null)
			{
				renderer.Model = Model.Load(ItemDefenition.Model);
				collider.Model = renderer.Model;
			}
		}
		
		base.OnAwake();
	}
}
