namespace Domain.Models
{
    public class ArticleModel
    {
        public int Article_Id { get; set; }
        public string Designation { get; set; }
        public double? Tarif { get; set; }    
        public int Region { get; set; }
        public VilleModel Ville { get; set; }
    }
}
