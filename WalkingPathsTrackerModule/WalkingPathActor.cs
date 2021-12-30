using System.Linq;
using NetTopologySuite.Geometries;
using OSMLSGlobalLibrary.Map;
using OSMLSGlobalLibrary.Observable.Geometries.Actor;
using OSMLSGlobalLibrary.Observable.Property;

namespace WalkingPathsTrackerModule
{
	[CustomStyle(@"new style.Style({
                stroke: new style.Stroke({
                    color: 'rgba(90, 0, 157, 1)',
                    width: 2
                })
            });
        ")]
	public class WalkingPathActor : LineStringActor
	{
		public WalkingPathActor(Coordinate[] coordinates) : base(coordinates)
		{
		}

		[ObservableProperty("Coordinates", false)]
		public string Coordinates => string.Join(
			"; ",
			CoordinateSequence.ToCoordinateArray().Select(coordinate => $"({coordinate.X.ToString()}. {coordinate.Y})")
		);

		[ObservableProperty("Number of uses", false)]
		public int NumberOfUses { get; set; } = 1;
	}
}