using System;
using NetTopologySuite.Geometries;

namespace ActorModule.Actor.State
{
	public record PastimeState(TimeSpan StartTime, TimeSpan EndTime, Point Point);
}