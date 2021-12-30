namespace ActorModule.Actor.Activity
{
	public abstract class ActivityBase
	{
		public ActivityBase(Actor actor)
		{
			Actor = actor;
		}

		public abstract string Name { get; }

		public Actor Actor { get; }

		public abstract int Priority { get; }

		public abstract (bool isActivityEnded, ActivityBase? nextActivitySuggested) Update();
	}
}