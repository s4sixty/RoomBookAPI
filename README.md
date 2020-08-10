# RoomBookAPI
RoomBook is an API built using .NET CORE 3.1 to manage meetings room bookings

## How to run ?

The API runs in a .NET environmenet on the port 4000 under development circumstances

Swagger has been configured to document the API endpoints at http://localhost:4000/swagger

Most of endpoints are secured with a JWT middleware, you need a jwt token authorization to access the api.

In order to get a generate a token, please use one of the sample users username and password at http://localhost:4000/users/login

### Sample data :

1. user:user

2. ninja:ninja

3. test:test
