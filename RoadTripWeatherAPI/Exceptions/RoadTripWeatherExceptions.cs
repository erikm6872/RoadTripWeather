using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadTripWeatherAPI.Exceptions
{
    public class NoRoutesException : Exception
    {
        public NoRoutesException() : base("An error occurred while retrieving Google Maps directions") { }
        public NoRoutesException(string message) : base(message) { }
        public NoRoutesException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidAPIKeyException : Exception
    {
        public InvalidAPIKeyException() : base("API keys are not defined in appsettings.json") { }
        public InvalidAPIKeyException(string message) : base(message) { }
        public InvalidAPIKeyException(string message, Exception inner) : base(message, inner) { }
    }

    public class SourceAPILimitationException : Exception
    {
        public SourceAPILimitationException() : base("Parameters exceed the limitations of a source API") { }
        public SourceAPILimitationException(string message) : base(message) { }
        public SourceAPILimitationException(string message, Exception inner) : base(message, inner) { }
    }
}
