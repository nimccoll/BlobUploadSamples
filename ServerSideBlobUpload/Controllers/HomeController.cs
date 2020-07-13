//===============================================================================
// Microsoft FastTrack for Azure
// Azure Blob Upload Sample
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
using Azure.Storage.Blobs;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ServerSideBlobUpload.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpGet]
        public ActionResult UploadBlob()
        {
            return View();
        }

        [HttpPost]
        [ActionName("UploadBlob")]
        public async Task<ActionResult> UploadBlobPost(HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile.ContentLength > 0)
            {
                string connectionString = ConfigurationManager.AppSettings["AZURE_STORAGE_CONNECTION_STRING"];

                // Create a BlobServiceClient object which will be used to create a container client
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                //Create a unique name for the container
                string containerName = "{your blob container name here}";

                // Create the container and return a container client object
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Get the file name
                string fileName = Path.GetFileName(uploadedFile.FileName);

                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(fileName);

                // Open the file and upload its data
                await blobClient.UploadAsync(uploadedFile.InputStream, true);

                ViewBag.Message = $"File {fileName} uploaded successfully";
            }

            return View();
        }
    }
}
