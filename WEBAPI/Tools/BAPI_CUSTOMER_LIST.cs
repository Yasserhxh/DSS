namespace WEBAPI.Tools
{
    public class BAPI_CUSTOMER_LIST
    {
        public CompanyDetails[] Details { get; set; }
    }

    public class CompanyDetails
    {
        public string CompanyCode { get; set; }

        public string Name { get; set; }
    }
}
