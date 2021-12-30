using ActorModule.Actor.Activity;
using ActorModule.Actor.State;

namespace ActorModule.Actor
{
	internal interface IActor : IActorMainState
	{
		ActivityBase? CurrentActivity { get; }

		bool TrySetNewActivity(ActivityBase activity);

		void Update();
	}
}