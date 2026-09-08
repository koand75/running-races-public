using RunningRacesApi.Enums;

namespace RunningRacesApi.Models
{
    /// <summary>
    /// Base data
    /// </summary>
    public class Race
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Race name
        /// </summary>
        /// <example>Budapest Marathon</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Race date
        /// </summary>
        /// <example>2025-10-05</example>
        public DateTime Date { get; set; }

        /// <summary>
        /// Race start date (or event date if it's a single day)
        /// </summary>
        /// <example>2025-10-05</example>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Race start date (or null date if it's a single day) 
        /// </summary>
        /// <example>2025-10-05</example>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// City of location
        /// </summary>
        /// <example>Budapest</example>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Distance in km
        /// </summary>
        /// <example>42.2</example>
        public double Distance { get; set; }

        /// <summary>
        /// Soft delete mark
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Last modification date
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// Relay ot individual
        /// </summary>
        public RaceType RaceType { get; set; }

        /// <summary>
        /// Connected teams
        /// </summary>
        public ICollection<Team> Teams { get; set; } = new List<Team>();

        /// <summary>
        /// Connected section
        /// </summary>
        public ICollection<Section> Sections { get; set; } = new List<Section>();

        /// <summary>
        /// Categories in race for example 5/7/10km
        /// </summary>
        public ICollection<RaceCategory> Categories { get; set; } = new List<RaceCategory>();
    }
}