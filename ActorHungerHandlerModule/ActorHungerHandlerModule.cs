using System;
using System.Collections.Immutable;
using System.Linq;
using ActorHungerHandlerModule.Actor.Activity;
using CityDataExpansionModule.OsmGeometries;
using NetTopologySuite.Geometries;
using OSMLSGlobalLibrary.Modules;

namespace ActorHungerHandlerModule
{
	public class ActorHungerHandlerModule : OSMLSModule
	{
		protected override void Initialize()
		{
			CafesAndRestaurantsCoordinates =
				MapObjects.GetAll<OsmNode>()
					.Cast<IOsmObject>()
					.Concat(MapObjects.GetAll<OsmClosedWay>())
					.Where(osmObject =>
						osmObject.Tags.TryGetValue("amenity", out var value) &&
						value is "cafe" or "restaurant" or "fast_food"
					).Cast<Geometry>()
					.Select(geometry => geometry.Coordinates.First())
					.ToImmutableList();

			if (CafesAndRestaurantsCoordinates.IsEmpty)
				throw new InvalidOperationException("There are no places to eat on the map section.");
		}

		private ImmutableList<Coordinate>? CafesAndRestaurantsCoordinates { get; set; }

		public override void Update(long elapsedMilliseconds)
		{
			MapObjects.GetAll<ActorModule.Actor.Actor>().ForEach(actor =>
			{
				var closestPlaceCoordinate = CafesAndRestaurantsCoordinates!
					.OrderBy(coordinate => coordinate.Distance(actor.Coordinate))
					.First();

				actor.TrySetNewActivity(new WalkingActivity(actor, closestPlaceCoordinate));
			});
		}
	}
}