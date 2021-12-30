using System;
using ActorModule.Actor.Activity;
using ActorModule.Actor.State;
using NetTopologySuite.Geometries;
using OSMLSGlobalLibrary.Map;
using OSMLSGlobalLibrary.Observable.Geometries.Actor;
using OSMLSGlobalLibrary.Observable.Property;

namespace ActorModule.Actor
{
	[CustomStyle(@"new style.Style({
                image: new style.Circle({
                    opacity: 1.0,
                    scale: 1.0,
                    radius: 5,
                    fill: new style.Fill({
                      color: 'rgba(0, 255, 0, 0.9)'
                    }),
                    stroke: new style.Stroke({
                      color: 'rgba(0, 0, 0, 0.4)',
                      width: 1
                    }),
                })
            });
        ")]
	public class Actor : PointActor, IActor
	{
		public Actor(Coordinate coordinate) : base(coordinate)
		{
		}

		/// <inheritdoc/>
		[ObservableProperty("Hunger", true)]
		public double Hunger { get; set; }

		/// <inheritdoc/>
		public HungerStatus HungerStatus => Hunger switch
		{
			// ((24 - 9) * 60 * 60) / 5 = 10800
			< 10.8 => HungerStatus.Satisfied,
			>= 10.8 and < 10.8 * 2 => HungerStatus.ALittleHungry,
			>= 10.8 * 2 and < 10.8 * 3 => HungerStatus.PrettyHungry,
			>= 10.8 * 3 and < 10.8 * 4 => HungerStatus.VeryHungry,
			>= 10.8 * 4 => HungerStatus.Starving,
			_ => throw new ArgumentOutOfRangeException()
		};

		[ObservableProperty("Hunger Status", false)]
		public string VerbalHungerStatus => HungerStatus switch
		{
			HungerStatus.Satisfied => "Satisfied",
			HungerStatus.ALittleHungry => "A little hungry",
			HungerStatus.PrettyHungry => "Pretty hungry",
			HungerStatus.VeryHungry => "Very hungry",
			HungerStatus.Starving => "Starving",
			_ => throw new ArgumentOutOfRangeException()
		};

		/// <inheritdoc/>
		[ObservableProperty("Fatigue", true)]
		public double Fatigue { get; set; }

		/// <inheritdoc/>
		public FatigueStatus FatigueStatus => Fatigue switch
		{
			// ((24 - 9) * 60 * 60) / 5 = 10800
			< 10.8 => FatigueStatus.Rested,
			>= 10.8 and < 10.8 * 2 => FatigueStatus.Normal,
			>= 10.8 * 2 and < 10.8 * 3 => FatigueStatus.Tired,
			>= 10.8 * 3 and < 10.8 * 4 => FatigueStatus.VeryTired,
			>= 10.8 * 4 => FatigueStatus.Exhausted,
			_ => throw new ArgumentOutOfRangeException()
		};

		[ObservableProperty("Fatigue Status", false)]
		public string VerbalFatigueStatus => FatigueStatus switch
		{
			FatigueStatus.Rested => "Rested",
			FatigueStatus.Normal => "Normal",
			FatigueStatus.Tired => "Tired",
			FatigueStatus.VeryTired => "VeryTired",
			FatigueStatus.Exhausted => "Exhausted",
			_ => throw new ArgumentOutOfRangeException()
		};

		/// <inheritdoc/>
		[ObservableProperty("Speed", true)]
		public double Speed { get; set; }

		/// <inheritdoc/>
		public Point? HomePoint { get; set; }

		/// <inheritdoc/>
		public PastimeState? JobTimeState { get; set; }

		/// <inheritdoc/>
		public ActivityBase? CurrentActivity { get; private set; }

		[ObservableProperty("Activity", false)]
		public string ActivityName => CurrentActivity?.Name ?? "-";

		/// <inheritdoc/>
		public bool TrySetNewActivity(ActivityBase activity)
		{
			if (activity.Actor != this)
				throw new ArgumentException(
					"The actor in the activity must be the same " +
					"as the one to which this activity is trying to be assigned."
				);

			if (CurrentActivity != null && activity.Priority <= CurrentActivity.Priority)
				return false;

			CurrentActivity = activity;

			return true;
		}

		private void UpdateCurrentActivity()
		{
			var activityUpdateResult = CurrentActivity?.Update();

			if (activityUpdateResult == null)
				return;

			if (activityUpdateResult.Value.isActivityEnded)
				CurrentActivity = null;

			if (activityUpdateResult.Value.nextActivitySuggested != null)
				TrySetNewActivity(activityUpdateResult.Value.nextActivitySuggested);
		}

		private void UpdateNeeds()
		{
			Hunger += 0.0005;
			Fatigue += 0.0005;
		}

		public void Update()
		{
			UpdateNeeds();
			UpdateCurrentActivity();
		}
	}
}