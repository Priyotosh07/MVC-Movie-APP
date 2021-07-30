create procedure [dbo].[InsertSongDetails]    
(    
    @Songname varchar(50),    
    @SongAuthor varchar(50),    
    @SongGenre varchar(50),    
    @SongRelaeaseDate date    
)    
As    
BEGIN    
    
 INSERT INTO Songs(Songname,SongAuthor,SongGenre,SongRelaeaseDate)    
 VALUES(@Songname,@SongAuthor,@SongGenre,@SongRelaeaseDate)    
    
END