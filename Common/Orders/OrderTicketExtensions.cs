namespace QuantConnect.Orders
{
    /// <summary>
    /// Extension methods for <see cref="OrderTicket"/>
    /// </summary>
    public static class OrderTicketExtensions
    {
        /// <summary>
        /// True if the source is not null and the <see cref="OrderStatus"/> is not
        /// <see cref="OrderStatus.None"/> or
        /// <see cref="OrderStatus.Invalid"/> or
        /// <see cref="OrderStatus.Canceled"/>
        /// </summary>
        /// <param name="source">The <see cref="OrderTicket"/> in question</param>
        /// <returns>true or false</returns>
        public static bool IsActive(this OrderTicket source) =>
            source != null && (
                source.Status != OrderStatus.None ||
                source.Status != OrderStatus.Invalid ||
                source.Status != OrderStatus.Canceled);

        /// <summary>
        /// True if the source is null or the <see cref="OrderStatus"/> is
        /// <see cref="OrderStatus.None"/> or
        /// <see cref="OrderStatus.Invalid"/> or
        /// <see cref="OrderStatus.Canceled"/>
        /// </summary>
        /// <param name="source">The <see cref="OrderTicket"/> in question</param>
        /// <returns>true or false</returns>
        public static bool IsNotActive(this OrderTicket source) => !IsActive(source);
    }
}