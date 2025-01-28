using System.Collections.Generic;

namespace FileCheckApp.App_Start
{
    public interface IStateKeeper
    {
        List<FileState> LoadLastState(string directoryPath);
        void SaveState(List<FileState> fileStates, string directoryPath);
    }
}