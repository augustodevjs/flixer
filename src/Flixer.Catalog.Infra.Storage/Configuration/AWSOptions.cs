namespace Flixer.Catalog.Infra.Storage.Configuration;

public class AwsOptions
{
    public string Region { get; set; } = null!;
    public string AccessKeyId { get; set; } = null!;
    public string SecretAccessKey { get; set; } = null!;
}