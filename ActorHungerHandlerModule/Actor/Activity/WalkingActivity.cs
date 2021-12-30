using ActorModule.Actor.Activity;
using NetTopologySuite.Geometries;

namespace ActorHungerHandlerModule.Actor.Activity
{
	public class WalkingActivity : WalkingActivityBase
	{
		public WalkingActivity(ActorModule.Actor.Actor actor, Coordinate targetPlace) : base(actor, targetPlace)
		{
		}

		public override string Name => "Going to eat in public place";

		public override int Priority => (int)Actor.HungerStatus * 52;

		public override (bool isActivityEnded, ActivityBase? nextActivitySuggested) Update()
		{
			var baseUpdateResult = base.Update();

			return baseUpdateResult.isActivityEnded
				? (true, new RestingActivity(Actor))
				: baseUpdateResult;
		}
	}
}