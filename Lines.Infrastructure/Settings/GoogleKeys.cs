namespace Lines.Infrastructure.Settings;
public class GoogleKeys
{
    public string Client_Id { get; set; } = string.Empty;
    public string Project_Id { get; set; } = string.Empty;
    public string Auth_Uri { get; set; } = string.Empty;
    public string Token_Uri { get; set; } = string.Empty;
    public string Auth_Provider_x509_cert_url { get; set; } = string.Empty;
    public string Client_Secret { get; set; } = string.Empty;
    public List<string> Redirect_Uris { get; set; } = [];
    public List<string> Javascript_Origins { get; set; } = [];
}