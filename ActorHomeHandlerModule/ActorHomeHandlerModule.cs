using ActorHomeHandlerModule.Actor.Activity;
using OSMLSGlobalLibrary.Modules;

namespace ActorHomeHandlerModule
{
	public class ActorHomeHandlerModule : OSMLSModule
	{
		protected override void Initialize()
		{
		}

		public override void Update(long elapsedMilliseconds)
		{
			MapObjects.GetAll<ActorModule.Actor.Actor>().ForEach(actor =>
			{
				if (actor.HomePoint == null) return;

				actor.TrySetNewActivity(new WalkingActivity(actor));
			});
		}
	}
}