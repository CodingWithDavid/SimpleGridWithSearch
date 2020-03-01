/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

//3rd party
using NLog;

namespace InternalLib
{
    public class ZipArchiver
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private ZipArchive zipArchive;

        /// <summary>
        /// Adds the files to an existing zip file
        /// </summary>
        /// <param name="zipFilename">File path to the zip file that the files are being added to.</param>
        /// <param name="sourceCollection">A list of file paths to the files being added to the zip.</param>
        public static void AddFilesToZip(string zipFilename, List<string> sourceCollection)
        {
            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(zipFilename, ZipArchiveMode.Update))
                {
                    foreach (string fileName in sourceCollection)
                    {
                        if (File.Exists(fileName))
                        {
                            zipArchive.CreateEntryFromFile(fileName, Path.GetFileName(fileName));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error("Failed to add files to zip.", ex, null);
                throw new Exception("Failed to add files to zip.");
            }
        }

        /// <summary>
        /// Adds the file to an existing zip file
        /// </summary>
        /// <param name="zipFilePath">File path to the zip file that the files are being added to.</param>
        /// <param name="sourceFilePath">The file path to the file being added to the zip.</param>
        /// <param name="targetEntryName">The name that the file will be seen as inside the zip.</param>
        public static void AddFileToZip(string zipFilePath, string sourceFilePath, string targetEntryName)
        {
            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Update))
                {
                    if (File.Exists(sourceFilePath))
                    {
                        zipArchive.CreateEntryFromFile(sourceFilePath, Path.GetFileName(targetEntryName));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to add file to zip.", ex, null);
                throw new Exception("Failed to add files to zip.");
            }
        }

        /// <summary>
        /// Add the given data string into the existing zip file under the given entry name.
        /// </summary>
        /// <param name="zipFilePath">File path to the zip file that is being updated.</param>
        /// <param name="entryName">Name of the entry that is created inside the zip.</param>
        /// <param name="dataToZip">Data to be written to the entry given.</param>
        public static void AddStringToZip(string zipFilePath, string entryName, string dataToZip)
        {
            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = zipArchive.CreateEntry(entryName);
                    using (StreamWriter streamWriter = new StreamWriter(entry.Open()))
                    {
                        streamWriter.Write(dataToZip);
                        streamWriter.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to add string to zip.", ex, null);
                throw new Exception("Failed to add files to zip.");
            }
        }


        /// <summary>
        /// Create a new zip file and add the given file to it.
        /// </summary>
        /// <param name="zipFilename">File path to the zip file that the files are being added to.</param>
        /// <param name="sourceFilename">The file path to the file being added to the zip.</param>
        /// <param name="targetFilename">The name that the file will be seen as inside the zip.</param>
        public static void CreateZipFromFile(string zipFilename, string sourceFilename, string targetFilename)
        {
            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(zipFilename, ZipArchiveMode.Create))
                {
                    if (File.Exists(sourceFilename))
                    {
                        zipArchive.CreateEntryFromFile(sourceFilename, Path.GetFileName(targetFilename));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to create zip from file.", ex, null);
                throw new Exception("Failed to create zip from file.");
            }
        }

