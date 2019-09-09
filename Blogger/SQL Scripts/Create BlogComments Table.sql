CREATE TABLE BlogPostComments
(
	CommentId INT IDENTITY NOT NULL PRIMARY KEY,
	PostId INT NOT NULL,
	Username VARCHAR(50) NOT NULL,
	MessageContent VARCHAR(255) NOT NULL,
	CreatedDate DATETIME2 DEFAULT CURRENT_TIMESTAMP NOT NULL
);

/*
	IDENTITY tells SQL Server to automatically insert a auto incremented number.
*/

