using System.Linq;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using PathsFindingCoreModule;

namespace ActorModule.Actor.Activity
{
	public abstract class WalkingActivityBase : ActivityBase
	{
		public WalkingActivityBase(Actor actor, Coordinate targetCoordinate) : base(actor)
		{
			TargetCoordinate = targetCoordinate;
		}

		protected Coordinate TargetCoordinate { get; }

		private Task<LineString> GetPath() => PathsFinding.GetPath(
			Actor.Coordinate,
			TargetCoordinate,
			"Walking"
		).ContinueWith(task =>
			new LineString(
				task.Result.Coordinates
					//.Prepend(Actor.Coordinate)
					.Append(TargetCoordinate)
					.ToArray()
			)
		);

		private Task<LineString>? PathFindingTask { get; set; }

		public LineString? Path { get; private set; }

		private int? NextPointIndex { get; set; }

		private Coordinate? NextCoordinate =>
			NextPointIndex == null ? null : Path?.GetCoordinateN(NextPointIndex.Value);

		public override (bool isActivityEnded, ActivityBase? nextActivitySuggested) Update()
		{
			if (Actor.Coordinate.Equals(TargetCoordinate))
				return (true, null);

			if (Path == null)
			{
				PathFindingTask ??= GetPath();

				if (PathFindingTask.IsCompletedSuccessfully)
					Path = PathFindingTask.Result;
				else if (PathFindingTask.IsCompleted)
					PathFindingTask = null;
			}
			else
			{
				NextPointIndex ??= 0;

				if (Actor.Coordinate.Equals(NextCoordinate))
					if (NextPointIndex == Path.NumPoints - 1)
						return (true, null);
					else
						NextPointIndex++;

				MovePointTowards(Actor, NextCoordinate!, Actor.Speed);

				Actor.Hunger += 0.0005;
				Actor.Fatigue += 0.0005;
			}

			return (false, null);
		}

		private static void MovePointTowards(Point point, Coordinate target, double distance)
		{
			// var vector = new Point(target.X - point.X, target.Y - point.Y);
			// var length = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			// var unitVector = new Point(vector.X / length, vector.Y / length);
			//
			// point.X += unitVector.X * distance;
			// point.Y += unitVector.Y * distance;

			if (point.X < target.X)
				if (point.X + distance >= target.X)
					point.X = target.X;
				else
					point.X += distance;
			else if (point.X > target.X)
				if (point.X - distance <= target.X)
					point.X = target.X;
				else
					point.X -= distance;

			if (point.Y < target.Y)
				if (point.Y + distance >= target.Y)
					point.Y = target.Y;
				else
					point.Y += distance;
			else if (point.Y > target.Y)
				if (point.Y - distance <= target.Y)
					point.Y = target.Y;
				else
					point.Y -= distance;
		}
	}
}