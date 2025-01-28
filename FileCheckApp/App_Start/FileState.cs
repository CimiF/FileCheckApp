namespace FileCheckApp.App_Start
{
    public class FileState
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string CheckSum { get; set; }
        public int version { get; set; }
        public ChangeType ChangeType { get; set; }
    }
}