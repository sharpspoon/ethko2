insert into BillingMethods
(BillingMethodName, insdate, FstUser)
values
('Hourly', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Contingency', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Flat Fee', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Mix of Flat Fee and Hourly', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net')),
('Pro Bono', GETDATE(), (select id from AspNetUsers where UserName='system@steelcitysites.net'))