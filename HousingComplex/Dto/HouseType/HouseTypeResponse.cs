using HousingComplex.Entities;

namespace HousingComplex.Dto.HouseType
{
    public class HouseTypeResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long Price { get; set; }
        public int StockUnit { get; set; }
        public Spesification? Spesification { get; set; }
    }
}
