CREATE TABLE ChatMessage (
MId INTEGER Primary Key AutoIncrement,
MessageId varchar(10), 
ChannelId varchar(10),
UserId varchar(10),
UserName nvarchar(50),
ChannelName nvarchar(100),
UserType int, 
IsTurbo int,
[Message] TEXT,
Sentiment double
)