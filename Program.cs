using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace FileModified
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The purpose of this application is to " +
                                "\ndetect if a file has been modified or not." +
                                "\nType 'exit' to close. . .");
            while (true)
            {
                Console.WriteLine("Please enter a file path to check");
                string fileToCheck = Console.ReadLine();

                if (fileToCheck == "exit")
                {
                    break;
                }

                if (!string.IsNullOrEmpty(fileToCheck))
                {
                    // Remove quotation marks if present
                    fileToCheck = fileToCheck.Trim('"');
                }

                string filePath = "filemodinfo.txt";



                // Read the last recorded modification time 
                DateTime lastKnownWriteTime = ReadWriteTime(filePath);

                try
                {
                    DateTime currenWriteTime = File.GetLastWriteTime(fileToCheck);

                    if (currenWriteTime != lastKnownWriteTime)
                    {
                        if (lastKnownWriteTime == DateTime.MinValue)
                        {
                            Console.WriteLine("A check on this file is not saved, saving now. . .");
                            SaveWriteTime(currenWriteTime, filePath);
                        }
                        else
                        {
                            Console.WriteLine($"File has been modified: {currenWriteTime} vs {lastKnownWriteTime}");
                            Console.WriteLine("Resetting saved write time to now");
                            SaveWriteTime(currenWriteTime, filePath);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No modifications have been made");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Checking File: {ex.Message} Please ensure the file path is correct.\n");
                }

            }
        }

        private static DateTime ReadWriteTime(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    string timeStr = File.ReadAllText(path);
                    return DateTime.Parse(timeStr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read the write time: {ex.Message}");
            }
            return DateTime.MinValue;
        }

        private static void SaveWriteTime(DateTime time, string path)
        {
            try
            {
                // Creating a full path using the current directory
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);

                if(!File.Exists(fullPath))
                {
                    // If file does not exist, create it in the relative directory
                    using (var stream = File.Create(fullPath)) { }
                    Console.WriteLine("File created at: " + fullPath);
                }

                // Write data to save to file
                File.WriteAllText(fullPath, time.ToString("o"));
                Console.WriteLine($"Saving current DateTime: {time:o}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save the write time: {ex.Message}");
            }
        }
    }
}
