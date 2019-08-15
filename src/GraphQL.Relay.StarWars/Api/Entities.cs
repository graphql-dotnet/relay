using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQL.Relay.StarWars.Api
{
    public static class UriListExtensions {
        public static IEnumerable<string> ToIds (this IList<Uri> list) {
            return list
                .Select(url => url?.ToString().Split('/')[5]);
        }
    }
    public interface ISwapiResponse {}

    public class EntityList<TEntity>: ISwapiResponse {

        public int Count { get; set; }
        public Uri Next { get; set; }
        public Uri Previous { get; set; }
        public List<TEntity> Results = new List<TEntity>();
    }

    public class Entity: ISwapiResponse
    {
        public string Id => Url.ToString()?.Split('/')[5];
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public Uri Url { get; set; }
    }

    public class Films : Entity
    {
        public string Title { get; set; }
        public int EpisodeId { get; set; }
        public string OpeningCrawl { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }
        public string ReleaseDate { get; set; }
        public IList<Uri> Characters { get; set; }
        public IList<Uri> Planets { get; set; }
        public IList<Uri> Starships { get; set; }
        public IList<Uri> Vehicles { get; set; }
        public IList<Uri> Species { get; set; }
    }

    public class Planets: Entity
    {
        public string Name { get; set; }
        public string RotationPeriod { get; set; }
        public string OrbitalPeriod { get; set; }
        public string Diameter { get; set; }
        public string Climate { get; set; }
        public string Gravity { get; set; }
        public string Terrain { get; set; }
        public string SurfaceWater { get; set; }
        public string Population { get; set; }
        public IList<Uri> Residents { get; set; }
        public IList<Uri> Films { get; set; }
    }

    public class Species: Entity
    {
        public string Name { get; set; }
        public string Classification { get; set; }
        public string Designation { get; set; }
        public string AverageHeight { get; set; }
        public string SkinColors { get; set; }
        public string HairColors { get; set; }
        public string EyeColors { get; set; }
        public string AverageLifespan { get; set; }
        public Uri Homeworld { get; set; }
        public string Language { get; set; }
        public IList<Uri> People { get; set; }
        public IList<Uri> Films { get; set; }
    }


    public class People: Entity
    {
        public string Name { get; set; }
        public string Height { get; set; }
        public string Mass { get; set; }
        public string HairColor { get; set; }
        public string SkinColor { get; set; }
        public string EyeColor { get; set; }
        public string BirthYear { get; set; }
        public string Gender { get; set; }
        public Uri Homeworld { get; set; }
        public IList<Uri> Films { get; set; }
        public IList<Uri> Species { get; set; }
        public IList<Uri> Vehicles { get; set; }
        public IList<Uri> Starships { get; set; }
    }

    public class Starships: Entity
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string CostInCredits { get; set; }
        public string Length { get; set; }
        public string MaxAtmospheringSpeed { get; set; }
        public int Crew { get; set; }
        public int Passengers { get; set; }
        public string CargoCapacity { get; set; }
        public string Consumables { get; set; }
        public double HyperdriveRating { get; set; }
        public string MGLT { get; set; }
        public string StarshipClass { get; set; }
        public IList<Uri> Pilots { get; set; }
        public IList<Uri> Films { get; set; }
    }

    public class Vehicles: Entity
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string CostInCredits { get; set; }
        public string Length { get; set; }
        public string MaxAtmospheringSpeed { get; set; }
        public string Crew { get; set; }
        public string Passengers { get; set; }
        public string CargoCapacity { get; set; }
        public string Consumables { get; set; }
        public string VehicleClass { get; set; }
        public IList<Uri> Pilots { get; set; }
        public IList<Uri> Films { get; set; }
    }
}