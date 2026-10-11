using CG.Exceptions;
using CG.Helpers.Interfaces;

namespace CG.Helpers.Implementations
{
    public class UploadFileValidator : IUploadFileValidator
    {
        private readonly IUploadFileMetadataExtractor _uploadFileMetadataExtractor;
        private readonly IFormFile _file;
        private readonly double _maxSizeMb;
        private readonly IList<string> _allowedContentTypes;

        public UploadFileValidator(
            IFormFile file,
            long? maxSizeMb = null,
            IList<string>? allowedContentTypes = null
        )
        {
            _uploadFileMetadataExtractor = new UploadFileMetadataExtractor(file);
            _file = file;
            _maxSizeMb = maxSizeMb ?? 5;
            _allowedContentTypes = allowedContentTypes ?? [
                "application/pdf",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "application/msword"
            ];
        }

        public bool IsContentTypeValid()
        {
            return _allowedContentTypes.Contains(_uploadFileMetadataExtractor.GetContentType());
        }

        public bool IsSizeValid()
        {
            return _uploadFileMetadataExtractor.GetSizeInMegabytes() <= _maxSizeMb;
        }

        public void EnsureValidOrThow()
        {
            if (_file is null || _file.Length == 0)
                throw new BadRequestException("No resume uploaded.");
            else if (!IsContentTypeValid())
                throw new BadRequestException($"The uploaded file's format is not supported.");
            else if (!IsSizeValid())
                throw new BadRequestException($"File size should not exceed {_uploadFileMetadataExtractor.GetSizeInBytes():F2} Mb.");
        }
    }
}