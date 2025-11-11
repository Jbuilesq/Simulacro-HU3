namespace productosApi.Application.Interfaces;


    public interface IPasswordHasher
    {
        string Hash(string password);
        bool verify(string hash, string password);
    }
