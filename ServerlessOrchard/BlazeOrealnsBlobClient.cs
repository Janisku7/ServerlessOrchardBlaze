using Azure.Core;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using System;
using System.Reflection.Metadata;

namespace ServerlessOrchard
{
    public class BlazeOrealnsBlobClient
    {
        private readonly BlazeBlobSettings BlazeBlobOptions;
        private BlobServiceClient _blobServiceClient;
        private BlobClientOptions blobClientOpions = new BlobClientOptions()
        {
            Retry = { Delay = TimeSpan.FromSeconds(2), MaxRetries = 10, Mode = RetryMode.Fixed },
            Diagnostics = { IsLoggingContentEnabled = true, ApplicationId = "ServerlessOrchard" }
        };
        public BlazeOrealnsBlobClient(BlobServiceClient blobServiceClient, IOptions<BlazeBlobSettings> options)
        {
            BlazeBlobOptions = options.Value;
            this._blobServiceClient = blobServiceClient;
            
        }
       
    }
    public class BlazeBlobSettings
    {
        public Uri ConnectionString { get; set; }
        public StorageSharedKeyCredential SharedKeyCredential { get; set; }
        public string ContainerName { get; set; }
        public string AccountName { get; set; }
    }
    
}
