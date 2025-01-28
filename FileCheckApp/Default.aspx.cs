using FileCheckApp.App_Start;
using Microsoft.Ajax.Utilities;
using System;
using System.IO;
using System.Web.UI;

namespace FileCheckApp
{
    public partial class _Default : Page
    {

        private const string DefaultDirectoryPath = "~/App_Data";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DirectoryPathTextBox.Text = DefaultDirectoryPath;
            }
        }

        protected void CheckDirectoryButton_Click(object sender, EventArgs e)
        {
            
            string relativePath = DirectoryPathTextBox.Text.IfNullOrWhiteSpace(DefaultDirectoryPath);
            string directoryPath = Server.MapPath(relativePath);
            Uri uri = new Uri(directoryPath);

            if (Directory.Exists(directoryPath))
            {
                DirectoryContent changeList;
                try
                {
                    changeList = DirectoryContent.CheckDirectoryChanges(directoryPath);
                }
                catch (Exception ex)
                {
                    string error = "<p>" + Resources.DirectoryError + "</p>";
                    error += "<p>" + ex.Message + "</p>";
                    return;
                }

                string content = "</br>";  
                if(changeList.IsNewDirectory)
                {
                    content += Resources.NewDirectory + " ,";

                }
                if (changeList.FileStates.Count == 0)
                {
                    content += Resources.NoChanges + "</br>";
                }
                else
                {
                    content += Resources.ChangesDetected + "</br>";
                }

                content += "<ul>";
                foreach (FileState item in changeList.FileStates)
                {
                    content += $"<li>{getItemTypeString(item.ChangeType)} {Resources.Version}:{item.version} {uri.MakeRelativeUri(new Uri(item.Path))}</li>";
                }                
                content += "</ul>";
                content +="</br>" + Resources.ChangeType_Explain + "</br>";
                DirectoryContents.Text = content;
            }
            else
            {
                DirectoryContents.Text = Resources.DirNotFound ;
            }
        }
        private string getItemTypeString(ChangeType type)
        {
            switch (type)   
            {
                case ChangeType.NoChange:
                    return Resources.ChangeType_NoChange;
                    break;
                case ChangeType.NewFile:
                    return Resources.ChangeType_NewFile;
                    break;
                case ChangeType.Modified:
                    return Resources.ChangeType_Modified;
                    break;
                case ChangeType.Deleted:
                    return Resources.ChangeType_Deleted;
                    break;
                default:
                    return Resources.ChangeType_Undefined;
                    break;
            }
        }
    }
}