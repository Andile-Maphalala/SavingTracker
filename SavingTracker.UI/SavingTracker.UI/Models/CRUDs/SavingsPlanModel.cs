using System.ComponentModel.DataAnnotations;

namespace SavingTracker.UI.Models.CRUDs
{
    public class SavingsPlanModel
    {
        [Display(Name = "Id", Order = 0)]

        public int Id { get; set; }

        [Display(Name = "Name", Order = 1)]
        public string Name { get; set; }


        [Display(Name = "Description", Order = 2)]
        public string? Description { get; set; }

        [Display(Name = "TargetAmount", Order = 3)]
        public decimal TargetAmount { get; set; }

        [Display(Name = "StartDate", Order = 4)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "EndDate", Order = 5)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]

        public DateTime? EndDate { get; set; }
    }
}
