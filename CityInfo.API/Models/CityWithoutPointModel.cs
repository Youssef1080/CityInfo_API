namespace CityInfo.API.Models
{
    /// <summary>
    /// city class but without the points of Interests List
    /// </summary>
    public class CityWithoutPointModel
    {
        /// <summary>
        /// identifier of city
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// the name of the city
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// information about the city
        /// </summary>
        public string? Description { get; set; }
    }
}