-- From the article "Low risk ways to use F# at work"
-- http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/

-- Create sprocs to use with the SqlInFsharp code
-- If run again, deletes all sprocs and then recreates them.


/* =====================================================
Upsert (merge) a record in the Customer table  

This is a contrived proc, just to demonstrate unit tests!
===================================================== */
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'up_Customer_Upsert' AND ROUTINE_SCHEMA='dbo')
    DROP PROCEDURE up_Customer_Upsert;
GO


CREATE PROCEDURE up_Customer_Upsert
(
    @CustomerId int 
	,@Name nvarchar(50) 
	,@Email nvarchar(50) 
	,@Birthdate datetime
)

AS
BEGIN

    /* ==========================
	Validate the params
	======================= */
	
	DECLARE @ValidationErrors bit
	
	IF @Name IS NULL OR LTRIM(@Name)='' 
		BEGIN
		RAISERROR ('@Name must not be null or blank',16,1);
		SET @ValidationErrors=1
		END

	IF @Email IS NULL OR LTRIM(@Email)='' 
		BEGIN
		RAISERROR ('@Email must not be null or blank',16,1);
		SET @ValidationErrors=1
		END
	
	IF @ValidationErrors =1 
		BEGIN
		RAISERROR ('Validation errors occurred',16,1);
		RETURN
		END

	/* ==========================
	Update or insert
	======================= */

	IF @CustomerId IS NOT NULL
		BEGIN
		UPDATE Customer
		SET 
			Name = @Name
			,Email = @Email
			,Birthdate = @Birthdate
		WHERE CustomerId = @CustomerId 

		RETURN @CustomerId 
		END
	ELSE 
		BEGIN
		INSERT Customer (
			Name 
			,Email
			,Birthdate
			)
		VALUES (			
			@Name
			,@Email
			,@Birthdate
			)
		RETURN SCOPE_IDENTITY()
		END

END
GO


-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: up_Customer_Upsert Succeeded'
ELSE PRINT 'Procedure Creation: up_Customer_Upsert Error on Creation'
GO

