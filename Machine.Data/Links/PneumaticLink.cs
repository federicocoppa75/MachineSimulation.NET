using System.ComponentModel.DataAnnotations.Schema;

namespace Machine.Data.Links
{
    [Table("PneumaticLink")]
    public class PneumaticLink : Link
    {
        public double OffPos { get; set; }
        public double OnPos { get; set; }

        /// <summary>
        /// Intervallo di tempo per passare dalla posizione ON alla posizione OFF (millisecondi).
        /// </summary>
        public double TOff { get; set; }

        /// <summary>
        /// Intervallo di tempo per passare dalla posizione OFF alla posizione ON (millisecondi).
        /// </summary>
        public double TOn { get; set; }

        /// <summary>
        /// Flag che indica che l'attivazione del link implica l'attivazione dell'utensile sottostante
        /// </summary>
        public bool ToolActivator { get; set; }
    }
}
