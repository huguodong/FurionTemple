
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FurionTemplate.Common.Oss
{
    public interface IMinioOss
    {
        /// <summary>
        /// 创建存储桶
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> MakeBucketAsync(string bucketName);
        /// <summary>
        /// 删除存储桶
        /// </summary>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns></returns>
        Task RemoveBucketAsync(string bucketName);

        /// <summary>
        /// 存储桶列表
        /// </summary>
        /// <returns></returns>
        Task<List<Bucket>> ListBucketsAsync();

        /// <summary>
        /// 判断存储桶是否存在
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<bool> BucketExistsAsync(string bucketName);
        /// <summary>
        /// 通过Stream上传对象
        /// </summary>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="objectName">存储桶里的对象名称</param>
        /// <param name="data">要上传的Stream对象</param>
        /// <param name="size">流的大小</param>
        /// <param name="contentType">文件的Content type，默认是"application/octet-stream"</param>
        /// <returns></returns>
        Task<bool> PutObjectAsync(string bucketName, string objectName, Stream data, long size, string contentType = "application/octet-stream");

        /// <summary>
        /// 返回对象数据的流
        /// </summary>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="objectName">存储桶里的对象名称</param>
        /// <param name="callback">处理流的回调函数</param>
        /// <returns></returns>
        Task<Stream> GetObjectAsync(string bucketName, string objectName);
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="objectName">存储桶里的对象名称</param>
        /// <returns></returns>
        Task RemoveObjectAsync(string bucketName, string objectName);

        /// <summary>
        /// 批量删除对象
        /// </summary>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="objectNames">存储桶里的对象名称列表</param>
        /// <returns></returns>
        Task RemoveObjectAsync(string bucketName, List<string> objectNames);
    }
}
