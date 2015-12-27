DROP PROCEDURE [dbo].SearchUsers
GO

/*
	SearchUsers stored procedure is a simple search of the DCCUser table for
	user accounts on the denver chess club website. It has features to search
	by name, email, uscf id, dcc id, membership status, user type, email opt out,
	and pagination
*/
CREATE PROCEDURE
  dbo.SearchUsers
(
	 @query NVARCHAR(50) = NULL -- user's email address
	,@ID INT = NULL -- user's DCC or USCF ID
	,@userType INT = 0 -- user type
	,@membershipStatus INT = NULL -- 0 for non member, 1 for expired, 2 for current
	,@allowEmails BIT = NULL -- does the user accept emails, null returns either 
	,@limit INT = 25 -- the number of results to return, must be between 1 and 100
	,@offset INT = 0 -- pagination
)
AS
	BEGIN
		-- restrict @limit to be between 1 and 100
		IF @limit IS NULL
		BEGIN
			set @limit = 25;
		END

		IF @limit > 50000
		BEGIN
			set @limit = 50000;
		END

		IF @limit < 1
		BEGIN
			set @limit = 1;
		END

		-- prevent offset from being null or below zero
		IF @offset IS NULL OR @offset < 0
		BEGIN
			set @offset = 0;
		END

		IF @userType IS NULL
		BEGIN
			set @userType = 0;
		END

		-- get the current date for membership status comparison
		DECLARE @currentDate DATETIME;
		SET @currentDate = GETDATE();

		-- get total results
		SELECT 
			COUNT(*) OVER() AS 'TotalResults'
			,[dbo].[DCCUsers].ID
			,[dbo].[DCCUsers].Name
			,[dbo].[DCCUsers].Email
			,[dbo].[DCCUsers].USCFNumber
			,[dbo].[DCCUsers].UserType
			,[dbo].[DCCUsers].Expiration
			,[dbo].[DCCUsers].UnclaimedAccount
			,[dbo].[DCCUsers].LastLogon
			,[dbo].[DCCUsers].AllowEmails
			,[dbo].[DCCUsers].Created
			,[dbo].[DCCUsers].Modified
		FROM [dbo].[DCCUsers]

		-- make sure the pgn is not null
		WHERE 1=1

			-- search for users by name or email
			AND (
				(@query IS NULL OR LEN(@query) < 3) 
					OR 
					(
						-- look for the user's name
						([dbo].[DCCUsers].Name LIKE '%' + @query + '%')
						OR
						-- if ignore colors is off, look for white in the black column
						([dbo].[DCCUsers].Email LIKE '%' + @query + '%')						
					)
				)
			-- search for users by their ID number
			AND (
				(@id IS NULL)
					OR
					(
						-- compare the provided ID with the DCC id
						([dbo].[DCCUsers].ID = @id)
						OR
						-- compare the provided id with the user's uscf number
						([dbo].[DCCUsers].USCFNumber = @id)
					)
			)
			-- use bitwise operator to filter by user type
			AND 
				([dbo].[DCCUsers].UserType & @userType = @userType)

			-- filter users by current membership status by comparing
			-- the user's expiration date with the current date
			AND (@membershipStatus IS NULL OR 
				(
					-- no status provided (expiration is null)
					(@membershipStatus = 0 AND [dbo].[DCCUsers].Expiration IS NULL)
					OR
					-- expired members (expiration is less than the current date)
					(@membershipStatus = 1 AND [dbo].[DCCUsers].Expiration < @currentDate)
					OR
					-- current members (expiration is greater than or equal to the current date)
					(@membershipStatus = 2 AND [dbo].[DCCUsers].Expiration >= @currentDate)
				)
			)

			-- Filter by users that allow solicitation emails
			AND (@allowEmails IS NULL OR
				([dbo].[DCCUsers].AllowEmails = @allowEmails)
			)

		-- sort by when the account was created and apply pagination
		ORDER BY [dbo].[DCCUsers].Name ASC OFFSET @offset 
		ROWS FETCH NEXT @limit 
		ROWS ONLY
	END
GO