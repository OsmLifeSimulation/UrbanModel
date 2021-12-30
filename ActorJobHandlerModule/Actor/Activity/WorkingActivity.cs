using System;
using ActorModule.Actor.Activity;

namespace ActorJobHandlerModule.Actor.Activity
{
	public class WorkingActivity : ActivityBase
	{
		public WorkingActivity(ActorModule.Actor.Actor actor) : base(actor)
		{
		}

		public override string Name => "Working";

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
			Actor.Hunger += 0.0005;
			Actor.Fatigue += 0.0005;

			return (false, null);
		}
	}
}