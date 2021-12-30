using OSMLSGlobalLibrary.Modules;

namespace ActorModule
{
	[CustomInitializationOrder(-100)]
	public class ActorModule : OSMLSModule
	{
		protected override void Initialize()
		{
			// See actors initialization in ActorInitializationModule project.
		}

		public override void Update(long elapsedMilliseconds) =>
			MapObjects.GetAll<Actor.Actor>().ForEach(actor => actor.Update());
	}
}