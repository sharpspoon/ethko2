insert into casestages
(CaseStageName, insdate, FstUser)
values
('Discovery', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('In Trial', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('On Hold', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))