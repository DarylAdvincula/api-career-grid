namespace CG.Helpers.Interfaces
{
    public interface IUploadFileValidator
    {
        bool IsContentTypeValid();

        bool IsSizeValid();

        void EnsureValidOrThow();
    }
}