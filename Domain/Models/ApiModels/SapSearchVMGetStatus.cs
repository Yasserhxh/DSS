namespace Domain.Models.ApiModels;
public class SapSearchVMGetStatus
{
    public string customerSap { get; set; }
    public string creditControlArea { get; set; }
    public string KUNAG_High { get; set; }
    public string KUNAG_Low { get; set; }
    public string KUNAG_Sign { get; set; }
    public string KUNAG_Option { get; set; }
    public string SPTAG_High { get; set; }
    public string SPTAG_Low { get; set; }
    public string SPTAG_Sign { get; set; }
    public string SPTAG_Option { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }

}
