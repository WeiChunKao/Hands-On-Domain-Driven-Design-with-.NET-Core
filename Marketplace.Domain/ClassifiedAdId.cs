﻿using System;

namespace Marketplace.Domain
{
    public class ClassifiedAdId
    {
        private readonly Guid _value;

        public ClassifiedAdId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentNullException(nameof(value), "Classified Ad id cannot be empty");

            _value = value;
        }

        public static implicit operator Guid(ClassifiedAdId self) => self._value;
    }
}