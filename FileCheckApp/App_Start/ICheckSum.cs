namespace FileCheckApp.App_Start
{
    public interface ICheckSum
    {
        string GetCheckSum(string filePath);
    }
}