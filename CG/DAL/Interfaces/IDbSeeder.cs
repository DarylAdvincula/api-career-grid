namespace CG.DAL.Interfaces
{
    public interface IDbSeeder
    {
        Task SeedAsync();
        Task SeedAdmin();
    }
}