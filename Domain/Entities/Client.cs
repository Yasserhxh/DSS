using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Client")]
    public class Client
    {
        [Key]
        public int Client_Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string CodeClientSap { get; set; }
        [ForeignKey("Forme_Juridique")]
        public int FormeJuridique_Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Ice { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Cnie { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Gsm { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string RaisonSociale { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string Adresse { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Destinataire_Interlocuteur { get; set; }
        [ForeignKey("Ville")]
        public int? IdVille { get; set; }
        [ForeignKey("Pays")]
        public int? IdPays { get; set; }
        [ForeignKey("Chantier")]
        public int Client_Ctn_Id { get; set; }

        public Ville Ville { get; set; }
        public Pays Pays { get; set; }
        public FormeJuridique Forme_Juridique { get; set; }
        public Chantier Chantier { get; set; }
    }
}
