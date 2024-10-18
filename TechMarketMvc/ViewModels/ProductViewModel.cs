using System.Collections.Generic;
using TechMarketMvc.Models;

namespace TechMarketMvc.ViewModels
{
    public class ProductViewModel
    {
        public IEnumerable<Computer> Computers { get; set; } = new List<Computer>();
        public IEnumerable<Phone> Phones { get; set; } = new List<Phone>();
        public IEnumerable<Smartwatch> Smartwatches { get; set; } = new List<Smartwatch>();
    }
}
