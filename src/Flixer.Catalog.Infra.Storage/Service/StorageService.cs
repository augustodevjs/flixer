using Amazon.S3;
using Amazon.Runtime;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Application.Intefaces;
using Flixer.Catalog.Infra.Storage.Configuration;

namespace Flixer.Catalog.Infra.Storage.Service;

public class StorageService : IStorageService
{
    private readonly string _bucketName;
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<StorageService> _logger;

    public StorageService(
        ILogger<StorageService> logger,
        IOptions<AwsOptions> awsOptions, 
        IOptions<StorageServiceOptions> storageOptions
    )
    {
        var awsConfig = awsOptions.Value;
        
        var s3Client = new AmazonS3Client(
            new BasicAWSCredentials(awsConfig.AccessKeyId, awsConfig.SecretAccessKey),
            Amazon.RegionEndpoint.GetBySystemName(awsConfig.Region)
        );

        _logger = logger;
        _s3Client = s3Client;
        _bucketName = storageOptions.Value.BucketName;

        _logger.LogInformation("StorageService initialized with bucket {BucketName} in region {Region}", _bucketName, awsConfig.Region);
    }

    public async Task<string> Upload(string fileName, string contentType, Stream fileStream)
    {
        _logger.LogInformation("Starting upload of file {FileName} to bucket {BucketName} with content type {ContentType}", fileName, _bucketName, contentType);
            
        var uploadRequest = new PutObjectRequest
        {
            Key = fileName,
            BucketName = _bucketName,
            InputStream = fileStream,
            ContentType = contentType
        };

        try
        {
            await _s3Client.PutObjectAsync(uploadRequest);
            _logger.LogInformation("File {FileName} uploaded successfully to bucket {BucketName}", fileName, _bucketName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while uploading file {FileName} to bucket {BucketName}", fileName, _bucketName);
            throw; 
        }

        return fileName;
    }

    public async Task Delete(string filePath)
    {
        _logger.LogInformation("Starting deletion of file {FilePath} from bucket {BucketName}", filePath, _bucketName);
            
        var deleteRequest = new DeleteObjectRequest
        {
            Key = filePath,
            BucketName = _bucketName
        };

        try
        {
            await _s3Client.DeleteObjectAsync(deleteRequest);
            _logger.LogInformation("File {FilePath} deleted successfully from bucket {BucketName}", filePath, _bucketName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting file {FilePath} from bucket {BucketName}", filePath, _bucketName);
            throw;
        }
    }
}