1. Tạo project asp webapi: dotnet new webapi -n ten_project
(webapp, webapi, mvc)

2. cài đặt thư viện cho entity framework(data model)
dotnet add package Pomelo.EntityFrameworkCore.MySql (Pomelo MySql Provider)
dotnet add package Microsoft.EntityFrameworkCore.Tools  
(Nếu bị lỗi ở câu lệnh trên thi dung: dotnet tool install --global dotnet-ef)

3. Đồng bộ hóa với database (Tạo ra 1 Migration)
dotnet ef migrations add InitialCreate

Cập nhật vào database
dotnet ef database update