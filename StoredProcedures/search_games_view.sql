ALTER VIEW search_games AS 
SELECT 
	ID as id,
	Event as event, 
	Site as site, 
	Date as date,
	Round as round,
	White as white_player,
	White_ELO as white_rating,
	White_USCF as white_uscf,
	Black as black_player,
	Black_ELO as black_rating,
	Black_USCF as black_uscf,
	ECO as opening_code,
	Opening as opening_name,
	Variation as opening_variation,
	Result as result,
	Private as private,
	Created as created_date,
	Modified as last_modified,
	PGN as content
FROM ChessGames;
