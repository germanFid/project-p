using ProjectP.ItemSystem;
using Sandbox;

public sealed class DebugItems : Component
{
	protected override void OnStart()
	{
		var lib = ResourceLibrary.GetAll<Item>();
		Log.Info("Items Loaded: " + lib.Count());
		foreach (var item in lib)
		{
			Log.Info(item.Name);
		}
		base.OnStart();
	}
}
