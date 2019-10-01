Create Table TwitchChannel 
(
Id INTEGER Primary Key AutoIncrement,
ChannelId varchar(10),
Channel nvarchar(25),
CreatedDate datetime,
ModifiedDate datetime,
OwnerUserId varchar(10)
)
