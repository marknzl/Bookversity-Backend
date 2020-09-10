using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Bookversity.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bookversity.Api.Services
{
    public class ImageStoreService
    {
        private readonly StorageSettings _storageSettings;

        public ImageStoreService(IOptionsSnapshot<StorageSettings> storageSettings)
        {
            _storageSettings = storageSettings.Value;
        }

        private BlobServiceClient GetBlobServiceClient()
        {
            return new BlobServiceClient(_storageSettings.ConnectionString);
        }

        public async Task CreateUserContainer(string userId)
        {
            var blobServiceClient = GetBlobServiceClient();
            await blobServiceClient.CreateBlobContainerAsync(userId, PublicAccessType.Blob);
        }

        public async Task<ImageUploadResponse> UploadImage(string userId, IFormFile imageFile)
        {
            var blobServiceClient = GetBlobServiceClient();
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(userId);

            string[] fileNameSeparated = imageFile.FileName.Split('.');
            string uniqueIdentifier = Guid.NewGuid().ToString();
            string uniqueFileName = $"{uniqueIdentifier}.{fileNameSeparated[1]}";

            var imageUploadResponse = new ImageUploadResponse()
            {
                ImageFileName = uniqueFileName,
                ImageUrl = $"{_storageSettings.BaseUrl}{userId}/{uniqueFileName}"
            };

            var blobClient = blobContainerClient.GetBlobClient(uniqueFileName);
            var memoryStream = new MemoryStream();

            await imageFile.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            await blobClient.UploadAsync(memoryStream, true);

            memoryStream.Close();


            return imageUploadResponse;
        }

        public async Task DeleteImage(Item item)
        {
            var blobServiceClient = GetBlobServiceClient();
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(item.SellerId);
            var blobClient = blobContainerClient.GetBlobClient(item.ImageFileName);

            await blobClient.DeleteIfExistsAsync();
        }
    }
}
