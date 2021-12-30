using ActorModule.Actor.Activity;

namespace ActorHungerHandlerModule.Actor.Activity
{
	public class RestingActivity : ActivityBase
	{
		public RestingActivity(ActorModule.Actor.Actor actor) : base(actor)
		{
		}

		public override string Name => "Eating in public place";

		public override int Priority => (int)Actor.HungerStatus * 52;

		public override (bool isActivityEnded, ActivityBase? nextActivitySuggested) Update()
		{
			Actor.Hunger -= 0.002;

			return (false, null);
		}
	}
}