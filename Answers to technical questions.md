# Technical questions

1. How long did you spend on the coding assignment? What would you add to your solution if you had more time? If you didn't spend much time on the coding assignment then use this as an opportunity to explain what you would add.

I've spent several hours for 5 days. I could have done it earlier but I wanted to demonstrate my skills by implementing the
best practices in terms of system design and architecture. The implementation is a bit over-engineered for a simple assignment like this one and I would not go this far if this was the production application. Here are some things I would like to improve regardless:

 * I would not keep the credential information (**CoinMarketCap API key**) in the configuration file. I would use some secure storage like **key vault** instead.
 * If this was the production application I would like to protect it from the malicious users by introducing the **request quotes**. That means application would need to identify the users and provide an authentication mechanism (e.g. API key).
 * Better **logging** through the whole application.
 * While the application is protected from sending the cryptocurrency code in invalid format, nothing prevents the user to input non-existing, non-cryptocurrency or cryptocurrency code not supported by the **CoinMarketCap API**. This can be prevented by reading the list of **supported cryptocurrency codes** from some kind of source (configuration, read from **CoinMarketCap API** and cached in memory etc.) and then validating the input against that list. I would implement it by using the **factory pattern** which would receive the input, validate it and create the instance of **CurrencyCode** type.
 * Better **error handling**. Instead of throwing exceptions I would use the patterns from **railway-oriented programming** concept.
 * I would use the **strongly typed HttpClient** implementation for calling the **CoinMarketCap API** instead of **IHttpClientFactory** in **CoinMarketCapExchangeRatesWebService** as recommended in .NET Core documentation.
 * In order for the application to recover from **transient network errors**, I would like to add the **retry policy** to the **CoinMarketCapExchangeRatesWebService**. It can be achieved by applying the **decorator pattern** to the **CoinMarketCapExchangeRatesWebService** or, even better, use the **Microsoft.Extensions.Http.Polly** NuGet package which would allow me to configure retry policies on the **HttpClient** instances directly during the application startup.
 * In order to improve performance, I could **cache CoinMarketCap API cryptocurrency quotes response** in-memory since **CoinMarketCap API** refreshes their values every minute. I would implement caching by applying the **decorator pattern** to the **CoinMarketCapExchangeRatesWebService**.
 * **Supported quote currency codes** are currently hard-coded in **CoinMarketCapExchangeRatesWebService**. I would like to make this more flexible by **making supported quote currency codes configurable**.
 * Better **handling of errors** from the **CoinMarketCap API**. Again, I would employ the use of patterns from **railway-oriented programming** concept.

2. What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it. 

For C# it's definitely **pattern matching**. It is hard to justify the use of it in this assignment, but I've managed to sneak it in (ExceptionFilter.cs, line 29):

```cs
if (context.Exception is DomainException exception)
{
    context.Result = new ObjectResult(AsErrorResponse(exception))
    {
        StatusCode = (int) HttpStatusCode.BadRequest
    };
    context.ExceptionHandled = true;
}
```

3. How would you track down a performance issue in production? Have you ever had to do this?

I had and I did. It was actually a Java project. I've used the [New Relic](https://newrelic.com/) Application Performance Monitoring Tool. I've also used tools such as JetBrains [dotTrace](https://www.jetbrains.com/profiler/) and [dotMemory](https://www.jetbrains.com/dotmemory/)  to profile the application performance in a QA environment, but never directly in production.

4. What was the latest technical book you have read or tech conference you have been to? What did you learn?

It was the [Hands-On Domain-Driven Design with .NET Core](https://www.amazon.com/gp/product/1788834097/ref=dbs_a_def_rwt_bibl_vppi_i0) by Alexey Zimarev. I've learned how to apply **DDD**,**CQRS** and **EventSourcing** patterns using the latest versions of **C#** language and .**NET Core** framework. 
As a bonus, I've learned how to implement this kind of architecture with the aid of variety modern tools and frameworks such as: **ASP .NET Core**, **EntityFramework Core**, **EventStore**,  **PostgreSQL**, **RavenDB** and **Docker**. 

5. What do you think about this technical assessment?

I think that it could emphasize more on the **domain logic** and not the **integration** with the **3rd party API**. I know that **integrations with external systems** can be hard, but the hardest part of every application is to separate the **business logic** or **application core** from the **system integration**. I think I've managed to accomplish that with my implementation of the assignment solution.

6. Please, describe yourself using JSON. 

```json
{
  "name": "Rade Milovic",
  "age": 29,
  "favoriteQuote": "Give a man a fish and you feed him for a day; teach a man to fish and you feed him for a lifetime.",
  "professional": {
    "occupation": "software engineer",
    "experienceInYears": 6,
    "values": [
      "bringing value to the business",
      "producing quality software",
      "helping others",
      "sharing knowledge"
    ]
  },
  "personal": {
    "hobbies": [
      "spending time with friends",
      "programming",
      "traveling",
      "hiking",
      "reading books",
      "exercising"
    ],
    "interests": [
      "technology",
      "software engineering",
      "building a business",
      "psychology",
      "philosophy",
      "health",
      "fitness",
      "nutrition"
    ],
    "values": [
      "honesty",
      "courage",
      "accountability",
      "modesty",
	  "kindness",
      "independence"
    ]
  }
}
```