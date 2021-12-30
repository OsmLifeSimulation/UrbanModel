using System.Collections.Generic;
using System.Linq;
using ActorModule.Actor;
using ActorModule.Actor.Activity;
using OSMLSGlobalLibrary.Modules;

namespace WalkingPathsTrackerModule
{
	public class WalkingPathsTrackerModule : OSMLSModule
	{
		protected override void Initialize()
		{
		}

		private IList<WalkingActivityBase> CheckedActivities { get; } = new List<WalkingActivityBase>();

		public override void Update(long elapsedMilliseconds)
		{
			MapObjects.GetAll<Actor>().ForEach(actor =>
			{
				if (actor.CurrentActivity is not WalkingActivityBase activity ||
				    activity.Path == null ||
				    CheckedActivities.Contains(activity))
					return;

				var existingPath = MapObjects.Get<WalkingPathActor>()
					.SingleOrDefault(path => path.EqualsTopologically(activity.Path));

				if (existingPath == null)
					MapObjects.Add(new WalkingPathActor(activity.Path.Coordinates));
				else
					existingPath.NumberOfUses++;

				CheckedActivities.Add(activity);
			});
		}
	}
}