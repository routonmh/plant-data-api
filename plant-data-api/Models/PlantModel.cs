using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PlantDataAPI.Models.Entities;
using Attribute = PlantDataAPI.Models.Entities.Attribute;
using DbConnection = PlantDataAPI.Utilities.DBs.DbConnection;

namespace PlantDataAPI.Models
{
    public static class PlantModel
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Plant> GetPlantByID(int id)
        {
            Plant plant = null;

            using (DbConnection db = new DbConnection())
            {
                MySqlCommand cmd = await db.GetCommandAsync();
                cmd.CommandText = "SELECT PlantID, CommonName, ScientificName, PlantDescription, IsEdible " +
                                  "FROM plant WHERE PlantID = @PlantID";
                cmd.Parameters.AddWithValue("@PlantID", id);

                DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    plant = new Plant()
                    {
                        PlantID = id,
                        CommonName = reader["CommonName"] as string ?? null,
                        ScientificName = reader["ScientificName"] as string ?? null,
                        PlantDescription = reader["PlantDescription"] as string ?? null,
                        IsEdible =  Convert.ToBoolean(reader["IsEdible"])
                    };
                }
            }

            return plant;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="plantId"></param>
        /// <returns></returns>
        public static async Task<List<Attribute>> GetAttributesForPlant(int plantId)
        {
            List<Attribute> attributes = new List<Attribute>();

            using (DbConnection db = new DbConnection())
            {
                MySqlCommand cmd = await db.GetCommandAsync();
                cmd.CommandText = "SELECT AttributeID, PlantDescription FROM attribute " +
                                  "WHERE PlantID = @PlantID";

                cmd.Parameters.AddWithValue("@PlantID", plantId);

                DbDataReader reader = await cmd.ExecuteReaderAsync();
                while(await reader.ReadAsync())
                    attributes.Add(new Attribute()
                    {
                        PlantID = plantId,
                        AttributeID = reader["AttributeID"] as int? ?? 0,
                        AttributeDescription = reader["AttributeDescription"] as string ?? string.Empty
                    });
            }

            return attributes;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="plantId"></param>
        /// <returns></returns>
        public static async Task<List<RegionShapeFile>> GetRegionShapeFilesForPlant(int plantId)
        {
            List<RegionShapeFile> regionShapeFiles = new List<RegionShapeFile>();

            using (DbConnection db = new DbConnection())
            {
                MySqlCommand cmd = await db.GetCommandAsync();
                cmd.CommandText = "SELECT RegionShapeFileID, PlantID, LinkToFile FROM region_shape_file " +
                                  "WHERE PlantID = @PlantID;";

                cmd.Parameters.AddWithValue("@PlantID", plantId);

                DbDataReader reader = await cmd.ExecuteReaderAsync();
                while(await reader.ReadAsync())
                    regionShapeFiles.Add(new RegionShapeFile()
                    {
                        PlantID = plantId,
                        RegionShapeFileID = reader["RegionShapeFileID"] as int? ?? 0,
                        LinkToFile = reader["LinkToFile"] as string ?? string.Empty
                    });
            }

            return regionShapeFiles;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="plantId"></param>
        /// <returns></returns>
        public static async Task<List<Image>> GetImagesForPlant(int plantId)
        {
            List<Image> attributes = new List<Image>();

            using (DbConnection db = new DbConnection())
            {
                MySqlCommand cmd = await db.GetCommandAsync();
                cmd.CommandText = "SELECT ImageID, LinkToFile FROM image " +
                                  "WHERE PlantID = @PlantID";

                cmd.Parameters.AddWithValue("@PlantID", plantId);

                DbDataReader reader = await cmd.ExecuteReaderAsync();
                while(await reader.ReadAsync())
                    attributes.Add(new Image()
                    {
                        PlantID = plantId,
                        ImageID = reader["ImageID"] as int? ?? 0,
                        LinkToFile = reader["LinkToFile"] as string ?? string.Empty
                    });
            }

            return attributes;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="plant"></param>
        /// <returns></returns>
        public static async Task<bool> AddPlant(Plant plant)
        {
            bool success = false;

            try
            {
                using (DbConnection db = new DbConnection())
                {
                    MySqlCommand cmd = await db.GetCommandAsync();
                    cmd.CommandText = "INSERT INTO plant (PlantID, CommonName, ScientificName, PlantDescription, IsEdible)" +
                                      "VALUES (@PlantID, @CommonName, @ScientificName, @PlantDescription, @IsEdible)";

                    cmd.Parameters.AddWithValue("@PlantID", plant.PlantID);
                    cmd.Parameters.AddWithValue("@CommonName", plant.CommonName);
                    cmd.Parameters.AddWithValue("@ScientificName", plant.ScientificName);
                    cmd.Parameters.AddWithValue("@PlantDescription", plant.PlantDescription);
                    cmd.Parameters.AddWithValue("@IsEdible", plant.IsEdible);

                    success = await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
            catch (MySqlException ex)
            {

            }

            return success;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static async Task<bool> AddAttribute(Attribute attribute)
        {
            bool success = false;

            try
            {
                using (DbConnection db = new DbConnection())
                {
                    MySqlCommand cmd = await db.GetCommandAsync();
                    cmd.CommandText = "INSERT INTO attribute (PlantID, AttributeDescription) " +
                                      "VALUES (@PlantID, @AttributeDescription);";
                    cmd.Parameters.AddWithValue("@PlantID", attribute.PlantID);
                    cmd.Parameters.AddWithValue("@AttributeDescription", attribute.AttributeDescription);

                    success = await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
            catch (MySqlException ex)
            {

            } 

            return success;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="regionShapeFile"></param>
        /// <returns></returns>
        public static async Task<bool> AddRegionShapeFile(RegionShapeFile regionShapeFile)
        {
            bool success = false;

            try
            {
                using (DbConnection db = new DbConnection())
                {
                    MySqlCommand cmd = await db.GetCommandAsync();
                    cmd.CommandText = "INSERT INTO region_shape_file (PlantID, LinkToFile) VALUES " +
                                      "(@PlantID, @LinkToFile);";

                    cmd.Parameters.AddWithValue("@PlantID", regionShapeFile.PlantID);
                    cmd.Parameters.AddWithValue("@LinkToFile", regionShapeFile.LinkToFile);

                    success = await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                
            }

            return success;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static async Task<bool> AddImage(Image image)
        {
            bool success = false;

            try
            {
                using (DbConnection db = new DbConnection())
                {
                    MySqlCommand cmd = await db.GetCommandAsync();
                    cmd.CommandText = "INSERT INTO image (PlantID, LinkToFile) " +
                                      "VALUES (@PlantID, @LinkToFile);";

                    cmd.Parameters.AddWithValue("@PlantID", image.PlantID);
                    cmd.Parameters.AddWithValue("@LinkToFile", image.LinkToFile);

                    success = await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
            catch (MySqlException ex)
            {

            }

            return success;
        }
    }
}