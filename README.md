# Words_Xamarin_MVVM_C#
A small app (written in C#) to demonstrate the MVVM architecture in the Xamarin crossplatform (iOS, Android) development.

# Warning
Before use, please insert a valid API key to the [ApiCommunicationService file](Words_MVVM/Services/ApiCommunicationService.cs).

It contains some well-known and well-tested external libraries:
- networking: [HttpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netcore-3.1)
- transitions: [NewtonSoft](https://github.com/JamesNK/Newtonsoft.Json)

The app is designed with the Separation of Concern (SoC) and SOLID principles. It's made by the use of Xamarin with the presentation of some platform-specific code. The main goal is to show a really simple MVVM architectural pattern in practice in the crossplatform world. Keywords that used: MVVM, command pattern, converter, data-binding, async - await, dependency injection.

The app contains a lot of comments, so it's easy to pick up the thread. If you have any suggestions, please contact me!

Tests and previews will be coming in the near future.
