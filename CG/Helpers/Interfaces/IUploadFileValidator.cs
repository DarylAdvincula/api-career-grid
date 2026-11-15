namespace CG.Helpers.Interfaces
{
    public interface IUploadFileValidator<ModelType>
    {
        bool IsContentTypeValid();

        bool IsSizeValid();

        void EnsureValidOrThow();
    }
}