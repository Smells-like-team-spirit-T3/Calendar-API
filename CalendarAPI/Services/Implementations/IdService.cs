using CalendarAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CalendarAPI.Services.Implementations
{
    public class IdService : IIdService
    {
        public string Get9DigitId()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return String.Format("{0:D9}", random);
        }
    }
}
