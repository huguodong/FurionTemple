
using Minio;
using Minio.DataModel;
using Minio.Exceptions;
using FurionTemplate.Common.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FurionTemplate.Common.Oss
{
    public class MinioOss : IMinioOss
    {
        public volatile MinioClient minioClient;
        private readonly object minioConnectionLock = new object();
        private readonly string endpoint;
        private readonly string accessKey;
        private readonly string secretKey;

        public MinioOss()
        {
            accessKey = Appsettings.App(new string[] { "AppSettings", "MinioOssSetting", "AccessKeyId" });
            secretKey = Appsettings.App(new string[] { "AppSettings", "MinioOssSetting", "AccessKeySecret" });
            endpoint = Appsettings.App(new string[] { "AppSettings", "MinioOssSetting", "Endpoint" });
            this.minioClient = GetMinioClient();
        }

        public MinioOss(string endpoint, string accessKey, string secretKey)
        {
            this.endpoint = endpoint;
            this.accessKey = accessKey;
            this.secretKey = secretKey;
            this.minioClient = GetMinioClient();
        }
        public MinioClient GetMinioClient()
        {
            //如果已经连接实例，直接返回
            if (this.minioClient != null)
            {
                return this.minioClient;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            lock (minioConnectionLock)
            {
                if (this.minioClient != null)
                {
                    //释放minioClient连接
                    this.minioClient = null;
                }
                try
                {
                    if (string.IsNullOrEmpty(accessKey) && string.IsNullOrEmpty(secretKey))
                    {
                        this.minioClient = new MinioClient(endpoint);
                    }
                    else
                    {
                        this.minioClient = new MinioClient(endpoint, accessKey, secretKey);
                    }

                    this.minioClient.WithTimeout(5000);

                }
                catch (Exception ex)
                {

                    throw new Exception("minio服务启用失败，请检查是否开启", ex);
                }
            }
            return this.minioClient;
        }


        public async Task<Tuple<bool, string>> MakeBucketAsync(string bucketName)
        {
            try
            {
                // Create bucket if it doesn't exist.
                bool found = await minioClient.BucketExistsAsync(bucketName);
                if (found)
                {
                    return new Tuple<bool, string>(false, "bucket already exist!");

                }
                else
                {
                    await minioClient.MakeBucketAsync(bucketName);
                    return new Tuple<bool, string>(true, "carate bucket successfully!");
                }
            }
            catch (MinioException e)
            {
                throw new Exception("Error occurred", e);
            }
        }

        public async Task RemoveBucketAsync(string bucketName)
        {
            try
            {
                // Check if my-bucket exists before removing it.
                bool found = await minioClient.BucketExistsAsync(bucketName);
                if (found)
                {
                    // Remove bucket my-bucketname. This operation will succeed only if the bucket is empty.
                    await minioClient.RemoveBucketAsync(bucketName);
                }
                else
                {
                    //Console.Out.WriteLine("mybucket does not exist");
                }
            }
            catch (MinioException e)
            {
                throw new Exception("删除存储桶失败!", e);
            }
        }
        public async Task<List<Bucket>> ListBucketsAsync()
        {
            try
            {
                // List buckets that have read access.
                var list = await minioClient.ListBucketsAsync();
                return list.Buckets;
            }
            catch (MinioException e)
            {
                throw new Exception("Error occurred", e);
            }
        }

        public async Task<bool> BucketExistsAsync(string bucketName)
        {
            try
            {
                // Check whether 'my-bucketname' exists or not.
                bool found = await minioClient.BucketExistsAsync(bucketName);
                return found;
            }
            catch (MinioException e)
            {
                throw new Exception($"BucketExistsAsync  Exception:", e);
            }
        }

        public async Task<bool> PutObjectAsync(string bucketName, string objectName, Stream data, long size, string contentType = "application/octet-stream")
        {
            try
            {
                await minioClient.PutObjectAsync(bucketName, objectName, data, size, contentType);
                return true;
            }
            catch (MinioException e)
            {
                throw new Exception($"上传文件失败!", e);
            }
            catch (Exception e)
            {
                throw new Exception($"上传文件失败!", e);
            }
        }




        public async Task<Stream> GetObjectAsync(string bucketName, string objectName)
        {
            try
            {
                MemoryStream stream = new MemoryStream();


                // Check whether the object exists using statObject().
                // If the object is not found, statObject() throws an exception,
                // else it means that the object exists.
                // Execution is successful.
                await minioClient.StatObjectAsync(bucketName, objectName);

                // Get input stream to have content of 'my-objectname' from 'my-bucketname'
                await minioClient.GetObjectAsync(bucketName, objectName,
                                                 (st) =>
                                                 {
                                                     st.CopyTo(stream);
                                                 });

                return stream;
            }
            catch (MinioException e)
            {
                throw new Exception($"获取文件失败:", e);
            }
        }

        public async Task RemoveObjectAsync(string bucketName, string objectName)
        {
            try
            {
                // Remove objectname from the bucket my-bucketname.
                await minioClient.RemoveObjectAsync(bucketName, objectName);
            }
            catch (MinioException e)
            {
                throw new Exception($"删除文件失败:", e);
            }
        }

        public async Task RemoveObjectAsync(string bucketName, List<string> objectNames)
        {
            try
            {
                // Remove objectname from the bucket my-bucketname.
                IObservable<DeleteError> observable = await minioClient.RemoveObjectAsync(bucketName, objectNames);
                IDisposable subscription = observable.Subscribe(
                    deleteError => { },
                    ex => throw new Exception($"删除文件失败:", ex),
                    () =>
                    {

                    });
            }
            catch (MinioException e)
            {
                throw new Exception($"删除文件失败:", e);
            }
        }

    }
}
