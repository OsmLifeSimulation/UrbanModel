using ActorModule.Actor.Activity;

namespace ActorHomeHandlerModule.Actor.Activity
{
	public class WalkingActivity : WalkingActivityBase
	{
		public WalkingActivity(ActorModule.Actor.Actor actor) : base(actor, actor.HomePoint!.Coordinate)
		{
		}

		public override string Name => "Going home";

		public override int Priority => (int)Actor.HungerStatus * 30 + (int)Actor.FatigueStatus * 30;

		public override (bool isActivityEnded, ActivityBase? nextActivitySuggested) Update()
		{
			var baseUpdateResult = base.Update();

			return baseUpdateResult.isActivityEnded
				? (true, new RestingActivity(Actor))
				: baseUpdateResult;
		}
	}
}