#TLSharp

[![Build status](https://ci.appveyor.com/api/projects/status/95rl618ch5c4h2fa?svg=true)](https://ci.appveyor.com/project/sochix/tlsharp)

Telegram (http://telegram.org) client library implemented in C#. Only basic functionality is currently implemented. **Consider donation to speed up development process.**

It's a perfect fit for any developer who would like to send data directly to Telegram users.

:star: If you :heart: library, please star it! :star:

:exclamation: **Please, don't use it for SPAM!**

:ru: Russian description you can find [here](https://habrahabr.ru/post/277079/)

#Table of contents?

- [How do I add this to my project?](#how-do-i-add-this-to-my-project)
- [Dependencies](#dependencies)
- [Starter Guide](#starter-guide)
  - [Quick configuration](#quick-configuration)
  - [Using TLSharp](#using-tlsharp)
- [Contributing](#contributing)
- [FAQ](#faq)
- [License](#license)

#How do I add this to my project?

Library isn't ready for production usage, that's why no Nu-Get package available.

To use it follow next steps:

1. Clone TLSharp from GitHub
1. Compile source with VS2015
1. Add reference to ```TLSharp.Core.dll``` to your awesome project.

#Dependencies

TLSharp has a few dependenices, most of functionality implemented from scratch.
All dependencies listed in [package.conf file](https://github.com/sochix/TLSharp/blob/master/TLSharp.Core/packages.config).

#Starter Guide

## Quick Configuration
Telegram API isn't that easy to start. You need to do some configuration first.

1. Create a [developer account](https://my.telegram.org/) in Telegram. 
1. Goto [API development tools](https://my.telegram.org/apps) and copy **API_ID** and **API_HASH** from your account to [TelegramClient.cs](https://github.com/sochix/TLSharp/blob/master/TLSharp.Core/TelegramClient.cs)

## Using TLSharp

###Initializing client

To initialize client you need to create a store in which TLSharp will save Session info.

```
var store = new FileSessionStore();
```

Next, create client instance and connect to Telegram server.

```
var client = new TelegramClient(store, "session");
await client.Connect();
```
Now, you can call methods.

All methods except [IsPhoneRegistered](#IsPhoneRegistered) requires to authenticated user. Example usage of all methods you can find in [Tests].

###Supported methods
Currently supported methods:
 - [IsPhoneRegistered - Check if phone is registered in Telegram](#isphoneregistered)
 - [Authenticate user](#authenticate-user)
 - [Get Contact by Phone number](#get-contact-by-phone-number)
 - [Get Contact by Username](#get-contact-by-username)
 - [Send Message to Contact](#send-message-to-contact)
 - [Send Media to Contact](#send-media-to-contact)
 - [Get Messages History for a Contact](#get-messages-history)

####IsPhoneRegistered
Check if phone number registered to Telegram.

_Example_:

```
var result = await client.IsPhoneRegistered(phoneNumber)
```

* phoneNumber - **string**, phone number in international format (eg. 791812312323)

**Returns:** **bool**, is phone registerd in Telegram or not.


####Authenticate user
Authenticate user by phone number and secret code.

_Example_:

```
	var hash = await client.SendCodeRequest(phoneNumber);
    
	var code = "1234"; //code that you receive from Telegram 

	var user = await client.MakeAuth(phoneNumber, hash, code); 
```
* phoneNumber - **string**, phone number in international format (eg. 791812312323)

**Returns:** **User**, authenticated User.

####Get Contact By Phone number
Get user id by phone number.

_Example_:

```
var res = await client.ImportContactByPhoneNumber("791812312323");
```

* phoneNumber - **string**, phone number in international format (eg. 791812312323)

**Returns**: **int?**, User Id or null if no such user. 

####Get Contact By Username
Get user id by userName.

_Example_:

```
var res = await client.ImportByUserName(userName);
```

* userName - **string**, user name  (eg. telegram_bot)

**Returns**: **int?**, User Id or null if no such user. 

####Send Message To Contact
Send text message to specified user

_Example_:

```
await client.SendMessage(userId, message);
```
* userId - **int**, user id
* message - **string**, message

####Send Media To Contact
Send media file to specified contact.

_Example_:

```
var mediaFile = await client.UploadFile(file_name, file);

var res = await client.SendMediaMessage(userId, mediaFile);
```

* file_name - **string**, file name with extension (eg. "file.jpg")
* file - **byte[]**, file content
* userId - **int**, user id
* mediaFile - **InputFile**, reference to uploaded file

**Returns**: **bool**, file sent or not

####Get Messages History
Returns messages history for specified userId.

_Example_:

```
var hist = await client.GetMessagesHistoryForContact(userId, offset, limit);
``` 

* userId - **int**, user id
* offset - **int**, from what index start load history
* limit - **int**, how much items return

**Returns**: **List\<Message\>**, message history

## Contributing

Contributing is highly appreciated!

###How to add new functions

Adding new functions is easy.

* Just create a new Request class in Requests folder.
* Derive it from MTProtoRequest.

Requests specification you can find in [Telegram API](https://core.telegram.org/#api-methods) reference.

_Example_:

```
public class ExampleRequest : MTProtoRequest
{
    private int _someParameter;

    // pass needed parameters through constructor, and save it to private vars
    public InitConnectionRequest(int someParameter)
    {
        _someParameter = someParameter;
    }

    // send all needed params to Telegram
    public override void OnSend(BinaryWriter writer)
    {
        writer.Write(_someParameter); 
    }

    // read a received data from Telegram 
    public override void OnResponse(BinaryReader reader)
    {
        _someParameter = reader.ReadUInt32();
    }

    public override void OnException(Exception exception)
    {
        throw new NotImplementedException();
    }

    public override bool Responded { get; }

    public override bool Confirmed => true;
}
```

More advanced examples you can find in [Requests folder](https://github.com/sochix/TLSharp/tree/master/TLSharp.Core/Requests). 

###What things can I Implement (Project Roadmap)?

* Factor out current TL language implementation, and use [this one](https://github.com/everbytes/SharpTL)
* Add possibility to get current user Chats and Users
* Fix Chat requests (Create, AddUser) 
* Add Updates handling
* Add possibility to work with Channels

# FAQ

#### I get an error MIGRATE_X?

TLSharp library should automatically handle this errors. If you see such errors, pls create a new issue.

#### I get an exception: System.IO.EndOfStreamException: Unable to read beyond the end of the stream. All test methos except that AuthenticationWorks and TestConnection return same error. I did every thing including setting api id and hash, and setting server address.

You should create a Telegram session. See [configuration guide](#sending-messages-set-up)

#### Why I get FLOOD_WAIT error?
It's Telegram restrictions. See [this](https://core.telegram.org/api/errors#420-flood)

#### Why does TLSharp lacks future XXXX?

Now TLSharp is basic realization of Telegram protocol, you can be a contributor or a sponsor to speed-up developemnt of any feature.

#### Nothing helps
Create an issue in project bug tracker.

**Attach this information**:

* Full problem description and exception message
* Stack-trace
* Your code that runs in to this exception

Without information listen above your issue will be closed. 

# License

**Please, provide link to an author when you using library**

The MIT License

Copyright (c) 2015 Ilya Pirozhenko http://www.sochix.ru/

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
