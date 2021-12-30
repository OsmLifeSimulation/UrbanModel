using NetTopologySuite.Geometries;

namespace ActorModule.Actor.State
{
	internal interface IActorMainState
	{
		double Hunger { get; set; }

		HungerStatus HungerStatus { get; }

		double Fatigue { get; set; }

		FatigueStatus FatigueStatus { get; }

		double Speed { get; set; }

		Point? HomePoint { get; set; }

		PastimeState? JobTimeState { get; set; }
	}

	public enum HungerStatus
	{
		Satisfied,
		ALittleHungry,
		PrettyHungry,
		VeryHungry,
		Starving
	}

	public enum FatigueStatus
	{
		Rested,
		Normal,
		Tired,
		VeryTired,
		Exhausted
	}
}