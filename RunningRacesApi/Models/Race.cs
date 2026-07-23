namespace RunningRacesApi.Models
{
    /// <summary>
    /// Futóverseny adatai
    /// </summary>
    public class Race
    {
        /// <summary>
        /// Egyedi azonosító
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Verseny neve
        /// </summary>
        /// <example>Budapest Marathon</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Verseny dátuma
        /// </summary>
        /// <example>2025-10-05</example>
        public DateTime Date { get; set; }

        /// <summary>
        /// Helyszín városa
        /// </summary>
        /// <example>Budapest</example>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Távolság kilométerben
        /// </summary>
        /// <example>42.2</example>
        public double Distance { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
    }
}