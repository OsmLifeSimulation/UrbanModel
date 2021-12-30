using System;
using System.Collections.Immutable;
using System.Linq;
using ActorFreeTimeHandlerModule.Actor.Activity;
using CityDataExpansionModule.OsmGeometries;
using NetTopologySuite.Geometries;
using OSMLSGlobalLibrary.Modules;

namespace ActorFreeTimeHandlerModule
{
	public class ActorFreeTimeHandlerModule : OSMLSModule
	{
		protected override void Initialize()
		{
			PlacesOfRestCoordinates =
				MapObjects.GetAll<OsmNode>()
					.Cast<IOsmObject>()
					.Concat(MapObjects.GetAll<OsmClosedWay>())
					.Where(osmObject =>
						osmObject.Tags.ContainsKey("leisure") ||
						osmObject.Tags.TryGetValue("amenity", out var value) && value is 
							"internet_cafe" or "bar" or "cinema" or "theatre"
					).Cast<Geometry>()
					.Select(geometry => geometry.Coordinates.First())
					.ToImmutableList();

			if (PlacesOfRestCoordinates.IsEmpty)
				throw new InvalidOperationException("There are no places to rest on the map section.");
		}

		private ImmutableList<Coordinate>? PlacesOfRestCoordinates { get; set; }

		public override void Update(long elapsedMilliseconds)
		{
			MapObjects.GetAll<ActorModule.Actor.Actor>().ForEach(actor =>
			{
				var closestPlaceCoordinate = PlacesOfRestCoordinates!
					.OrderBy(coordinate => coordinate.Distance(actor.Coordinate))
					.First();

				actor.TrySetNewActivity(new WalkingActivity(actor, closestPlaceCoordinate));
			});
		}
	}
}