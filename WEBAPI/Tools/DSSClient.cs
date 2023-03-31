using Domain.Entities;

namespace WEBAPI.Tools
{
    public class DSSClient : Client
    {

        public SapClient AssociatedSapClient { get; set; }
        public int? AssociatedSapClientId { get; set; }
    }
}
