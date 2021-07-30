Create procedure [dbo].[SongDetailsbyID](@Id int)    
AS    
BEGIN    
     SELECT * FROM Songs where id=@Id    
END 