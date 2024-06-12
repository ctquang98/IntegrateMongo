namespace IntegrateMongo1.Models
{
    public class GetAllParams
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 3;
        public string filterField { get; set; } = "";
        public string filterValue { get; set; } = "";
        public string sortField { get; set; } = "";
        public bool sortAsc { get; set; } = true;
    }
}