        /// <summary>
        /// Create a new zip file and add the given files to it.
        /// </summary>
        /// <param name="zipFilename">File path to the zip file that the files are being added to.</param>
        /// <param name="sourceCollection">A list of file paths to the files being added to the zip.</param>
        public static void CreateZipFromFiles(string zipFilename, List<string> sourceCollection)
        {
            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(zipFilename, ZipArchiveMode.Create))
                {
                    foreach (string fileName in sourceCollection)
                    {
                        if (File.Exists(fileName))
                        {
                            zipArchive.CreateEntryFromFile(fileName, Path.GetFileName(fileName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to create zip from files.", ex, null);
                throw new Exception("Failed to create zip from file.");
            }
        }

        /// <summary>
        /// Create a new zip file and add the given files to it.
        /// </summary>
        /// <param name="zipFilename">File path to the zip file that the files are being added to.</param>
        /// <param name="sourceCollection">A dictionary of entry names and file paths to the files being added to the zip.</param>
        public static void CreateZipFromDictionary(string zipFilename, Dictionary<string, string> sourceCollection)
        {
            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(zipFilename, ZipArchiveMode.Create))
                {
                    foreach (KeyValuePair<string, string> kvp in sourceCollection)
                    {
                        zipArchive.CreateEntryFromFile(kvp.Key, kvp.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to create zip from dictionary.", ex, null);
                throw new Exception("Failed to create zip from dictionary.");
            }
        }

        public static void CreateZipFromDirectory(string zipFilename, string directoryToZip)
        {
            try
            {
                ZipFile.CreateFromDirectory(directoryToZip, zipFilename);
            }
            catch (Exception ex)
            {
                logger.Error("Failed to create zip from dictionary.", ex, null);
                throw new Exception("Failed to create zip from dictionary.");
            }
        }

        /// <summary>
        /// Create a new zip file and save the given data string into the given entry name.
        /// </summary>
        /// <param name="zipFilename">File path to the zip file that is being created.</param>
        /// <param name="entryName">Name of the entry that is created inside the zip.</param>
        /// <param name="dataToZip">Data to be written to the entry given.</param>
        public static void CreateZipFromString(string zipFilename, string entryName, string dataToZip)
        {
            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(zipFilename, ZipArchiveMode.Create))
                {
                    ZipArchiveEntry entry = zipArchive.CreateEntry(entryName);
                    using (StreamWriter streamWriter = new StreamWriter(entry.Open()))
                    {
                        streamWriter.Write(dataToZip);
                        streamWriter.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to create zip from string.", ex, null);
                throw new Exception("Failed to create zip from dictionary.");
            }
        }

        /// <summary>
        /// Extract data from the given zip file name to the given extraction directory.
        /// </summary>
        /// <param name="zipFilename">File path to the zip file that is being used in the extraction.</param>
        /// <param name="extractionDirectory">Path to the directory that the data will be extracted to.</param>
        public static bool ExtractZipToDirectory(string zipFilename, string extractionDirectory)
        {
            bool result = false;
            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(zipFilename, ZipArchiveMode.Read))
                {
                    zipArchive.ExtractToDirectory(extractionDirectory);
                }

                result = true;
            }
            catch (Exception ex)
            {
                logger.Error("Failed to extract zip to directory.", ex, null);
                throw new Exception("Failed to extract zip to directory." + ex);
            }

            return result;
        }

        public static void DecompressAndWriteToFile(string zipFilename, string extractionDirectory, string newName)
        {
            const int bufferSize = 4096;
            var compressedfileInfo = new FileInfo(zipFilename);

            using (var compressedFileStream = compressedfileInfo.OpenRead())
            {
                var newDecompressedPath = Path.Combine(extractionDirectory, newName);
                using (var decompressedFileSream = File.Create(newDecompressedPath))
                {
                    using (var gZipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        var buffer = new byte[bufferSize];
                        int numRead;
                        while ((numRead = gZipStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            decompressedFileSream.Write(buffer, 0, numRead);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Gets the names of the files held inside of the provided zip.
        /// </summary>
        /// <param name="zipFileName">The file path to the zip file to look into.</param>
        /// <returns>An IEnumerable representing the collection of items held inside the zip.</returns>
        public static IEnumerable<string> GetFileNames(string zipFileName)
        {
            using (ZipArchive zipArchive = ZipFile.Open(zipFileName, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    yield return entry.Name;
                }
            }
        }

        /// <summary>
        /// Gets the names of the files held inside of the provided zip.
        /// </summary>
        /// <param name="zipStream">A stream representing the zip to look into.</param>
        /// <returns>An IEnumerable representing the collection of items held inside the zip.</returns>
        public static IEnumerable<string> GetFileNames(Stream zipStream)
        {
            using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    yield return entry.Name;
                }
            }
        }

        /// <summary>
        /// Close the currently opened zip file.
        /// </summary>
        public void CloseZipFile()
        {
            zipArchive.Dispose();
            zipArchive = null;
        }

        /// <summary>
        /// Get the file stream for the given internal file name.
        /// </summary>
        /// <param name="filename">The name of the file inside the zip file to fetch the stream to.</param>
        /// <returns>A stream for the internal file inside the zip.</returns>
        public Stream GetFileStream(string filename)
        {
            Stream result = null;

            ZipArchiveEntry entry = zipArchive.GetEntry(filename);

            result = entry.Open();

            return result;
        }

        /// <summary>
        /// Open the zip file.
        /// </summary>
        /// <param name="zipFileName">The file path to the zip file to open.</param>
        /// <returns>A boolean to indicate if the file was opened.</returns>
        public bool OpenZipFile(string zipFileName)
        {
            bool result = false;
            zipArchive = ZipFile.Open(zipFileName, ZipArchiveMode.Update);
            if (zipArchive.Entries.Any())
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Open the zip file.
        /// </summary>
        /// <param name="zipFileStream">A stream representing the zip file to open.</param>
        /// <returns>A boolean to indicate if the file was opened.</returns>
        public bool OpenZipFile(Stream zipFileStream)
        {
            bool result = false;
            zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Update);
            if (zipArchive.Entries.Any())
            {
                result = true;
            }
            return result;
        }
    }
}