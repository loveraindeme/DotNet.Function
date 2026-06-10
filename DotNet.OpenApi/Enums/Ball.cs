using System.Text.Json.Serialization;

namespace DotNet.OpenApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<Ball>))]
    public enum Ball
    {
        Basketball,

        Football,

        Volleyball,

        Golfball,

        Tennis,

        Billiards,

        Badminton,
    }
}
