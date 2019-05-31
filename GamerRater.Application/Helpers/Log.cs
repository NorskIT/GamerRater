using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace GamerRater.Application.Helpers
{
    public static class Log
    {
        public static StorageFile ErrorFile { get; set; }

        private static async Task CreateErrorFile()
        {
            try
            {
                //Saves file in AppData/Local/Packages/'Packagenumber'/LocalState/ErrorFile.txt
                var local = ApplicationData.Current.LocalFolder;
                ErrorFile = await local.CreateFileAsync("ErrorFile.txt", CreationCollisionOption.OpenIfExists);
            }
            catch (Exception)
            {
                //TODO: Send to OS crash center?
                //Possible file permission error, try to continue.
            }
        }

        public static async Task WriteMessage(string strMessage)
        {
            try
            {
                if (ErrorFile == null) await CreateErrorFile().ConfigureAwait(true);

                await FileIO.AppendTextAsync(ErrorFile,
                    string.Format("{0} - {1}\r\n", DateTime.Now.ToLocalTime().ToString(), strMessage));
            }
            catch (Exception)
            {
                //TODO: Send to OS crash center?
                //Possible file permission error, try to continue.
            }
        }
    }
}
