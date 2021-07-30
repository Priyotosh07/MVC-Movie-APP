create procedure [dbo].[UpdateSongDetails]    
(    
    @Id int,    
    @Songname varchar(50),    
    @SongAuthor varchar(50),    
    @SongGenre varchar(50),    
    @SongRelaeaseDate date   
)    
As    
BEGIN    
     UPDATE Songs    
     SET Songname =@Songname,    
     SongAuthor =@SongAuthor,    
     SongGenre = @SongGenre,    
     SongRelaeaseDate =@SongRelaeaseDate    
     Where Id=@Id    
END 