using Aliyun.OSS;
using Aliyun.OSS.Common;
using FurionTemplate.Common.Helper;
using System;
using System.IO;
using System.Threading;

namespace FurionTemplate.Common.Oss
{
    public static class AliOss
    {

        static readonly string bucketName = Appsettings.App(new string[] { "AppSettings", "AliOssSetting", "BucketName" });
        static readonly string accessKeyId = Appsettings.App(new string[] { "AppSettings", "AliOssSetting", "AccessKeyId" });
        static readonly string accessKeySecret = Appsettings.App(new string[] { "AppSettings", "AliOssSetting", "AccessKeySecret" });
        static readonly string endpoint = Appsettings.App(new string[] { "AppSettings", "AliOssSetting", "Endpoint" });
        static readonly string domain = Appsettings.App(new string[] { "AppSettings", "AliOssSetting", "Domain" });
        static readonly AutoResetEvent _event = new AutoResetEvent(false);
        static readonly ClientConfiguration configuration = new ClientConfiguration()
        {
            ConnectionTimeout = 10000
        };
        static readonly OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret, configuration);


        public static string PutObjectFromFile(Stream fileToUpload, string fileDir, string fileExtension)
        {
            string path = fileDir + "/" + DateTime.Now.ToString("yyyy-MM-dd").Replace('-', '/') + "/";
            string key = path + Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower() + fileExtension;
            try
            {
                client.PutObject(bucketName, key, fileToUpload);
                return domain + key;

            }
            catch (OssException ex)
            {
                throw new Exception($"Failed with error code: {ex.ErrorCode}; Error info: {ex.Message}. \nRequestID:{ ex.RequestId}\tHostID:{ex.HostId}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        public static void AsyncPutObject(Stream fileToUpload, string fileDir, string fileExtension)
        {
            string path = fileDir + "/" + DateTime.Now.ToString("yyyy-MM-dd").Replace('-', '/') + "/";
            string key = path + Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower() + fileExtension;
            try
            {
                string result = "ok";
                // 1. put object to specified output stream
                client.BeginPutObject(bucketName, key, fileToUpload, PutObjectCallback, result.ToCharArray());
                _event.WaitOne();
            }
            catch (OssException ex)
            {
                throw new Exception($"Failed with error code: {ex.ErrorCode}; Error info: {ex.Message}. \nRequestID:{ ex.RequestId}\tHostID:{ex.HostId}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        private static void PutObjectCallback(IAsyncResult ar)
        {
            try
            {
                client.EndPutObject(ar);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                _event.Set();
            }
        }

    }
}
