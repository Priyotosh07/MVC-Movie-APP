create procedure [dbo].[DelteSongs]    
(    
    @Id int     
)    
As    
BEGIN    
    DELETE FROM Songs WHERE Id=@Id    
END 