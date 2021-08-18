# Open Hackathon Web Application

The open hackathon helps to organize hackathons online, and distribute the prize to the winners with better transparency and less transaction fee.

Receiving prizes from the cross borders in fiat money has issues like submitting documents, and traditional payment processors take high fees. Open hackathon solves that issue.

## Use of Blockchain Technology

We have used blockchain technology for better transparency, and effortless money transfer to the winners.

### Description

User Roles

1. Owner 
2. Hackathon Manager
3. Registered Member

Owner: Can create hackathons.
Hackathon Manager: Can set winner prizes.
Registered Member : Can Register with active hackathon and get prize. 

 
Tool Requirements:

- Visual Studio 2017 or later
- SQL server
- Stratis Fullnode


We have pre-defined users sets to use the system. These wallets needs to have some funds to send transactions.


| FirstName | LastName | Email | Password |WalletAddress|UserRole
| ----------- | -----------|-----------|-----------|-----------|-----------|
|Jon|Smith|jon@email.com|123456|PUHsZ9bmsTfsxe2o45wH4eWA5xuuxmmfe4|1|
|William|James|will@email.com|123456|PD7vaHkcUR7eTW8q2yP7gJ8BUPB1gkQUqK|2|
|Benjamin|State|ben@email.com|123456|PUh7CgvkiDouGTJgFGNMnksdgmFZuAQ1hu|3|
|Ava|Jons|ava@email.com|123456|PTyTQ27BqhSJwneG1adaG7rXSsgYiYbap3|3|
|Olivia|Gates|oliva@email.com|123456|PXNLWytw8T95R3GYZaf1DChbZnuHgsQqq6|3|

Configuration:
1. Set database connection string to the appsettings.json file.
2. Deploy your contract and set contract address to the appsettings.json file.

Demo: 

[![Watch the video](https://i.imgur.com/5WvRB4A.jpg)](https://youtu.be/YlUVwyYkPuA)



