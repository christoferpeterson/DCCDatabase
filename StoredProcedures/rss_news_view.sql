CREATE VIEW rss_news AS
SELECT 
	RSSFeedItems.ID as id,
	RSSFeedItems.Title as title,
	RSSFeedItems.Description as description,
	RSSFeedItems.Link as url,
	RSSFeedItems.PublishDate as publish_date,
	RSSFeedItems.Modified as modified_date,
	RSSFeedItems.Created as created_date,
	ManagedFeeds.Location as region,
	ManagedFeeds.Language as language,
	ManagedFeeds.Title as feed_name,
	ManagedFeeds.Link as feed_url,
	ManagedFeeds.ContentType as content_type
FROM RSSFeedItems
LEFT JOIN ManagedFeeds
ON RSSFeedItems.Feed_ID = ManagedFeeds.ID