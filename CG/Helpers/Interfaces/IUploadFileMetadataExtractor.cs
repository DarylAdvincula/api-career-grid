namespace CG.Helpers.Interfaces
{
    public interface IUploadFileMetadataExtractor
    {
        public string GetExtension();

        public string GetContentType();

        public long GetSizeInBytes();

        public double GetSizeInMegabytes();

        string GetFileName();

        string GetUniqueFileName();

        string GetFilePath(string uploadFolderPath);
    }
}