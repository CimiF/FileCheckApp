using Antlr.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Web;

namespace FileCheckApp.App_Start
{
    public class StateKeeperSingleJsonFile: IStateKeeper
    {
        private const string fileName = "stateHistory.json";
        private string stateFilePath = HttpContext.Current.Server.MapPath($"~/{fileName}");

        //Loads the last state of the one directory from textFile as json
        public  List<FileState> LoadLastState(string directoryPath)
        {
               var result = GetLastDirectoryContents().Where(x=>x.DirectoryPath == directoryPath).FirstOrDefault()?.FileStates;                

            return result;
        }
        //Loads all saved states
        private  List<DirectoryContent> GetLastDirectoryContents()
        {
            List<DirectoryContent> result = new List<DirectoryContent>();
            if (File.Exists(stateFilePath))
            {
                string json = File.ReadAllText(stateFilePath);
                result = JsonConvert.DeserializeObject<List<DirectoryContent>>(json);
            }
            return result;
        }
        //Updates the state of the files in the directory
        public  void SaveState(List<FileState> fileStates, string directoryPath)
        {            
            var contents = GetLastDirectoryContents();
            if(contents.Any(x => x.DirectoryPath == directoryPath))
            {
                contents.Where(x => x.DirectoryPath == directoryPath).FirstOrDefault().FileStates = fileStates;
            }
            else
            {
                contents.Add(new DirectoryContent
                {
                    DirectoryPath = directoryPath,
                    FileStates = fileStates
                });
            }
            string json = JsonConvert.SerializeObject(contents);

            bool createdNewMutex;
            using (Mutex mutex = new Mutex(false, "stateFilePath", out createdNewMutex))
            {
                if (createdNewMutex)
                {
                    try
                    {
                        File.WriteAllText(stateFilePath, json);
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
                else
                {
                    throw new IOException("The file is being used by another process. Try again later.");
                }
            }
        }
    }
}