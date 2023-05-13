# StrativAPIProj
Web API development and testing.


Repository Link GitHub:
https://github.com/saurab90/StrativAPIProj


1. GET API Link:
http://localhost:62742/api/weatherupdate

2. POST API Link:
http://localhost:62742/api/weatherupdate/TempUpdate

POST Request Parameters:
Body => raw =>(JSON Type data)

{
    "CurrentLocation":"Dhaka",
    "Destination":"Faridpur",
    "SearchDate":"2023-05-13"
}


Test Environment: Postman
*NB-Local Environment API Test needed Desktop Postman Agent software.


API Project + User Project:
1. API Controller: "WeatherUpdateController"
2. User Interface Controller: "TemperatureController"
3. Database: No Database need for this project


Instructions:
01: Clone/Download project from git repository.
02: Connect Internet connection PC/Laptop.
03: (Build+Run) the project
04: Poject will show a default page with layout. In page header navbar pannel you will get 2 options:
		4.1) Coolest 10 Districts.
		4.2) Create Request

4.1: Coolest 10 District: It's will get data from our created GET API and show in a list.
4.2: Create Request: It's will POST data to our created POST API and return response.





