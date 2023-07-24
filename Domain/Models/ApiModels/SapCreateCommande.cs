namespace Domain.Models.ApiModels;
public class SapCreateCommande
{
    public string SALES_ORG { get; set; }
    public string PARTN_NUMB { get; set; }
    public string PLANT { get; set; }
    public string CodeArticle { get; set; }
    public decimal QuantiteArticle { get; set; }
}
