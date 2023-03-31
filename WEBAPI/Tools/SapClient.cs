using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBAPI.Tools
{
    public class SapClient : Client
    {
        [InverseProperty("AssociatedSapClient")]
        public DSSClient AssociatedDssClient { get; set; }
    }
}
