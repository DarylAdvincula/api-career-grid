namespace CG.Helpers.Interfaces
{
    public class UploadFileMetadataExtractor : IUploadFileMetadataExtractor
    {
        private readonly IFormFile _file;
        private static string? uniqueFileName = null;

        public UploadFileMetadataExtractor(IFormFile file)
        {
            _file = file;
        }
        
        public string GetExtension()
        {
            return Path.GetExtension(_file.FileName);
        }

        public string GetContentType()
        {
            return _file.ContentType.ToLowerInvariant();
        }

        public long GetSizeInBytes()
        {
            return _file.Length;
        }

        public double GetSizeInMegabytes()
        {
            return GetSizeInBytes() / 1024.0 / 1024.0;
        }

        public string GetFileName()
        {
            return _file.FileName;
        }

        public string GetUniqueFileName()
        {
            if (uniqueFileName is null)
                uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(GetFileName())}";
            
            return uniqueFileName;
        }

        public string GetFilePath(string uploadFolderPath)
        {
            return Path.Combine(uploadFolderPath, GetUniqueFileName());
        }
    }
}