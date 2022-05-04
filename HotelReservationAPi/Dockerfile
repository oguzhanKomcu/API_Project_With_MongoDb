#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/runtime:6.0 

WORKDIR /app
COPY C:\Users\LB 00101\source\repos\API_Project_With_MongoDb\HotelReservationAPi\bin\Release\net6.0\publish .
ENTRYPOINT ["dotnet", "HotelReservationAPi.dll"]
