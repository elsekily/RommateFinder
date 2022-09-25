using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace RoommateFinderAPI.Entities.Resources
{
    public class LocationUtilities
    {
        public const int SRID = 4326;
        public static Point GetLocation(double latitude, double longitude)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(SRID);
            return geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(latitude, longitude));
        }
    }
}