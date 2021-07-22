using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Dapper;
using MvcMovie.Data;

namespace MvcMovie.Models
{
    public class Songs
    {
        

        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Song Name!")]
        public string Songname { get; set; }

        [Required(ErrorMessage = "Enter Song Author Name!")]
        public string SongAuthor { get; set; }

        [Required(ErrorMessage = "Enter Song Genre Name!")]
        public string SongGenre { get; set; }

        [Required(ErrorMessage = "Enter Song Releasing  Date!")]
        public DateTime  SongRelaeaseDate {get; set; }

        static string strConnectionString = "Server=MININT-B3VJKIM;Database=MvcMovieContext1;Trusted_Connection=True;MultipleActiveResultSets=true";





        public static IEnumerable<Songs> GetSongs()
        {
            List<Songs> songlist = new List<Songs>();
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                songlist = con.Query<Songs>("SongDetails").ToList();
            }
            return songlist;
        }
        
        public static Songs GetsongsByID(int? ID)
        {
            Songs songs = new Songs();
            if (ID == null)
                return songs;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", ID);
                songs = con.Query<Songs>("SongDetailsbyID", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return songs;
        }

        public static int AddSongs(Songs songs)
        {
            int rowInserted = 0;
            using(IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Songname",songs.Songname);
                parameters.Add("@SongAuthor",songs.SongAuthor);
                parameters.Add("@SongGenre",songs.SongGenre);
                parameters.Add("@SongRelaeaseDate",songs.SongRelaeaseDate);

                rowInserted = con.Execute("InsertSongDetails", parameters, commandType: CommandType.StoredProcedure);
            }
            return rowInserted;

        }


        public static int UpdateSongList(Songs songs)
        {
            int rowAffected = 0;

            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", songs.Id);
                parameters.Add("@Songname", songs.Songname);
                parameters.Add("@SongAuthor", songs.SongAuthor);
                parameters.Add("@SongGenre", songs.SongGenre);
                parameters.Add("@SongRelaeaseDate", songs.SongRelaeaseDate);
                rowAffected = con.Execute("UpdateSongDetails", parameters, commandType: CommandType.StoredProcedure);
            }

            return rowAffected;
        }


        public static int DeleteSong(int id)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                rowAffected = con.Execute("DelteSongs", parameters, commandType: CommandType.StoredProcedure);

            }

            return rowAffected;
        }



    }
}
