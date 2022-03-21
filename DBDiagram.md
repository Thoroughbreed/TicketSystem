//// -- LEVEL 1
//// -- Tables and References

// Creating tables
Table Users as U {
  ID int [pk, increment] // auto-increment
  full_name nvarchar
  display_name nvarchar
  email email
  password nvarchar
  created_at timestamp
  roleID int
}

Table Requesters as Req {
  ID int [pk, increment]
  full_name nvarchar
  email email
  created_at timestamp
  phone nvarchar
}

Ref: U.roleID < R.ID
Table Roles as R {
  ID int [pk, increment]
  Rolename varchar
}

Ref: U.ID > T.TCreatorID
Ref: U.ID > T.TAssignedID
Ref: U.ID > T.TClosedByID
Ref: Req.ID > T.TRequesterID
Table Tickets as T {
  ID int [pk, increment]
  TCaption nvarchar
  TDesc nvarchar
  TCreatorID int
  TAssignedID int
  TCategoryID int
  TPriorityID int
  TStatusID int
  TRequesterID int
  TCreatedAt timestamp
  TClosed timestamp
  TClosedByID int
}

Ref: TC.TicketID < T.ID
Ref: TC.UserID < U.ID
Table TicketChangelog as TC {
  ID int [pk, increment]
  TicketID int
  UserID int
  LogText nvarchar
}

Ref: TEdit.TicketID < T.ID
Ref: TEdit.UserID < U.ID
Table TicketEdits as TEdit {
  ID int [pk, increment]
  UserID int
  TicketID int
  EditedAt timestamp
}

Ref: C.TicketID < T.ID
Ref: C.UserID < U.ID
Table Comments as C {
  ID int [pk, increment]
  UserID int 
  TicketID int
  Comment nvarchar
  CTime timestamp
}

Ref: Sub.TicketID < T.ID
Ref: Sub.UserID < U.ID
Table Subscribers as Sub {
  ID int [pk, increment]
  UserID int
  TicketID int
}

Ref: Cat.ID > T.TCategoryID
Table Category as Cat {
  ID int [pk, increment]
  Category nvarchar
}

Ref: P.ID > T.TPriorityID
Table Priority as P {
  ID int [pk, increment]
  Priority nvarchar
}

Ref: Stat.ID > T.TStatusID
Table Status as Stat {
  ID int [pk, increment]
  Status nvarchar
}

// Creating references
// You can also define relaionship separately
// > many-to-one; < one-to-many; - one-to-one
//Ref: U.country_code > countries.code  
//Ref: merchants.country_code > countries.code

