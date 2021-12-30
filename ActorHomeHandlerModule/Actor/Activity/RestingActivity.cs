using ActorModule.Actor.Activity;

namespace ActorHomeHandlerModule.Actor.Activity
{
	public class RestingActivity : ActivityBase
	{
		public RestingActivity(ActorModule.Actor.Actor actor) : base(actor)
		{
		}

		public override string Name => "Resting at home";

		public override int Priority => (int)Actor.HungerStatus * 30 + (int)Actor.FatigueStatus * 30;

		public override (bool isActivityEnded, ActivityBase? nextActivitySuggested) Update()
		{
			Actor.Hunger -= 0.001;
			Actor.Fatigue -= 0.001;

			return (false, null);
		}
	}
}