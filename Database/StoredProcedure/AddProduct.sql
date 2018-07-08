CREATE PROCEDURE [dbo].[AddProduct]
	@type INT,
	@name NVARCHAR(1000),
	@material INT,
	@description NVARCHAR(4000),
	@articul NVARCHAR(256),
	@prices [dbo].[ProductPriceTable] readonly,
	@images [dbo].[ImagesTable] readonly
AS
	DECLARE @id as table(Id INT NOT NULL) 
	INSERT INTO [dbo].[Products](Type,Name,Material,Description,Articul)
	OUTPUT INSERTED.Id INTO @id
	VALUES(@Type,@Name,@Material,@Description,@Articul)

	DECLARE @productId INT;
	SELECT top 1 @productId=Id FROM @id;

	INSERT INTO [dbo].ProductPhotos(ProductId,FileName)
	SELECT @productId, FileName 
	FROM @images

	INSERT INTO [dbo].[ProductPrice] (ProductId,Price,SizeId)
	SELECT @productId, p.Price,p.SizeId 
	FROM @prices p

	SELECT @productId;
