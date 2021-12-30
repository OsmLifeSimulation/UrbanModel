using ActorModule.Actor.Activity;
using NetTopologySuite.Geometries;

namespace ActorFreeTimeHandlerModule.Actor.Activity
{
	public class WalkingActivity : WalkingActivityBase
	{
		public WalkingActivity(ActorModule.Actor.Actor actor, Coordinate targetPlace) : base(actor, targetPlace)
		{
		}

		public override string Name => "Going to rest in public resting place";

		public override int Priority => (int)Actor.FatigueStatus * 52;

		public override (bool isActivityEnded, ActivityBase? nextActivitySuggested) Update()
		{
			var baseUpdateResult = base.Update();

			return baseUpdateResult.isActivityEnded
				? (true, new RestingActivity(Actor))
				: baseUpdateResult;
		}
	}
}