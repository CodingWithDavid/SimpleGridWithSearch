/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace InternalLib
{
    public class FilePoller
    {
        private string inputfolder = "";
        private List<FileInfo> pass1Files = new List<FileInfo>();
        private List<FileInfo> pass2Files = new List<FileInfo>();

        /// <summary>
        /// This method will read in a list of files from a directory, ensure
        /// they are not still be written to, and return that list
        /// </summary>
        /// <param name="folder">Folder to reading files from</param>
        /// <returns>List of file names</returns>
        public List<string> GetFiles(string folder)
        {
            inputfolder = folder;
            List<string> result = new List<string>();

            //get what is there now
            Pass1();

            //wait a second
            Thread.Sleep(1000);

            //get the list again, in case there are any still being written out
            Pass2();

            result = FilesToProcess();
            pass1Files.Clear();
            pass2Files.Clear();

            return result;
        }

        /// <summary>
        /// this method is responsible for comparing 2 list of files, ensure each
        /// file is not locked and that the size has not changed between polling's
        /// </summary>
        /// <returns>List of file names that are OK to process</returns>
        private List<string> FilesToProcess()
        {
            List<string> result = new List<string>();
            //get the files that the size didn't change between passes
            foreach (FileInfo fi in pass1Files)
            {
                if (!IsFileLocked(fi))
                {
                    if (fi.Length == GetPass2FileSize(fi.Name))
                    {
                        result.Add(fi.FullName);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Checks to see if the file asked for is locked by another process
        /// </summary>
        /// <param name="file">file to test</param>
        /// <returns>true if the file is locked</returns>
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        /// <summary>
        /// Compares the file size of a file from the first polling cycle to the current one
        /// </summary>
        /// <param name="name">folder to look in</param>
        /// <returns>the length of the matching file</returns>
        private long GetPass2FileSize(string name)
        {
            long result = 0;
            foreach (FileInfo fi in pass2Files)
            {
                if (fi.Name.Match(name))
                {
                    if (fi.Exists)
                    {
                        result = fi.Length;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Reads in a list of files from a folder
        /// </summary>
        /// <returns>the list of file names from the folder passed in</returns>
        private List<string> Pass2()
        {
            //pass 2
            List<string> files = FileFolderHelper.GetFilesInfolder(inputfolder);
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                pass2Files.Add(fi);
            }
            return files;
        }

        /// <summary>
        /// Reads in a list of files from a folder
        /// </summary>
        /// <returns>the list of file names from the folder passed in</returns>
        private List<string> Pass1()
        {
            //read in the input folder
            List<string> files = FileFolderHelper.GetFilesInfolder(inputfolder);
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                pass1Files.Add(fi);
            }
            return files;
        }
    }
}