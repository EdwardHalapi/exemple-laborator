using CSharp.Choices;
using System;

namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class PaidCaruciorEvent
    {
        public interface IPaidCarucioredEvent { }

        public record PaidCaruciorScucceededEvent : IPaidCarucioredEvent
        {
            public DateTime PublishedDate { get; }

            internal PaidCaruciorScucceededEvent(DateTime publishedDate)
            {
                PublishedDate = publishedDate;
            }
        }

        public record PaidCaruciorFaildEvent : IPaidCarucioredEvent
        {
            public string Reason { get; }

            internal PaidCaruciorFaildEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
