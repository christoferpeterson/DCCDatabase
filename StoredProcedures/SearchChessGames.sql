DROP PROCEDURE [dbo].SearchChessGames
GO

CREATE PROCEDURE
  dbo.SearchChessGames
(
	 @white NVARCHAR(50) = NULL -- white player's name (must be 3 or more characters)
	,@black NVARCHAR(50) = NULL -- black player's name (must be 3 or more characters)
	,@whiteRatingStart INT = NULL -- start of white rating range
	,@whiteRatingEnd INT = NULL -- end of white rating range
	,@blackRatingStart INT = NULL -- start of black rating range
	,@blackRatingEnd INT = NULL -- end of black rating range
	,@limit INT = 25 -- the number of results to return, must be between 1 and 100
	,@offset INT = 0 -- pagination
	,@ignoreColors INT = 0 -- search white and black names
)
AS
	BEGIN
		-- restrict @limit to be between 1 and 100
		IF @limit > 100
		BEGIN
			set @limit = 100;
		END

		IF @limit < 1
		BEGIN
			set @limit = 1;
		END

		-- make sure the white rating start is not greater than the end
		IF @whiteRatingStart IS NOT NULL AND @whiteRatingEnd IS NOT NULL
		BEGIN
			IF @whiteRatingStart > @whiteRatingEnd
			BEGIN
				set @whiteRatingEnd = NULL;
			END
		END

		-- make sure the black rating start is not greater than the end
		IF @blackRatingStart IS NOT NULL AND @blackRatingEnd IS NOT NULL
		BEGIN
			IF @blackRatingStart > @blackRatingEnd
			BEGIN
				set @blackRatingEnd = NULL;
			END
		END

		-- get total results
		SELECT 
			COUNT(*) OVER() AS 'TotalResults'
			,[dbo].[ChessGames].ID
			,[dbo].[ChessGames].White
			,[dbo].[ChessGames].White_ELO
			,[dbo].[ChessGames].Black
			,[dbo].[ChessGames].Black_ELO
			,[dbo].[ChessGames].[Date]
		FROM [dbo].[ChessGames]

		-- make sure the pgn is not null
		WHERE 1=1

			-- search for colors based on names
			-- use the ignorecolors parameter to check for white or black players
			-- make sure white is not null and has more than 2 characters
			AND ((@white IS NULL OR LEN(@white) < 3) OR
					-- look for white in the white column
					([dbo].[ChessGames].White LIKE '%' + @white + '%'
					OR
					-- if ignore colors is off, look for white in the black column
					(@ignoreColors = 1 AND [dbo].[ChessGames].Black LIKE '%' + @white + '%'))
				)
			-- make sure black is not null and has more than 2 characters
			AND ((@black IS NULL OR LEN(@black) < 3) OR
					-- look for black in black column
					([dbo].[ChessGames].Black LIKE '%' + @black + '%'
					OR
					-- if ignore colors is on, look for black in the white column
					(@ignoreColors = 1 AND [dbo].[ChessGames].White LIKE '%' + @black + '%'))
				)

			-- search for player ratings with a given rating range
			-- first find white ratings greater than the white rating start
			AND (@whiteRatingStart IS NULL OR
				([dbo].[ChessGames].White_ELO >= @whiteRatingStart)
			)

			-- find white ratings less than the white rating end
			AND (@whiteRatingEnd IS NULL OR
				([dbo].[ChessGames].White_ELO <= @whiteRatingEnd)
			)

			-- find black ratings greater than the black rating start
			AND (@blackRatingStart IS NULL OR
				([dbo].[ChessGames].Black_ELO >= @blackRatingStart)
			)

			-- find black ratings less than the black rating end
			AND (@blackRatingEnd IS NULL OR
				([dbo].[ChessGames].Black_ELO <= @blackRatingEnd)
			)
		
		ORDER BY [dbo].[ChessGames].[Date] DESC OFFSET @offset 
		ROWS FETCH NEXT @limit 
		ROWS ONLY
	END
GO