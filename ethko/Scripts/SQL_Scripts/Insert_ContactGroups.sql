insert into ContactGroups
(ContactGroupName, insdate, FstUser)
values
('Default', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))