using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace JustEatIt.Services
{
    public class S3ImageService
    {
        public static async Task UploadFileToS3(int name, string path)
        {
            try
            {
                using var client = new AmazonS3Client("AKIAU7T2N3UNUTP5VCXP",
                            "gwzkTj7y+6qI3vUInqzxKJfecbI9IIDyiXeegK6J", RegionEndpoint.CACentral1);
                var fileTransferUtility = new TransferUtility(client);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    FilePath = path,
                    BucketName = "justeatit",
                    Key = name.ToString(),
                    CannedACL = S3CannedACL.PublicRead
                };

                //await fileTransferUtility.UploadAsync(path, "justeatit", name.ToString());
                await fileTransferUtility.UploadAsync(uploadRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Amazon S3 Error {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Some other ex {e.Message}");
            }
        }

        public static async Task RemoveFileFromS3(int name)
        {
            try
            {
                using var client = new AmazonS3Client("AKIAU7T2N3UNUTP5VCXP",
                    "gwzkTj7y+6qI3vUInqzxKJfecbI9IIDyiXeegK6J", RegionEndpoint.CACentral1);
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = "justeatit",
                    Key = name.ToString()
                };

                await client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Amazon S3 Error {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Some other ex {e.Message}");
            }
        }
    }
}