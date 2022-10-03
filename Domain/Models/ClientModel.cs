using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class ClientModel
    {
        public int Client_Id { get; set; }
        public string CodeClientSap { get; set; }
        public int FormeJuridique_Id { get; set; }
        public string Ice { get; set; }
        public string Cnie { get; set; }
        public string Gsm { get; set; }
        public string RaisonSociale { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string Destinataire_Interlocuteur { get; set; }
        public int? IdVille { get; set; }
        public int? IdPays { get; set; }
        public int Client_Ctn_Id { get; set; }

        public VilleModel Ville { get; set; }
        public PaysModel Pays { get; set; }
        public FormeJuridiqueModel Forme_Juridique { get; set; }
        public ChantierModel Chantier { get; set; }
    }
}
