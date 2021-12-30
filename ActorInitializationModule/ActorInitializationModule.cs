using System;
using System.Collections.Immutable;
using System.Linq;
using ActorModule.Actor;
using ActorModule.Actor.State;
using CityDataExpansionModule.OsmGeometries;
using MoreLinq;
using OSMLSGlobalLibrary.Modules;

namespace ActorInitializationModule
{
	[CustomInitializationOrder(-10)]
	public class ActorInitializationModule : OSMLSModule
	{
		protected override void Initialize()
		{
			var random = new Random();
			const int actorsCountOnOneLevelPerEntrance = 3;

			var apartmentsBuildings = MapObjects.GetAll<OsmClosedWay>()
				.Where(way =>
					way.Tags.TryGetValue("building", out var value) && value is "apartments"
				)
				.ToImmutableList();

			if (apartmentsBuildings.IsEmpty)
				throw new InvalidOperationException("The map area must contain apartments buildings.");

			var workBuildingsEntrances = MapObjects.GetAll<OsmClosedWay>()
				.Where(way =>
					way.Tags.TryGetValue("building", out var value) && value is "office" or "retail"
				).Select(way => way.Nodes.First(node => node.Tags.ContainsKey("entrance")))
				.ToImmutableList();

			if (workBuildingsEntrances.IsEmpty)
				throw new InvalidOperationException("The map area must contain work entrances.");

			var actors = apartmentsBuildings.Select(building =>
				{
					building.Tags.TryGetValue("building:levels", out var levelsCountString);
					var levelsCount = levelsCountString == null ? 1 : int.Parse(levelsCountString);

					var homeEntrances =
						building.Nodes.Where(node => node.Tags.ContainsKey("entrance")).ToImmutableList();

					return (levelsCount, homeEntrances);
				})
				.SelectMany(buildingParameters =>
				{
					var (levelsCount, homeEntrances) = buildingParameters;

					return homeEntrances.Select(homeEntrance =>
					{
						var actorsPerEntrance = levelsCount * actorsCountOnOneLevelPerEntrance;

						return Enumerable.Range(0, actorsPerEntrance)
							.Select(_ => new Actor(homeEntrance.Coordinate.Copy()))
							.Select(actor =>
							{
								double GenerateRandomDouble(double minValue, double maxValue) =>
									random.NextDouble() * (maxValue - minValue) + minValue;

								actor.Hunger = GenerateRandomDouble(0, 54);
								actor.Fatigue = GenerateRandomDouble(0, 54);
								actor.Speed = GenerateRandomDouble(1, 3);
								actor.HomePoint = homeEntrance;

								actor.JobTimeState = new PastimeState(
									TimeSpan.FromHours(8),
									TimeSpan.FromHours(16),
									workBuildingsEntrances.Shuffle().First()
								);

								return actor;
							});
					});
				})
				.SelectMany(actors => actors)
				.ToImmutableList();

			if (actors.IsEmpty)
				throw new InvalidOperationException(
					"Not a single actor has been created. " +
					"Perhaps there are no apartments buildings with entrances on the map area."
				);

			actors.ForEach(actor => MapObjects.Add(actor));
		}

		public override void Update(long elapsedMilliseconds)
		{
		}
	}
}