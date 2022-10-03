using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Client
{
    public class ClientSearchVm
    {
        public string Ice { get; set; }
        public string Cnie { get; set; }
        public string RS { get; set; }
        public IEnumerable<ClientModel> Clients { get; set; }
    }
}
