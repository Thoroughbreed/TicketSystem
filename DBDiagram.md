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
  TPriority int
  TRequesterID int
  TCreatedAt timestamp
  TClosed timestamp
  TClosedByID int
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
Table SubscribedTo as Sub {
  ID int [pk, increment]
  UserID int
  TicketID int
}

// Creating references
// You can also define relaionship separately
// > many-to-one; < one-to-many; - one-to-one
//Ref: U.country_code > countries.code  
//Ref: merchants.country_code > countries.code
