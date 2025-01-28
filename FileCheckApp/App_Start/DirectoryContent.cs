using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCheckApp.App_Start
{
    public class DirectoryContent
    {
        public string DirectoryPath { get; set; }
        public List<FileState> FileStates { get; set; }

        [JsonIgnore]
        public bool IsNewDirectory { get; set; } = false;

        private static readonly ICheckSum checkSum = Global.ServiceProvider.GetService <ICheckSum>();
        private static readonly IStateKeeper stateKeeper = Global.ServiceProvider.GetService<IStateKeeper>();

        public static DirectoryContent CheckDirectoryChanges(string directoryPath)
        {
            DirectoryContent result = new DirectoryContent();
            result.DirectoryPath = directoryPath;
            List<FileState> lastState = stateKeeper.LoadLastState(directoryPath);
            List<FileState> currentState = GetDirectoryContent(directoryPath);
            
            result.IsNewDirectory = lastState == null;

            result.FileStates = CompareStates(lastState, currentState);
            if (result.FileStates.Count > 0)
            {
                foreach (var rState in result.FileStates.Where(z => z.ChangeType != ChangeType.Deleted))
                {
                    currentState.Where(y => y.Path == rState.Path).FirstOrDefault().version = rState.version;
                }

                stateKeeper.SaveState(currentState, directoryPath);
            }

            return result;
        }
        private static List<FileState> CompareStates(List<FileState> lastState, List<FileState> currentState)
        {
            List<FileState> result = new List<FileState>();
            foreach (var file in currentState)
            {
                var lastFile = lastState?.Where(x => x.Path == file.Path).FirstOrDefault();
                if (lastFile == null)
                {
                    result.Add(new FileState
                    {
                        Path = file.Path,
                        Name = file.Name,
                        CheckSum = file.CheckSum,
                        ChangeType = ChangeType.NewFile,
                        version = 1
                    });
                }
                else if (lastFile.CheckSum != file.CheckSum)
                {
                    result.Add(new FileState
                    {
                        Path = file.Path,
                        Name = file.Name,
                        CheckSum = file.CheckSum,
                        ChangeType = ChangeType.Modified,
                        version = ++lastFile.version
                    });
                }
            }
            if (lastState != null)
            {
                foreach (var file in lastState)
                {
                    if (!currentState.Any(x => x.Path == file.Path))
                    {
                        result.Add(new FileState
                        {
                            Path = file.Path,
                            Name = file.Name,
                            CheckSum = file.CheckSum,
                            ChangeType = ChangeType.Deleted,
                            version = ++file.version
                        });
                    }
                }
            }
            return result;
        }
        private static List<FileState> GetDirectoryContent(string directoryPath)
        {
            List<FileState> fileStates = new List<FileState>();
            string[] files = System.IO.Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                fileStates.Add(new FileState
                {
                    Path = file,
                    Name = System.IO.Path.GetFileName(file),
                    CheckSum = checkSum.GetCheckSum(file)
                });
            }
            System.IO.Directory.GetDirectories(directoryPath).ToList().ForEach(dir =>
            {
                fileStates.AddRange(GetDirectoryContent(dir));
            });
            return fileStates;
        }
    }
}