Drop Table Card;

Create Table Cards(
	CardId BigInt Identity(1,1) Primary Key,
	FirstName Varchar(200) Not Null,
	LastName Varchar(200) Not Null,
	Country Varchar(200) Not Null default(''),
	PicturePath Varchar(200) Not Null default(''),
	CreatedDate DateTime Default(GetDate()) Not null,
	PositionId BigInt
)

Create Table Positions(
	PositionId BigInt Identity(1,1) Primary Key,
	PositionName Varchar(200) Not Null default(''),
)



