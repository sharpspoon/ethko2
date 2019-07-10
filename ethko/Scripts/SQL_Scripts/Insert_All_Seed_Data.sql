insert into UserTypes
(UserTypeName, insdate)
values
('Attorney', GETDATE()),
('Paralegal', GETDATE()),
('Staff', GETDATE());

insert into aspnetusers
(id, fname, lname, usertypeid, email, emailconfirmed, passwordhash, securitystamp,phonenumberconfirmed, twofactorenabled, lockoutenabled, accessfailedcount, username)
values
('4b6983a2-7178-472b-b7ae-f96470ea8087', 'Robin','Ward',1,'system@steelcitysites.net', 0, 'AEc78Zla/rBy6zDF+GRskTyFtZ/FtsvAMp4BK5L/swVWUfXGFkHGx5SFq10kybaD6Q==','9229c419-dd65-49d2-bb7f-ad9d667221b8',0,0,1,0,'system@steelcitysites.net')

insert into ContactGroups
(ContactGroupName, insdate, FstUser)
values
('Default', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))

insert into casestages
(CaseStageName, insdate, FstUser)
values
('Discovery', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('In Trial', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('On Hold', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))

insert into BillingMethods
(BillingMethodName, insdate, FstUser)
values
('Hourly', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Contingency', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Flat Fee', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Mix of Flat Fee and Hourly', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Pro Bono', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))

insert into Offices
(OfficeName, insdate, FstUser)
values
('Birmingham', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))