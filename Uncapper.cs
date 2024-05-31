using Newtonsoft.Json.Linq;
using System;
using System.Data.SQLite;
using System.IO;

namespace WuWa_Uncapper
{
    internal class Program
    {
        static void Main(string[] args) 
        {
            
			Console.WriteLine("Wuthering Waves 120 FPS Change");
			
            Console.WriteLine("\nNotes:");
			
            Console.WriteLine("\nThis program just changes, a database file that is located inside the game folder.");
            Console.WriteLine("It doesn't in anyway modify any existing game memory handles during gameplay which makes it safe.");
            Console.WriteLine("Do take note that 60 fps above in the game would lead to unexpected behaviour in game.");
            Console.WriteLine("FPS above 60 is considered experiemental and not yet officially added by the devs.");
            Console.WriteLine("\nPress any key to continue...");
			Console.ReadKey();
			
            Console.WriteLine("\nBefore the program starts, the program would look for values located in the database first.");
            Console.WriteLine("Do note that if the settings fails to pass the prerequisites, you would need to do the following as the program instructs.");
			Console.WriteLine("Simply find your game directory file and head towards its LocalStorage.db, as shown below.");
			Console.WriteLine("e.g. E:\\Games\\Wuthering Waves\\Wuthering Waves Game\\Client\\Saved\\LocalStorage\\LocalStorage.db");
			Console.WriteLine("\nPress any key to continue...");
			Console.ReadKey();         
            
            string dbPath;
            do
            {
                Console.Write("\nEnter the file path: ");
                dbPath = Console.ReadLine();
                if (string.IsNullOrEmpty(dbPath))
                {
                    Console.WriteLine("Please enter a proper file path directory.");
                }
            } while (string.IsNullOrEmpty(dbPath));
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try 
                {
                    connection.Open();
                    Console.WriteLine("Connected to the Game Settings database.");

                    string selectQuery = "SELECT value FROM LocalStorage WHERE key = 'GameQualitySetting';";
                    string gameQualitySettingJson = null;

                    using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection))
                    {
                        using (SQLiteDataReader dbReader = selectCommand.ExecuteReader())
                        {
                            if (dbReader.Read())
                            {
                                gameQualitySettingJson = dbReader["value"].ToString();
                                Console.WriteLine("\nOriginal GameQualitySetting JSON found");
                                
                            }
                        }
                    }

                    if (gameQualitySettingJson == null)
                    {
                        Console.WriteLine("No GameQualitySetting found.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        return;
                    }

                    var gameQualitySetting = JObject.Parse(gameQualitySettingJson);
                    
                    int frameRate = gameQualitySetting["KeyCustomFrameRate"].Value<int>();
                    if (frameRate < 60)
                    {
                        Console.WriteLine("FPS Limit is not set to 60. Simply run the game and change the setting to 60, also disable Vsync if it's turned off.");
                        Console.WriteLine("Press any key to exit the program...");
                        Console.ReadKey();
                        return;
                    }

                    if (gameQualitySetting["KeyPcVsync"].Value<int>() != 0)
                    {
                        Console.WriteLine("It appears Vsync is turned on in game. Simply run the game and disable it, also set FPS Limit to 60");
                        Console.WriteLine("Press any key to exit the program...");
                        Console.ReadKey();
                        return;
                    }
                
                    gameQualitySetting["KeyCustomFrameRate"] = 120;
                    string updatedGameQualitySettingJson = gameQualitySetting.ToString();

                    Console.WriteLine("\nUpdated GameQualitySetting JSON:");
                    Console.WriteLine(updatedGameQualitySettingJson);

                    string updateQuery = "UPDATE LocalStorage SET value = @value WHERE key = 'GameQualitySetting';";

                    using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@value", updatedGameQualitySettingJson);
                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        Console.WriteLine($"\n{rowsAffected} row(s) updated. \n\nGame should be patched, run the game. If you changed any of the settings you'll have to do this again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occured: " + ex.Message);
                    Console.WriteLine("Program ran into an unexpected problem. Report this to the dev, press any key to exit the program");
                    Console.WriteLine("If the program is returning with an error about 'Disk I/O'");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine("\nPress any key to exit the program.");
            Console.ReadKey();
        }
    }
}