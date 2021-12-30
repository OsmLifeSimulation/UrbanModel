using ActorJobHandlerModule.Actor.Activity;
using OSMLSGlobalLibrary.Modules;

namespace ActorJobHandlerModule
{
	public class ActorJobHandlerModule : OSMLSModule
	{
		protected override void Initialize()
		{
		}

		public override void Update(long elapsedMilliseconds)
		{
			MapObjects.GetAll<ActorModule.Actor.Actor>().ForEach(actor =>
			{
				if (actor.JobTimeState == null) return;

				actor.TrySetNewActivity(new WalkingActivity(actor));
			});
		}
	}
}