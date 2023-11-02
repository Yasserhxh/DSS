namespace Domain.Models.Commande;
public class CommandesPDFViewModel
{
    public CommandeModel commande { get; set; }
    public List<ValidationModel> validations { get; set; }
    public UserModel user { get; set; }
}
