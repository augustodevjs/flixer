using Amazon.S3;
using Amazon.Runtime;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Flixer.Catalog.Application.Intefaces;
using Flixer.Catalog.Infra.Storage.Configuration;

namespace Flixer.Catalog.Infra.Storage.Service;

public class StorageService : IStorageService
{
    private readonly string _bucketName;
    private readonly IAmazonS3 _s3Client;

    public StorageService(
        IOptions<AwsOptions> awsOptions, 
        IOptions<StorageServiceOptions> storageOptions)
    {
        var awsConfig = awsOptions.Value;

        _s3Client = new AmazonS3Client(
            new BasicAWSCredentials(awsConfig.AccessKeyId, awsConfig.SecretAccessKey),
            Amazon.RegionEndpoint.GetBySystemName(awsConfig.Region)
        );

        _bucketName = storageOptions.Value.BucketName;
    }

    public async Task<string> Upload(string fileName, string contentType, Stream fileStream)
    {
        var uploadRequest = new PutObjectRequest
        {
            Key = fileName,
            BucketName = _bucketName,
            InputStream = fileStream,
            ContentType = contentType
        };

        await _s3Client.PutObjectAsync(uploadRequest);
        
        return fileName;
    }
    
    public async Task Delete(string filePath)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            Key = filePath,
            BucketName = _bucketName
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
    }
}