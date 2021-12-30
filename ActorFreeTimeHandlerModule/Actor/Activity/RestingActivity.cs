using ActorModule.Actor.Activity;

namespace ActorFreeTimeHandlerModule.Actor.Activity
{
	public class RestingActivity : ActivityBase
	{
		public RestingActivity(ActorModule.Actor.Actor actor) : base(actor)
		{
		}

		public override string Name => "Resting in public resting place";

		public override int Priority => (int)Actor.FatigueStatus * 52;

		public override (bool isActivityEnded, ActivityBase? nextActivitySuggested) Update()
		{
			Actor.Fatigue -= 0.002;

			return (false, null);
		}
	}
}