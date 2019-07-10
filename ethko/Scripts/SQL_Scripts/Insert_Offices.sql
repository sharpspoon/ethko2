insert into Offices
(OfficeName, insdate, FstUser)
values
('Birmingham', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))