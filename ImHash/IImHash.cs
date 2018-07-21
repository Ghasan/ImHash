namespace ImHash
{
    public interface IImHash
    {
        bool[] GetImageHash(string path);
        bool AreSimilar(bool[] firstHash, bool[] secondHash);
    }
}
