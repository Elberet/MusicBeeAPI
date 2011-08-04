using System;
using System.Collections.Generic;
using System.Text;

namespace MusicBeePlugin
{
    public sealed class MusicBeeException : Exception
    {
        public MusicBeeException() { }
        public MusicBeeException(string message) : base(message) { }
        public MusicBeeException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
