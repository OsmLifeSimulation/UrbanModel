using System;
using ActorModule.Actor.Activity;

namespace ActorJobHandlerModule.Actor.Activity
{
	public class WalkingActivity : WalkingActivityBase
	{
		public WalkingActivity(ActorModule.Actor.Actor actor) : base(actor, actor.JobTimeState!.Point.Coordinate)
		{
		}

		public override string Name => "Going to work";

		public override int Priority
		{
			get
			{
				var currentTime = DateTime.Now.TimeOfDay;
				var isJobTime = currentTime > Actor.JobTimeState!.StartTime && currentTime < Actor.JobTimeState.EndTime;

				return isJobTime ? 100 : 10;
			}
		}

		public override (bool isActivityEnded, ActivityBase? nextActivitySuggested) Update()
		{
			var baseUpdateResult = base.Update();

			return baseUpdateResult.isActivityEnded
				? (true, new WorkingActivity(Actor))
				: baseUpdateResult;
		}
	}
}