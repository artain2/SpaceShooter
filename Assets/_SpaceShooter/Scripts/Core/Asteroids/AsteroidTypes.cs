using System.Collections.Generic;

namespace SpaceShooter.Core.Asteroids
{
    public static class AsteroidTypes
    {
        public static IReadOnlyList<string> AllTypes = new[] {Common, Hard, Snowball, Splitter};
        
        public const string Common = "Common";
        public const string Hard = "Hard";
        public const string Snowball = "Snowball";
        public const string Splitter = "Splitter";
    }
}