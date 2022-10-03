using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Commande
{
    public class CommandeViewModel
    {
        public CommandeModel Commande { get; set; }
        public List<DetailCommandeModel> DetailCommandes { get; set; }
        public ClientModel Client { get; set; }
        public ChantierModel Chantier { get; set; }
    }
}
